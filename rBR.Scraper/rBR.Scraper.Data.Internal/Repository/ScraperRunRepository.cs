using rBR.BaseLibraries.Data.Internal.Repository;
using rBR.BaseLibraries.Domain.Entity;
using rBR.Scraper.Data.Internal.Context;
using rBR.Scraper.Domain.Interface.Repository;
using System;

namespace rBR.Scraper.Data.Internal.Repository
{
    /// <inheritdoc cref="IScraperRunRepository"/>
    public class ScraperRunRepository : BaseRepository<ScraperRun, rBRScraperContext>, IScraperRunRepository
    {
        /// <summary>
        /// The <see cref="ScraperRunRepository"/>'s constructor.
        /// </summary>
        /// <param name="context">The context class for which this instance is being type-set.</param>
        public ScraperRunRepository(Lazy<rBRScraperContext> context) : base(context)
        {
        }
    }
}
