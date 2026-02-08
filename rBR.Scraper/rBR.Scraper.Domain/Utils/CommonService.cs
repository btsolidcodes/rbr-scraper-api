using rBR.BaseLibraries.Domain.Entity;
using rBR.BaseLibraries.Domain.Enumerator;
using rBR.BaseLibraries.Domain.Interface;
using rBR.Scraper.Domain.Interface.ExternalService;
using rBR.Scraper.Domain.Interface.Repository;
using rBR.Scraper.Domain.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;

namespace rBR.Scraper.Domain.Utils
{
    /// <summary>
    /// The class for gathering common business rules methods/processes.
    /// </summary>
    public class CommonService
    {
        internal readonly Lazy<IApiFyExternalService> _apiFyExternalService;
        internal readonly Lazy<IBaseUnitOfWork> _baseUnitOfWork;

        /// <summary>
        /// The <see cref="CommonService"/>'s constructor.
        /// </summary>
        /// <param name="apiFyExternalService">The interface surface of the <see cref="IApiFyExternalService"/> registered service for distributed access across other services/classes.</param>
        /// <param name="baseUnitOfWork">The interface surface of the <see cref="IBaseUnitOfWork"/> registered service for distributed access across other services/classes.</param>
        public CommonService(Lazy<IApiFyExternalService> apiFyExternalService, Lazy<IBaseUnitOfWork> baseUnitOfWork)
        {
            BusinessErrorMessages.Culture = CultureInfo.CurrentCulture;

            _apiFyExternalService = apiFyExternalService ?? throw new ArgumentNullException(nameof(apiFyExternalService));
            _baseUnitOfWork = baseUnitOfWork ?? throw new ArgumentNullException(nameof(baseUnitOfWork));
        }

        /// <summary>
        /// The method for checking if a given data model already exists in the database.
        /// </summary>
        /// <param name="datasetDataId">The dataset id of the <see cref="ScraperRun"/> to be found.</param>
        /// <param name="scraperRunRepository">The interface surface of the <see cref="IScraperRunRepository"/> type.</param>
        /// <returns>A <see cref="ScraperRun"/> object for the result of the process.</returns>
        internal async Task<ScraperRun> CheckIfScraperRunExists(string datasetDataId, IScraperRunRepository scraperRunRepository)
        {
            ScraperRun existingModel = await scraperRunRepository.GetAsync([x => x.DatasetId == datasetDataId]);

            if (existingModel == null)
                throw new KeyNotFoundException(string.Format(BusinessErrorMessages.ObjectIdNotFoundError, nameof(ScraperRun), datasetDataId));

            return existingModel;
        }

        /// <summary>
        /// The method that checks if a scraper run belongs to the type of scraper it should.
        /// </summary>
        /// <param name="name">The name of the scraper for the scraper run.</param>
        /// <param name="allowedScraper">The name of the allowed scraper.</param>
        internal void CheckIfScraperIsAllowed(string name, string allowedScraper)
        {
            if (!name.Equals(allowedScraper, StringComparison.CurrentCultureIgnoreCase))
                throw new Exception(string.Format(BusinessErrorMessages.InvalidDatasetForScraper, name, allowedScraper.ToLower()));
        }

        /// <summary>
        /// The method for checking if a given data model already exists in the database.
        /// </summary>
        /// <param name="scraperId">The id of the <see cref="CommonScraper"/> to be found.</param>
        /// <param name="commonScraperRepository">The interface surface of the <see cref="ICommonScraperRepository"/> type.</param>
        /// <returns>A <see cref="CommonScraper"/> object for the result of the process.</returns>
        internal async Task<CommonScraper> CheckIfCommonScraperExists(Guid scraperId, ICommonScraperRepository commonScraperRepository)
        {
            CommonScraper existingModel = await commonScraperRepository.FindAsync(scraperId) ?? throw new KeyNotFoundException(string.Format(BusinessErrorMessages.ObjectIdNotFoundError, nameof(CommonScraper), scraperId));

            if (existingModel != null && existingModel.Status == (int)BaseStatus.EnumStatus.Inactive)
                throw new KeyNotFoundException(string.Format(BusinessErrorMessages.InactiveScraperForImport, scraperId));

            return existingModel;
        }

        /// <summary>
        /// The method for checking if there are available Runs for a download.
        /// </summary>
        /// <param name="scraperId">The id of the scraper that will have it's datasets downloaded.</param>
        /// <param name="scraperRunRepository">The interface surface of the <see cref="IScraperRunRepository"/> type.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="ScraperRun"/> objects for the result of the process.</returns>
        internal async Task<List<ScraperRun>> CheckForAvailableRuns(Guid scraperId, IScraperRunRepository scraperRunRepository)
        {
            List<ScraperRun> runs = await scraperRunRepository.ListAsync([x => x.ScraperId == scraperId && !x.Imported && string.IsNullOrWhiteSpace(x.ImportingError)]);

            if (runs == null || runs.Count == 0)
                throw new InvalidEnumArgumentException(BusinessErrorMessages.NotAvailableRunsException);

            return runs;
        }

        /// <summary>
        /// The call for the implementation of the IDisposable interface - disposing of the context.
        /// </summary>
        public void Dispose()
        {
            _baseUnitOfWork.Value.Dispose();
        }
    }
}
