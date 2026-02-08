using rBR.BaseLibraries.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rBR.Scraper.Domain.Interface.Service
{
    /// <summary>
    /// The class for dealing with the business rules related to the datasets' processes.
    /// </summary>
    public interface IInstagramDatasetService
    {
        /// <summary>
        /// The method for importing a dataset from APIFY.
        /// </summary>
        /// <param name="datasetDataId">The APIFY id of the dataset.</param>
        /// <returns>A <see cref="InstagramScraperDataset"/> object for the result of the process.</returns>
        Task<List<InstagramScraperDataset>> ImportAsync(string datasetDataId);
        /// <summary>
        /// The method for importing all of the available datasets of a given scraper.
        /// </summary>
        /// <param name="scraperId">The id of the scraper.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="ScraperRun"/> objects for the result of the process.</returns>
        Task<List<ScraperRun>> ImportAllAsync(Guid scraperId);
        /// <summary>
        /// The method for listing Instagram Scraper Dataset items.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="inputUrl">The url of the items of the dataset.</param>
        /// <param name="timestampStart">The starting date for the timestamp of the dataset items.</param>
        /// <param name="status">The items' status id.</param>
        /// <param name="page">The page that will be listed.</param>
        /// <param name="pageSize">The size of the page that will be listed.</param>
        /// <returns>A <see cref="ListItems{T}"/> of <see cref="InstagramScraperDataset"/> objects for the result of the process.</returns>
        Task<ListItems<List<InstagramScraperDataset>>> ListAsync(string datasetId, string inputUrl, DateTimeOffset? timestampStart, int? status, int page, int pageSize);
        /// <summary>
        /// The method for listing Instagram Scraper Dataset items ordered by their Input Url.
        /// </summary>
        /// <param name="runId">The id of the run that owns the dataset items.</param>
        /// <param name="page">The page that will be listed.</param>
        /// <param name="pageSize">The size of the page that will be listed.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="InstagramScraperDataset"/> objects for the result of the process.</returns>
        Task<ListItems<List<InstagramScraperDataset>>> ListByRunAsync(Guid runId, int page, int pageSize);
        /// <summary>
        /// The call for the implementation of the IDisposable interface - disposing of the context.
        /// </summary>
        void Dispose();
    }
}
