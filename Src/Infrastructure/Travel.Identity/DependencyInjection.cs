using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Travel.Identity.Settings;
using Travel.Application.Common.Interfaces;
using Travel.Identity.Services;

namespace Travel.Identity
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthSettings>(configuration.GetSection("AuthSettings"));
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
