using rBR.BaseLibraries.Data.Internal.Repository;
using rBR.BaseLibraries.Domain.Entity;
using rBR.Scraper.Data.Internal.Context;
using rBR.Scraper.Domain.Interface.Repository;
using System;

namespace rBR.Scraper.Data.Internal.Repository
{
    /// <inheritdoc cref="IInstagramDatasetRepository"/>
    public class InstagramDatasetRepository : BaseRepository<InstagramScraperDataset, rBRScraperContext>, IInstagramDatasetRepository
    {
        /// <summary>
        /// The <see cref="InstagramDatasetRepository"/>'s constructor.
        /// </summary>
        /// <param name="context">The context class for which this instance is being type-set.</param>
        public InstagramDatasetRepository(Lazy<rBRScraperContext> context) : base(context)
        {
        }
    }
}
