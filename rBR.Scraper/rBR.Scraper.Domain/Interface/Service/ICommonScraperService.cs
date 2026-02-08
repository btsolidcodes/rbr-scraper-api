using rBR.BaseLibraries.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rBR.Scraper.Domain.Interface.Service
{
    /// <summary>
    /// The class for dealing with the business rules related to the common scraper processes.
    /// </summary>
    public interface ICommonScraperService
    {
        /// <summary>
        /// The method for creating a list of new scraper objects.
        /// </summary>
        /// <param name="dataModel">The model containing the information for the creation process.</param>
        /// <param name="commit">
        /// The flag indifcating if the operation should be committed:
        /// <list type="bullet">
        /// <item>
        /// <description><see langword="True"/> if the operation should be committed, and</description>
        /// </item>
        /// <item>
        /// <description><see langword="False"/> if the operation shouldn't be committed.</description>
        /// </item>
        /// </list>
        /// </param>
        /// <returns>A <see cref="List{T}"/> of <see cref="CommonScraper"/> objects for the result of the process.</returns>
        Task<List<CommonScraper>> CreateAsync(List<CommonScraper> dataModel, bool commit = true);
        /// <summary>
        /// The method for obtaining a single scraper object.
        /// </summary>
        /// <param name="id">The id of the object to be retrieved.</param>
        /// <returns>A <see cref="CommonScraper"/> object for the result of the process.</returns>
        Task<CommonScraper> GetAsync(Guid id);
        /// <summary>
        /// The method for obtaining a single scraper object from APIFY.
        /// </summary>
        /// <param name="id">The id of the object to be retrieved.</param>
        /// <returns>A <see cref="CommonScraper"/> object for the result of the process</returns>
        Task<CommonScraper> GetFromApiFyAsync(string id);
        /// <summary>
        /// The method for listing scrapers filtered by title, createdAt, and status.
        /// </summary>
        /// <param name="title">The title of the scraper (or part of it).</param>
        /// <param name="createdAtStart">The starting date for the createdAt date of the scraper.</param>
        /// <param name="status">The scrapers' status id.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="CommonScraper"/> objects for the result of the process.</returns>
        Task<List<CommonScraper>> ListAsync(string title, DateTimeOffset? createdAtStart, int? status);
        /// <summary>
        /// The method for listing scrapers  filtered by title, name, and createdAt from APIFY.
        /// </summary>
        /// <param name="title">The title of the scraper (or part of it).</param>
        /// <param name="name">The name of the scraper (or part of it).</param>
        /// <param name="createdAtStart">The starting date for the createdAt date of the scraper.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="CommonScraper"/> objects for the result of the process.</returns>
        Task<List<CommonScraper>> ListFromApiFyAsync(string title, string name, DateTimeOffset? createdAtStart);
        /// <summary>
        /// The call for the implementation of the IDisposable interface - disposing of the context.
        /// </summary>
        void Dispose();
    }
}
