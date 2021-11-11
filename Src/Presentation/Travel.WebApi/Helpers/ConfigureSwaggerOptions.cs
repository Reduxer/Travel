using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace Travel.WebApi.Helpers
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach(var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateOpenApiInfo(description));
            }
        }

        private static OpenApiInfo CreateOpenApiInfo(ApiVersionDescription description)
        {
            var openApiInfo = new OpenApiInfo()
            {
                Title = "Travel Tour",
                Version = description.ApiVersion.ToString(),
                Description = "Web Service for Travel Tour",
                Contact = new OpenApiContact()
                {
                    Name = "IT Department",
                    Email = "lesamelb@gmail.com",
                    Url = new Uri("Https://TravelTour.xxx")
                }
            };

            if (description.IsDeprecated)
            {
                openApiInfo.Description += " (Deprecated)";
            }

            return openApiInfo;
        }
    }
}
