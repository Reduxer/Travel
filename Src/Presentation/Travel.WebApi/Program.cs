using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Serilog;
using Serilog.Exceptions;
using Serilog.Events;
using Serilog.Enrichers;
using Serilog.Formatting.Compact;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Travel.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Travel.WebApi
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Assembly", assemblyName.Name)
                .Enrich.WithProperty("Assembly", assemblyName.Version)
                .WriteTo.SQLite(
                    Path.Combine(Environment.CurrentDirectory, "Logs", "log.db"),
                    restrictedToMinimumLevel: LogEventLevel.Information, 
                    storeTimestampInUtc: true)
                .WriteTo.File(
                    new CompactJsonFormatter(), 
                    Path.Combine(Environment.CurrentDirectory, "Logs", "log.json"), 
                    rollingInterval: RollingInterval.Day, 
                    restrictedToMinimumLevel: LogEventLevel.Information)
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting Host");
                var host = CreateHostBuilder(args).Build();

                using var scope = host.Services.CreateScope();
                var services = scope.ServiceProvider;

                try
                {
                    var dbContext = services.GetRequiredService<ApplicationDbContext>();

                    if (dbContext.Database.IsSqlServer())
                    {
                        await dbContext.Database.MigrateAsync();
                    }

                    await ApplicationDbContextSeed.SeedSampleData(dbContext);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error ocuured while migrating or seeding the database");
                    throw;
                }

                await host.RunAsync();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host Terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
