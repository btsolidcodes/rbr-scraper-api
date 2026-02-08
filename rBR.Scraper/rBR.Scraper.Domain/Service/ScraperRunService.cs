using rBR.BaseLibraries.Domain.Entity;
using rBR.BaseLibraries.Domain.Interface;
using rBR.Scraper.Domain.Interface.ExternalService;
using rBR.Scraper.Domain.Interface.Repository;
using rBR.Scraper.Domain.Interface.Service;
using rBR.Scraper.Domain.Properties;
using rBR.Scraper.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace rBR.Scraper.Domain.Service
{
    /// <inheritdoc cref="IScraperRunService"/>
    public class ScraperRunService : CommonService, IScraperRunService
    {
        private readonly Lazy<ICommonScraperRepository> _commonScraperRepository;
        private readonly Lazy<IScraperRunRepository> _scraperRunRepository;

        /// <summary>
        /// The <see cref="ScraperRunService"/>'s constructor.
        /// </summary>
        /// <param name="apiFyExternalService">The interface surface of the <see cref="IApiFyExternalService"/> registered service for distributed access across other services/classes.</param>
        /// <param name="baseUnitOfWork">The interface surface of the <see cref="IBaseUnitOfWork"/> registered service for distributed access across other services/classes.</param>
        /// <param name="commonScraperRepository">The interface surface of the <see cref="ICommonScraperRepository"/> registered service for distributed access across other services/classes.</param>
        /// <param name="scraperRunRepository">The interface surface of the <see cref="IScraperRunRepository"/> registered service for distributed access across other services/classes.</param>
        public ScraperRunService(Lazy<IApiFyExternalService> apiFyExternalService, Lazy<IBaseUnitOfWork> baseUnitOfWork, Lazy<ICommonScraperRepository> commonScraperRepository, Lazy<IScraperRunRepository> scraperRunRepository) : base(apiFyExternalService, baseUnitOfWork)
        {
            _commonScraperRepository = commonScraperRepository ?? throw new ArgumentNullException(nameof(commonScraperRepository));
            _scraperRunRepository = scraperRunRepository ?? throw new ArgumentNullException(nameof(scraperRunRepository));
        }

        /// <inheritdoc cref="IScraperRunService.CreateAsync(List{ScraperRun}, bool)"/>
        public async Task<List<ScraperRun>> CreateAsync(List<ScraperRun> dataModel, bool commit = true)
        {
            for (int i = 0; i < dataModel.Count; i++)
            {
                dataModel[i].Scraper = await CheckIfCommonScraperExists(dataModel[i].ScraperId);
                dataModel[i].ScraperId = Guid.Empty;
            }

            List<ScraperRun> existingRuns = await _scraperRunRepository.Value.ListAsync();
            dataModel = [.. dataModel.Where(x => !existingRuns.Any(y => y.DataId == x.DataId))];

            dataModel = [.. await _scraperRunRepository.Value.AddRangeAsync(dataModel)];

            if (dataModel != null && commit)
                await _baseUnitOfWork.Value.CommitAsync();

            return dataModel;
        }

        /// <inheritdoc cref="IScraperRunService.GetAsync(Guid)"/>
        public Task<ScraperRun> GetAsync(Guid id)
        {
            return _scraperRunRepository.Value.FindAsync(id);
        }

        ///<inheritdoc cref="IScraperRunService.ListAsync(Guid, string, DateTimeOffset?, bool?, bool?)"/>
        public async Task<List<ScraperRun>> ListAsync(Guid scraperId, string dataStatus, DateTimeOffset? startedAtstart, bool? imported, bool? importingError)
        {
            List<Expression<Func<ScraperRun, bool>>> predicate = [x => x.ScraperId == scraperId];

            if (!string.IsNullOrWhiteSpace(dataStatus))
                predicate.Add(x => x.DataStatus.Equals(dataStatus, StringComparison.CurrentCultureIgnoreCase));

            if (startedAtstart.HasValue)
                predicate.Add(x => x.StartedAt >= startedAtstart.Value);

            if (imported.HasValue)
                predicate.Add(x => x.Imported == imported.Value);

            if (importingError.HasValue && importingError.Value)
                predicate.Add(x => !string.IsNullOrWhiteSpace(x.ImportingError));

            List<ScraperRun> result = await _scraperRunRepository.Value.ListAsync(predicate, [nameof(ScraperRun.Scraper)]);

            if (result?.Count > 0)
                result = [.. result.OrderBy(x => x.DataStatus).ThenByDescending(x => x.StartedAt)];

            for (int i = 0; i < result.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(result[i].ImportingError))
                {
                    TranslateErrorMessage(result, i);
                }
            }

            return result;
        }

        ///<inheritdoc cref="IScraperRunService.ListFromApiFyAsync(string, string, DateTimeOffset?)"/>
        public async Task<List<ScraperRun>> ListFromApiFyAsync(string actorId, string dataStatus, DateTimeOffset? startedAtstart)
        {
            List<ScraperRun> result = [];

            CommonScraper existingModel = await CheckIfCommonScraperExists(actorId);

            try
            {
                result = await _apiFyExternalService.Value.ListRunsAsync(actorId, dataStatus);
            }
            catch (Exception ex)
            {
                Exception exception = new(string.Format(BusinessErrorMessages.ApiFyFailedRequest, ex.Message));
                throw;
            }

            if (result?.Count > 0)
            {
                if (startedAtstart.HasValue)
                    result = [.. result.Where(x => x.StartedAt >= startedAtstart.Value)];

                if (string.IsNullOrWhiteSpace(dataStatus))
                    result = [.. result.OrderBy(x => x.DataStatus).ThenByDescending(x => x.StartedAt)];
                else
                    result = [.. result.OrderByDescending(x => x.StartedAt)];

                for (int i = 0; i < result.Count; i++)
                    result[i].ScraperId = existingModel.Id;
            }

            return result;
        }

        /// <summary>
        /// The private method for translating the error messages of the scraper runs.
        /// </summary>
        /// <param name="runs">The list of runs to be verified.</param>
        /// <param name="index">The index of the list that's being verified.</param>
        private static void TranslateErrorMessage(List<ScraperRun> runs, int index)
        {
            if (runs[index].ImportingError.Contains('|'))
            {
                string[] errors = runs[index].ImportingError.Split("|");
                errors[1] = $"{BusinessErrorMessages.ApiFyDatasetUnavailable} {errors[1]}";
                runs[index].ImportingError = $"{errors[1]}|{errors[0]}";
            }
            else
            {
                BusinessErrorMessages.Culture = new CultureInfo("pt-BR");
                if (runs[index].ImportingError == BusinessErrorMessages.NotAvailableRunsException)
                {
                    BusinessErrorMessages.Culture = CultureInfo.CurrentCulture;
                    runs[index].ImportingError = BusinessErrorMessages.NotAvailableRunsException;
                }
                BusinessErrorMessages.Culture = new CultureInfo("en-US");
                if (runs[index].ImportingError == BusinessErrorMessages.NotAvailableRunsException)
                {
                    BusinessErrorMessages.Culture = CultureInfo.CurrentCulture;
                    runs[index].ImportingError = BusinessErrorMessages.NotAvailableRunsException;
                }
                BusinessErrorMessages.Culture = new CultureInfo("pt-BR");
                if (runs[index].ImportingError == BusinessErrorMessages.MissingInstagramProfileForInstagram)
                {
                    BusinessErrorMessages.Culture = CultureInfo.CurrentCulture;
                    runs[index].ImportingError = BusinessErrorMessages.MissingInstagramProfileForInstagram;
                }
                BusinessErrorMessages.Culture = new CultureInfo("en-US");
                if (runs[index].ImportingError == BusinessErrorMessages.MissingInstagramProfileForInstagram)
                {
                    BusinessErrorMessages.Culture = CultureInfo.CurrentCulture;
                    runs[index].ImportingError = BusinessErrorMessages.MissingInstagramProfileForInstagram;
                }
            }
        }

        /// <summary>
        /// The method for checking if a given data model already exists in the database.
        /// </summary>
        /// <param name="dataId">The data id of the <see cref="CommonScraper"/> to be found.</param>
        /// <returns>A <see cref="CommonScraper"/> object for the result of the process.</returns>
        private async Task<CommonScraper> CheckIfCommonScraperExists(string dataId)
        {
            CommonScraper existingModel = await _commonScraperRepository.Value.GetAsync([x => x.DataId == dataId]);

            if (existingModel == null)
                throw new KeyNotFoundException(string.Format(BusinessErrorMessages.ObjectIdNotFoundError, nameof(CommonScraper), dataId));

            return existingModel;
        }

        /// <summary>
        /// The method for checking if a given data model already exists in the database.
        /// </summary>
        /// <param name="scraperId">The data id of the <see cref="CommonScraper"/> to be found.</param>
        /// <returns>A <see cref="CommonScraper"/> object for the result of the process.</returns>
        private async Task<CommonScraper> CheckIfCommonScraperExists(Guid scraperId)
        {
            CommonScraper existingModel = await _commonScraperRepository.Value.FindAsync(scraperId);

            if (existingModel == null)
                throw new KeyNotFoundException(string.Format(BusinessErrorMessages.ObjectIdNotFoundError, nameof(CommonScraper), scraperId));

            return existingModel;
        }
    }
}
