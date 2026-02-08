using rBR.Scraper.Application.Model.Admin.View;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rBR.Scraper.Application.Interface.AppService.Admin
{
    /// <summary>
    /// The class that gathers the methods for applying transformations' rules to objects related to the datasets' processes.
    /// </summary>
    public interface IInstagramDatasetAppService
    {
        /// <summary>
        /// The method for importing a dataset from APIFY.
        /// </summary>
        /// <param name="datasetDataId">The APIFY id of the dataset.</param>
        /// <returns>A <see cref="InstagramScraperDatasetViewModel"/> object for the result of the process.</returns>
        Task<List<InstagramScraperDatasetViewModel>> ImportAsync(string datasetDataId);
        /// <summary>
        /// The method for importing all of the available datasets of a given scraper.
        /// </summary>
        /// <param name="scraperId">The id of the scraper.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="ScraperRunViewModel"/> objects for the result of the process.</returns>
        Task<List<ScraperRunViewModel>> ImportAllAsync(Guid scraperId);
        /// <summary>
        /// The method for listing Instagram Scraper Dataset items.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="inputUrl">The url of the items of the dataset.</param>
        /// <param name="timestampStart">The starting date for the timestamp of the dataset items.</param>
        /// <param name="status">The items' status id.</param>
        /// <param name="page">The page that will be listed.</param>
        /// <param name="pageSize">The size of the page that will be listed.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="InstagramScraperDatasetViewModel"/> objects for the result of the process.</returns>
        Task<ListItemsViewModel<List<InstagramScraperDatasetViewModel>>> ListAsync(string datasetId, string inputUrl, DateTimeOffset? timestampStart, int? status, int page, int pageSize);
        /// <summary>
        /// The method for listing Instagram Scraper Dataset items ordered by their Input Url.
        /// </summary>
        /// <param name="runId">The id of the run that owns the dataset items.</param>
        /// <param name="page">The page that will be listed.</param>
        /// <param name="pageSize">The size of the page that will be listed.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="InstagramScraperDatasetViewModel"/> objects for the result of the process.</returns>
        Task<ListItemsViewModel<List<InstagramScraperDatasetViewModel>>> ListByRunAsync(Guid runId, int page, int pageSize);
    }
}
