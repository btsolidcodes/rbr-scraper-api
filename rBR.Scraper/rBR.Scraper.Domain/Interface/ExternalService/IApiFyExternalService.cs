using rBR.BaseLibraries.Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rBR.Scraper.Domain.Interface.ExternalService
{
    /// <summary>
    /// The class that gathers the methods for external requests to APIFY.
    /// </summary>
    public interface IApiFyExternalService
    {
        /// <summary>
        /// The method for getting an Actor from APIFY.
        /// </summary>
        /// <param name="id">The id of the actor.</param>
        /// <returns>A <see cref="CommonScraper"/> object for the result of the process.</returns>
        Task<CommonScraper> GetActorAsync(string id);
        /// <summary>
        /// The method for listing Actors from APIFY.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of <see cref="CommonScraper"/> for the result of the process.</returns>
        Task<List<CommonScraper>> ListActorsAsync();
        /// <summary>
        /// The method for listing scrapers' runs by actor.
        /// </summary>
        /// <param name="actorId">The id of the actor of the runs.</param>
        /// <param name="dataStatus">The status of the runs.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="ScraperRun"/> objects for the result of the process.</returns>
        Task<List<ScraperRun>> ListRunsAsync(string actorId, string dataStatus);
        /// <summary>
        /// The method for getting a <see cref="InstagramScraperDataset"/> from APIFY.
        /// </summary>
        /// <param name="datasetId">The dataset Id.</param>
        /// <param name="scraperRun">The Scraper Run that owns the dataset.</param>
        /// <param name="datasetParents">The list of dataset parents of the importing dataset.</param>
        /// <returns>A <see cref="InstagramScraperDataset"/> object for the result of the process.</returns>
        Task<List<InstagramScraperDataset>> GetInstagramScraperDatasetItemsAsync(string datasetId, ScraperRun scraperRun, List<InstagramProfileScraperDataset> datasetParents);
        /// <summary>
        /// The method for getting a <see cref="InstagramProfileScraperDataset"/> from APIFY.
        /// </summary>
        /// <param name="datasetId">The dataset Id.</param>
        /// <param name="scraperRun">The Scraper Run that owns the dataset.</param>
        /// <returns>A <see cref="InstagramProfileScraperDataset"/> object for the result of the process.</returns>
        Task<List<InstagramProfileScraperDataset>> GetInstagramProfileScraperDatasetItemsAsync(string datasetId, ScraperRun scraperRun);
    }
}
