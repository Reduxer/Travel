using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Travel.WebApi;
using Travel.Data.Contexts;
using Moq;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.IntegrationTests
{
    public class DatabaseFixture : IDisposable
    {
        private static IConfigurationRoot _configuration;

        private static IServiceScopeFactory _serviceScopeFactory;

        public DatabaseFixture()
        {
            var configBuilder = new ConfigurationBuilder();

            configBuilder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();

            _configuration = configBuilder.Build();

            var startup = new Startup(_configuration);

            var services = new ServiceCollection();
            services.AddSingleton(Mock.Of<IWebHostEnvironment>(env => env.EnvironmentName == "Testing" && env.ApplicationName == "Travel.WebApi"));
            services.AddLogging();

            startup.ConfigureServices(services);

            _serviceScopeFactory = services.BuildServiceProvider()
                .GetService<IServiceScopeFactory>();

            EnsureDatabase();
        }

        private static void EnsureDatabase()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var appDbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            appDbContext.Database.EnsureCreated();
        }

        public static async Task ResetState()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var appDbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            await appDbContext.Database.EnsureDeletedAsync();
        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetService<IMediator>();
            return await mediator.Send(request);
        }

        public static async Task AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            await dbContext.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public static async Task<TEntity> FindAsync<TEntity>(int id) where TEntity : class
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

            return await dbContext.FindAsync<TEntity>(id);
        }

        public void Dispose()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var logger = scope.ServiceProvider.GetService<ILogger<DatabaseFixture>>();
            logger.LogInformation("DatabaseFixture object is being dispose.");
        }
    }
}
