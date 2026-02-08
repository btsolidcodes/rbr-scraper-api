using rBR.Scraper.Application.Model.Admin.Request;
using rBR.Scraper.Application.Model.Admin.View;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rBR.Scraper.Application.Interface.AppService.Admin
{
    /// <summary>
    /// The class that gathers the methods for applying transformations' rules to objects related to the common scraper' processes.
    /// </summary>
    public interface ICommonScraperAppService
    {
        /// <summary>
        /// The method for creating a list of new scraper objects.
        /// </summary>
        /// <param name="requestModel">The model containing the information for the creation process.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="CommonScraperViewModel"/> objects for the result of the process.</returns>
        Task<List<CommonScraperViewModel>> CreateAsync(CreateCommonScraperRequestModel requestModel);
        /// <summary>
        /// The method for obtaining a single scraper object.
        /// </summary>
        /// <param name="id">The id of the object to be retrieved.</param>
        /// <returns>A <see cref="CommonScraperViewModel"/> object for the result of the process.</returns>
        Task<CommonScraperViewModel> GetAsync(Guid id);
        /// <summary>
        /// The method for obtaining a single scraper object from APIFY.
        /// </summary>
        /// <param name="id">The id of the object to be retrieved.</param>
        /// <returns>A <see cref="CommonScraperViewModel"/> object for the result of the process</returns>
        Task<CommonScraperFromApiFyViewModel> GetFromApiFyAsync(string id);
        /// <summary>
        /// The method for listing scrapers filtered by title, createdAt, and status.
        /// </summary>
        /// <param name="title">The title of the scraper (or part of it).</param>
        /// <param name="createdAtStart">The starting date for the createdAt date of the scraper.</param>
        /// <param name="status">The scrapers' status id.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="CommonScraperViewModel"/> objects for the result of the process.</returns>
        Task<List<CommonScraperViewModel>> ListAsync(string title, DateTimeOffset? createdAtStart, int? status);
        /// <summary>
        /// The method for listing scrapers  filtered by title, name, and createdAt from APIFY.
        /// </summary>
        /// <param name="title">The title of the scraper (or part of it).</param>
        /// <param name="name">The name of the scraper (or part of it).</param>
        /// <param name="createdAtStart">The starting date for the createdAt date of the scraper.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="CommonScraperViewModel"/> objects for the result of the process.</returns>
        Task<List<CommonScraperFromApiFyViewModel>> ListFromApiFyAsync(string title, string name, DateTimeOffset? createdAtStart);
    }
}
