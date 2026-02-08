using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using rBR.BaseLibraries.Data.Internal.Properties;
using rBR.BaseLibraries.Data.Internal.Settings;
using rBR.BaseLibraries.Domain.Entity;
using rBR.Scraper.Data.Internal.ModelConfiguration;
using rBR.Scraper.Domain.Interface.Context;
using System;
using System.Linq;

namespace rBR.Scraper.Data.Internal.Context
{
    /// <inheritdoc cref="IrBRScraperContext"/>
    public class rBRScraperContext : DbContext, IrBRScraperContext
    {
        private readonly string _contextName;
        private readonly Lazy<BaseContextSettings> _contextSettings;
        private readonly Lazy<IHostApplicationLifetime> _hostApplicationLifetime;
        private readonly Lazy<ILogger<rBRScraperContext>> _logger;

        /// <summary>
        /// The <see cref="rBRScraperContext"/>'s constructor.
        /// </summary>
        /// <param name="dbContextOptions">The <see cref="DbContextOptions"/> instance for accessing basic connection settings between the context and the database.</param>
        /// <param name="contextSettings">The <see cref="BaseContextSettings"/> class for registering connection properties for database contexts.</param>
        /// <param name="hostApplicationLifetime">The interface surface for the <see cref="IHostApplicationLifetime"/> services available through dependency injection.</param>
        /// <param name="logger">The interface surface for the <see cref="ILogger{TCategoryName}"/> services available through dependency injection.</param>
        public rBRScraperContext(DbContextOptions dbContextOptions, Lazy<BaseContextSettings> contextSettings, Lazy<IHostApplicationLifetime> hostApplicationLifetime, Lazy<ILogger<rBRScraperContext>> logger) : base(dbContextOptions)
        {
            _contextSettings = contextSettings ?? throw new ArgumentNullException(nameof(contextSettings));
            _hostApplicationLifetime = hostApplicationLifetime ?? throw new ArgumentNullException(nameof(hostApplicationLifetime));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _contextName = nameof(rBRScraperContext);

            Set<InstagramProfileScraperDataset>("InstagramProfileScraperDatasets");
            Set<InstagramScraperDataset>("InstagramScraperDatasets");
            Set<CommonScraper>("CommonScrapers");
            Set<ScraperRun>("ScraperRuns");
        }

        /// <summary>
        /// The override method for setting database context options.
        /// </summary>
        /// <param name="dbContextOptionsBuilder">The <see cref="DbContextOptionsBuilder"/> instance for setting database context build options.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            try
            {
                //Supressing the lazy loading warning for disposed context.
                dbContextOptionsBuilder.ConfigureWarnings(warn => warn.Ignore(CoreEventId.LazyLoadOnDisposedContextWarning));

                //This checks if the context's database connection isn't configured. If it isn't, then it configures it.
                if (!dbContextOptionsBuilder.IsConfigured)
                {
                    if (_contextSettings.Value.ConnectionStrings.FirstOrDefault().DatabaseType == 0)
                        dbContextOptionsBuilder.UseLazyLoadingProxies().UseMySql(_contextSettings.Value.ConnectionStrings.FirstOrDefault(x => x.ContextName == _contextName).ConnectionStringMySQL, MariaDbServerVersion.LatestSupportedServerVersion);
                    else if (_contextSettings.Value.ConnectionStrings.FirstOrDefault().DatabaseType == 1)
                        dbContextOptionsBuilder.UseLazyLoadingProxies().UseSqlServer(_contextSettings.Value.ConnectionStrings.FirstOrDefault(x => x.ContextName == _contextName).ConnectionStringSQLServer);
                }
            }
            catch (Exception ex)
            {
                _logger.Value.LogError(ex, string.Format(DataInternalErrorMessages.ErrorConfiguringContext, _contextName), dbContextOptionsBuilder);
                _hostApplicationLifetime.Value.StopApplication();
            }
        }

        /// <summary>
        /// The overriding method for configuring the database's customized models' properties (detected by the EF Core).
        /// </summary>
        /// <param name="modelBuilder">The API surface of <see cref="ModelBuilder"/> for associating application entities with database table objects.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
                //Making sure the base method is completed before executing the overriding one.
                base.OnModelCreating(modelBuilder);

                modelBuilder.ConfigureInstagramProfileScraperDataset(_contextSettings.Value.ConnectionStrings.FirstOrDefault(x => x.ContextName == _contextName).DatabaseType);
                modelBuilder.ConfigureInstagramScraperDataset(_contextSettings.Value.ConnectionStrings.FirstOrDefault(x => x.ContextName == _contextName).DatabaseType);
                modelBuilder.ConfigureCommonScraper(_contextSettings.Value.ConnectionStrings.FirstOrDefault(x => x.ContextName == _contextName).DatabaseType);
                modelBuilder.ConfigureScraperRun(_contextSettings.Value.ConnectionStrings.FirstOrDefault(x => x.ContextName == _contextName).DatabaseType);
            }
            catch (Exception ex)
            {
                _logger.Value.LogError(ex, string.Format(DataInternalErrorMessages.ErrorModelingContext, _contextName), modelBuilder);
                _hostApplicationLifetime.Value.StopApplication();
            }
        }
    }
}
