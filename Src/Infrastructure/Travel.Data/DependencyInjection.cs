using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Travel.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Travel.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Travel.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureData(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();

                var environment = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

                if(environment.IsEnvironment("Testing"))
                {
                    options.UseInMemoryDatabase("TravelTourTestDatabase");
                }
                else
                {
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                }
            });

            services.AddScoped<IApplicationDbContext>(provider =>
            {
                return provider.GetService<ApplicationDbContext>();
            });

            return services;
        }
    }
}
