using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using rBR.BaseLibraries.Infrastructure.Initialization;
using rBR.Scraper.Data.Internal.Context;

namespace rBR.Scraper.Infrastructure.Initialization
{
    /// <summary>
    /// The static class for starting the database initialization processes.
    /// </summary>
    public static class DatabaseInitialization
    {
        /// <summary>
        /// The static method that checks and runs the databases' migrations.
        /// </summary>
        /// <param name="applicationBuilder">The <see cref="IApplicationBuilder"/> interface surface of the application builder for distributed access across other services/classes.</param>
        public static void MigrateContexts(this IApplicationBuilder applicationBuilder)
        {
            BaseDatabaseInitialization.MigrateContext<rBRScraperContext>(applicationBuilder);
        }

        /// <summary>
        /// The static method of initially inserting required records into the database.
        /// </summary>
        /// <param name="applicationBuilder">The <see cref="IApplicationBuilder"/> interface surface of the application builder for distributed access across other services/classes.</param>
        public static void SeedDatabases(this IApplicationBuilder applicationBuilder)
        {
            using IServiceScope scope = applicationBuilder.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

            rBRScraperContext context = scope.ServiceProvider.GetRequiredService<rBRScraperContext>();

            context.SaveChanges();
        }
    }
}
