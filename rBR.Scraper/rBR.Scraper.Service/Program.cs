using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace rBR.Scraper.Service
{
    /// <summary>
    /// The class that establishes the application's entry point.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The application's entry point method.
        /// </summary>
        /// <param name="args">Application startup parameters.</param>
        public static void Main(string[] args)
        {
            //This creates and starts the application host.
            CreateConfigureRunAppHostBuilder(args);
        }

        /// <summary>
        /// The method in which the application's host will be configured and started, using the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="args">Application startup parameters.</param>
        private static void CreateConfigureRunAppHostBuilder(string[] args)
        {
            //Creating the web app builder.
            WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args);

            //Creating the Startup class that will implement the "deploy" of the host.
            Startup startup = new(webApplicationBuilder);

            //Configuring the application's services through the Startup class.
            startup.ConfigureServices(webApplicationBuilder.Services);

            //Removing the standard log messages for the development environment.
            if (webApplicationBuilder.Environment.IsDevelopment())
            {
                webApplicationBuilder.Logging.ClearProviders();
                webApplicationBuilder.Logging.AddConsole();
            }

            //Building the web application.
            WebApplication webApplication = webApplicationBuilder.Build();

            //Configuring the application's HTTP request pipeline via Startup.
            startup.Configure(webApplication);

            //Checking if there was any failure before application startup.
            if (webApplication.Lifetime.ApplicationStopping.IsCancellationRequested)
                webApplication.WaitForShutdown();
            else
                //Initializing the application.
                webApplication.Run();
        }
    }
}
