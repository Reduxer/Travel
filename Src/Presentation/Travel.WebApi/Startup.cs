using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Travel.Data;
using Travel.Shared;
using Travel.Identity;
using Travel.WebApi.Filters;
using Travel.Application;
using Microsoft.AspNetCore.Mvc;
using Travel.WebApi.Helpers;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Travel.Identity.Middlewares;
using Travel.WebApi.Extensions;
using VueCliMiddleware;
using Microsoft.AspNetCore.SpaServices;

namespace Travel.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication(Configuration);
            services.AddInfrastructureData(Configuration);
            services.AddInfrasturctureShared(Configuration);
            services.AddInfrastructureIdentity(Configuration);

            services.AddSpaStaticFiles(options =>
            {
                options.RootPath = "../vue-app/dist";
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddControllers(options => 
            {
                options.Filters.Add(new ApiExceptionFilter());
            })
            .AddNewtonsoftJson(opts =>
            {
                opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGenExtension();
            services.AddApiVersioningExtension();
            services.AddVersionedApiExplorerExtension();
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, 
            IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerExtension(apiVersionDescriptionProvider);
            }

            app.UseStaticFiles();

            app.UseSpaStaticFiles();

            app.UseCors(opts => 
            {
                opts.AllowAnyOrigin();
                opts.AllowAnyHeader();
                opts.AllowAnyMethod();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMiddleware<JwtMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapToVueCliProxy(
                    "{*path}",
                    new SpaOptions { SourcePath = "../vue-app", StartupTimeout = System.TimeSpan.FromSeconds(30)},
                    npmScript: (System.Diagnostics.Debugger.IsAttached) ? "serve" : null,
                    forceKill: true,
                    wsl: false // Set to true if you are using WSL on windows. For other operating systems it will be ignored
                    );
            });
        }
    }
}
