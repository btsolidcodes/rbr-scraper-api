using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using rBR.BaseLibraries.Application.Settings;
using rBR.BaseLibraries.Data.Internal.Repository;
using rBR.BaseLibraries.Data.Internal.Settings;
using rBR.BaseLibraries.Domain.Interface;
using rBR.BaseLibraries.Infrastructure.IoC;
using rBR.Scraper.Application.AppService.Admin;
using rBR.Scraper.Application.Validator.Admin;
using rBR.Scraper.Data.External.Service;
using rBR.Scraper.Data.External.Settings;
using rBR.Scraper.Data.Internal.Context;
using rBR.Scraper.Data.Internal.Repository;
using rBR.Scraper.Domain.Interface.Context;
using rBR.Scraper.Domain.Service;
using rBR.Scraper.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace rBR.Scraper.Infrastructure.IoC
{
    /// <summary>
    /// The static class for registering the services, contexts, and settings that the application will access via Dependency Injection.
    /// </summary>
    public static class NativeInjector
    {
        private const string rBRScraperContextName = "rBRScraperContext";

        /// <summary>
        /// The static method for triggering commands to register services and properties for consumption by dependency injection.
        /// </summary>
        /// <param name="tupleServiceCollectionConfiguration">A <see cref="Tuple{T1, T2}"/> of service interface surfaces and application settings for distributed access across other services/classes.</param>
        /// <param name="hostEnvironment">A <see cref="IHostEnvironment"/> interface surface for distributed access across other services/classes.</param>
        public static void RegisterIoC(this (IServiceCollection, IConfiguration) tupleServiceCollectionConfiguration, IHostEnvironment hostEnvironment)
        {
            //This triggers the static settings' registration.
            RegisterSettings(tupleServiceCollectionConfiguration.Item1, tupleServiceCollectionConfiguration.Item2, hostEnvironment);

            //This triggers the common cross cutting classes registration.
            RegisterCrossCutting(tupleServiceCollectionConfiguration.Item1);

            //This triggers the context classes registration.
            RegisterContexts(tupleServiceCollectionConfiguration.Item1);

            //This triggers the services registration.
            RegisterServices(tupleServiceCollectionConfiguration.Item1);
        }

        /// <summary>
        /// The static method for registering the application's static settings.
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/> interface surface of registered services for distributed access across other services/classes.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> interface surface of host settings for distributed access across other services/classes.</param>
        /// <param name="hostEnvironment">A <see cref="IHostEnvironment"/> interface surface for distributed access across other services/classes.</param>
        internal static void RegisterSettings(IServiceCollection serviceCollection, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            List<Type> settingsTypes =
            [
                typeof(BaseAuthSettings),
                typeof(BaseContextSettings),
                typeof(BaseControllersSettings),
                typeof(BaseCorsPolicySettings),
                typeof(BaseSwaggerSettings),
                typeof(ScraperSettings),
                typeof(ApiFySettings),
            ];

            BaseNativeInjector.RegisterSettings(serviceCollection, configuration, settingsTypes, hostEnvironment, "");
        }

        /// <summary>
        /// The private static method for registering the Cross Cutting IoC.
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/> interface surface of registered services for distributed access across other services/classes.</param>
        internal static void RegisterCrossCutting(IServiceCollection serviceCollection)
        {
            BaseNativeInjector.RegisterCommonCrossCutting(serviceCollection);
        }

        /// <summary>
        /// The static method for registering the application's contexts.
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/> interface surface of registered services for distributed access across other services/classes.</param>
        internal static void RegisterContexts(IServiceCollection serviceCollection)
        {
            //Retrieving the Context Settings.
            BaseContextSettings contextSettings = serviceCollection.BuildServiceProvider().GetRequiredService<BaseContextSettings>();

            if (contextSettings.ConnectionStrings.FirstOrDefault().DatabaseType == 0)
                serviceCollection.AddDbContext<rBRScraperContext>(options => options.UseLazyLoadingProxies().UseMySql(contextSettings.ConnectionStrings.FirstOrDefault(x => x.ContextName == rBRScraperContextName).ConnectionStringMySQL, MariaDbServerVersion.LatestSupportedServerVersion));
            else if (contextSettings.ConnectionStrings.FirstOrDefault().DatabaseType == 1)
                serviceCollection.AddDbContext<rBRScraperContext>(options => options.UseLazyLoadingProxies().UseSqlServer(contextSettings.ConnectionStrings.FirstOrDefault(x => x.ContextName == rBRScraperContextName).ConnectionStringSQLServer), ServiceLifetime.Scoped);

            serviceCollection.AddScoped<IrBRScraperContext, rBRScraperContext>();
            serviceCollection.AddScoped<IBaseUnitOfWork, BaseUnitOfWork<rBRScraperContext>>();
        }

        /// <summary>
        /// The static method for registering the application's dependency injection of the services.
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/> interface surface of registered services for distributed access across other services/classes.</param>
        public static void RegisterServices(IServiceCollection serviceCollection)
        {
            List<Type> servicesTypes =
            [
                typeof(ApiFyExternalService),

                typeof(CommonScraperAppService),
                typeof(CommonScraperService),
                typeof(CommonScraperRepository),

                typeof(InstagramDatasetAppService),
                typeof(InstagramDatasetService),
                typeof(InstagramDatasetRepository),

                typeof(InstagramProfileDatasetAppService),
                typeof(InstagramProfileDatasetService),
                typeof(InstagramProfileDatasetRepository),

                typeof(ScraperRunAppService),
                typeof(ScraperRunService),
                typeof(ScraperRunRepository),

                typeof(CreateListCommonScraperValidator),
                typeof(CreateListScraperRunValidator),
                typeof(ListCommonScraperFromApiFyValidator),
                typeof(ListCommonScraperValidator),
                typeof(ListInstagramProfileDatasetValidator),
                typeof(ListInstagramScraperDatasetValidator),
                typeof(ListScraperRunFromApiFyValidator),
                typeof(ListScraperRunValidator),
            ];

            BaseNativeInjector.RegisterServices(serviceCollection, servicesTypes);
        }
    }
}
