using rBR.Scraper.Application.Model.Admin.View;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rBR.Scraper.Application.Interface.AppService.Admin
{
    /// <summary>
    /// The class that gathers the methods for applying transformations' rules to objects related to the datasets' processes.
    /// </summary>
    public interface IInstagramProfileDatasetAppService
    {
        /// <summary>
        /// The method for importing a dataset from APIFY.
        /// </summary>
        /// <param name="datasetDataId">The APIFY id of the dataset.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="InstagramProfileScraperDatasetViewModel"/> objects for the result of the process.</returns>
        Task<List<InstagramProfileScraperDatasetViewModel>> ImportAsync(string datasetDataId);
        /// <summary>
        /// The method for importing all of the available datasets of a given scraper.
        /// </summary>
        /// <param name="scraperId">The id of the scraper.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="ScraperRunViewModel"/> objects for the result of the process.</returns>
        Task<List<ScraperRunViewModel>> ImportAllAsync(Guid scraperId);
        /// <summary>
        /// The method for listing Instagram Profile Scraper Dataset items ordered by their Input Url.
        /// </summary>
        /// <param name="datasetId">The id of the dataset that owns the items that will be listed.</param>
        /// <param name="userName">The user name of the dataset profile items (or part of it).</param>
        /// <param name="fullName">The full name of the dataset profile items (or part of it).</param>
        /// <param name="status">The items' status id.</param>
        /// <param name="page">The page that will be listed.</param>
        /// <param name="pageSize">The size of the page that will be listed.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="InstagramProfileScraperDatasetViewModel"/> objects for the result of the process.</returns>
        Task<ListItemsViewModel<List<InstagramProfileScraperDatasetViewModel>>> ListAsync(string datasetId, string userName, string fullName, int? status, int page, int pageSize);
        /// <summary>
        /// The method for listing Instagram Profile Scraper Dataset items ordered by their Full Name.
        /// </summary>
        /// <param name="runId">The id of the run that owns the dataset items.</param>
        /// <param name="page">The page that will be listed.</param>
        /// <param name="pageSize">The size of the page that will be listed.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="InstagramProfileScraperDatasetViewModel"/> objects for the result of the process.</returns>
        Task<ListItemsViewModel<List<InstagramProfileScraperDatasetViewModel>>> ListByRunAsync(Guid runId, int page, int pageSize);
    }
}
