using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;
using rBR.BaseLibraries.Service.Extension;
using rBR.Scraper.Infrastructure.Initialization;
using rBR.Scraper.Infrastructure.IoC;
using System.Threading.Tasks;

namespace rBR.Scraper.Service
{
    /// <summary>
    /// The class to configure and initialize the application's host.
    /// </summary>
    public class Startup
    {
        private const string Pattern = "/";
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IConfiguration _configuration;
        private string corsPolicyName;

        /// <summary>
        /// The <see cref="Startup"/>'s constructor.
        /// </summary>
        /// <param name="webApplicationBuilder">The .NET services/web application builder.</param>
        public Startup(WebApplicationBuilder webApplicationBuilder)
        {
            //This sets the current environment into a class variable.
            _hostEnvironment = webApplicationBuilder.Environment;

            //This block sets the application base path and retrieves the static settings files of the application.
            webApplicationBuilder.Configuration.SetBasePath(webApplicationBuilder.Environment.ContentRootPath);
            webApplicationBuilder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            webApplicationBuilder.Configuration.AddJsonFile($"appsettings.{webApplicationBuilder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);

            //This turns on the application environment variables consumption.
            webApplicationBuilder.Configuration.AddEnvironmentVariables();

            //This sets the current builder configuration into a class variable.
            _configuration = webApplicationBuilder.Configuration;
        }

        /// <summary>
        /// The method for registering and configuring the application's services.
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/> interface surface of registered services for distributed access across other services/classes.</param>
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            //This commands the Inversion of Control configuration.
            (serviceCollection, _configuration).RegisterIoC(_hostEnvironment);

            //This commands the configuration of the controllers behaviors.
            serviceCollection.ConfigureControllers();

            //This commands the configuration of the default CORS policy.
            corsPolicyName = serviceCollection.ConfigureCors();

            //This commands the configuration of the Authentification parameters/policies.
            (serviceCollection, _configuration).ConfigureAuthentication();

            //This commands the configuration of the Authorization parameters/policies.
            serviceCollection.ConfigureAuthorization();

            //This commands the configuration of the Swagger documents.
            serviceCollection.ConfigureSwagger();
        }

        /// <summary>
        /// The method for configuring the HTTP request pipeline processes, tools, and functions.
        /// </summary>
        /// <param name="applicationBuilder">The interface surface <see cref="IApplicationBuilder"/> for configuring the behavior of the HTTP request pipeline.</param>
        public void Configure(IApplicationBuilder applicationBuilder)
        {
            //This turns on the usage of static files in the application.
            applicationBuilder.UseStaticFiles();

            //Starting the application's database and stores.
            applicationBuilder.MigrateContexts();

            //Seeding the application's database and stores.
            applicationBuilder.SeedDatabases();

            //This turns on the usage of the routing between the request and the available (most fit) endpoints.
            applicationBuilder.UseRouting();

            //This turns on the requirements of the CORS functionalities with the past configured policy.
            applicationBuilder.UseCors(corsPolicyName);

            //This turns on the requirement of authentication for the public endpoints.
            applicationBuilder.UseAuthentication();

            //This turns on the requirment of authorization levels for the public endpoints.
            applicationBuilder.UseAuthorization();

            //This turns on the usage of the Open API specifications standards for mapping, documenting, presenting, etc., the API.
            applicationBuilder.UseOpenApi(configuration =>
            {
                configuration.PostProcess = (document, _) =>
                {
                    if (_hostEnvironment.IsProduction())
                        document.Schemes = [OpenApiSchema.Https];
                    else
                        document.Schemes = [OpenApiSchema.Http, OpenApiSchema.Https];
                };
            });

            //This turns un the usage of the Swagger UI (and sets its configurations).
            applicationBuilder.UseSwaggerUi(configuration =>
            {
                configuration.DocExpansion = "none";
                configuration.DefaultModelExpandDepth = 3;
                configuration.DefaultModelsExpandDepth = 3;
                configuration.OperationsSorter = "alpha";
                configuration.TagsSorter = "alpha";
            });

            //This sets the usage of the endpoints and its mappings.
            applicationBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute
                    (
                        name: "areas",
                        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                    ).RequireCors(corsPolicyName);

                endpoints.MapGet(Pattern, async context =>
                {
                    await Task.Delay(1);
                    context.Response.Redirect("/swagger");
                });
            });
        }
    }
}
