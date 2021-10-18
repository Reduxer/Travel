using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Travel.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Travel.Application.Common.Interfaces;

namespace Travel.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureData(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite("Data Source=TravelTourDatabase.sqlite3");
            });

            services.AddScoped<IApplicationDbContext>(provider =>
            {
                return provider.GetService<ApplicationDbContext>();
            });

            return services;
        }
    }
}
