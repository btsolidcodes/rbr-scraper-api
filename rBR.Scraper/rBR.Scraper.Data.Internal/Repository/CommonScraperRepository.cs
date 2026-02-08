using rBR.BaseLibraries.Data.Internal.Repository;
using rBR.BaseLibraries.Domain.Entity;
using rBR.Scraper.Data.Internal.Context;
using rBR.Scraper.Domain.Interface.Repository;
using System;

namespace rBR.Scraper.Data.Internal.Repository
{
    /// <inheritdoc cref="ICommonScraperRepository"/>
    public class CommonScraperRepository : BaseRepository<CommonScraper, rBRScraperContext>, ICommonScraperRepository
    {
        /// <summary>
        /// The <see cref="CommonScraperRepository"/>'s constructor.
        /// </summary>
        /// <param name="context">The context class for which this instance is being type-set.</param>
        public CommonScraperRepository(Lazy<rBRScraperContext> context) : base(context)
        {
        }
    }
}
