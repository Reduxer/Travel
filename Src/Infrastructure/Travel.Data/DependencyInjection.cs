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

namespace Travel.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureData(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IApplicationDbContext>(provider =>
            {
                return provider.GetService<ApplicationDbContext>();
            });

            return services;
        }
    }
}
