using rBR.BaseLibraries.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rBR.Scraper.Domain.Interface.Service
{
    /// <summary>
    /// The class for dealing with the business rules related to the common scraper run processes.
    /// </summary>
    public interface IScraperRunService
    {
        /// <summary>
        /// The method for creating a list of new scraper runs objects.
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
        /// <returns>A <see cref="List{T}"/> of <see cref="ScraperRun"/> objects for the result of the process.</returns>
        Task<List<ScraperRun>> CreateAsync(List<ScraperRun> dataModel, bool commit = true);
        /// <summary>
        /// The method for obtaining a single run object.
        /// </summary>
        /// <param name="id">The id of the object to be retrieved.</param>
        /// <returns>A <see cref="ScraperRun"/> object for the result of the process.</returns>
        Task<ScraperRun> GetAsync(Guid id);
        /// <summary>
        /// The method for listing runs filtered by scraper and startedat date.
        /// </summary>
        /// <param name="scraperId">The runs scraper id.</param>
        /// <param name="dataStatus">The status of the runs.</param>
        /// <param name="startedAtstart">The starting date for the startedAt date.</param>
        /// <param name="imported">The flag indicating if the Scraper Run Dataset was already imported:
        /// <list type="bullet">
        /// <item>
        /// <description>True: if it was already imported;</description>
        /// </item>
        /// <item>
        /// <description>False: if it was not imported yet;</description>
        /// </item>
        /// <item>
        /// <description>Null: if it is not to be filtered by this property.</description>
        /// </item>
        /// </list>
        /// </param>
        /// <param name="importingError">The flag indicating if the Scraper Run Dataset was already imported and there was an error:
        /// <list type="bullet">
        /// <item>
        /// <description>True: if it was already imported and there was an error;</description>
        /// </item>
        /// <item>
        /// <description>False: if it was already imported and there was not an error;</description>
        /// </item>
        /// <item>
        /// <description>Null: if it is not to be filtered by this property.</description>
        /// </item>
        /// </list>
        /// </param>
        /// <returns>A <see cref="List{T}"/> of <see cref="ScraperRun"/> objects for the result of the process.</returns>
        Task<List<ScraperRun>> ListAsync(Guid scraperId, string dataStatus, DateTimeOffset? startedAtstart, bool? imported, bool? importingError);
        /// <summary>
        /// The method for listing scrapers' runs filtered by actor, status, and startedAt from APIFY.
        /// </summary>
        /// <param name="actorId">The id of the actor that owns the runs.</param>
        /// <param name="dataStatus">The status of the runs.</param>
        /// <param name="startedAtstart">The starting date for the startedAt date of the run.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="ScraperRun"/> objects for the result of the process.</returns>
        Task<List<ScraperRun>> ListFromApiFyAsync(string actorId, string dataStatus, DateTimeOffset? startedAtstart);
        /// <summary>
        /// The call for the implementation of the IDisposable interface - disposing of the context.
        /// </summary>
        void Dispose();
    }
}
