using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Travel.Domain.Settings;
using Travel.Application.Common.Interfaces;
using Travel.Shared.Services;
using Travel.Shared.Files;

namespace Travel.Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrasturctureShared(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();

            return services;
        }
    }
}
