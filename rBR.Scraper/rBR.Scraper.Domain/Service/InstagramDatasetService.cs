using rBR.BaseLibraries.Domain.Entity;
using rBR.BaseLibraries.Domain.Enumerator;
using rBR.BaseLibraries.Domain.Interface;
using rBR.Scraper.Domain.Interface.ExternalService;
using rBR.Scraper.Domain.Interface.Repository;
using rBR.Scraper.Domain.Interface.Service;
using rBR.Scraper.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace rBR.Scraper.Domain.Service
{
    /// <inheritdoc cref="IInstagramDatasetService"/>
    public class InstagramDatasetService : CommonService, IInstagramDatasetService
    {
        private readonly Lazy<ICommonScraperRepository> _commonScraperRepository;
        private readonly Lazy<IInstagramDatasetRepository> _instagramDatasetRepository;
        private readonly Lazy<IInstagramProfileDatasetRepository> _instagramProfileDatasetRepository;
        private readonly Lazy<IScraperRunRepository> _scraperRunRepository;

        /// <summary>
        /// The <see cref="InstagramDatasetService"/>'s constructor.
        /// </summary>
        /// <param name="apiFyExternalService">The interface surface of the <see cref="IApiFyExternalService"/> registered service for distributed access across other services/classes.</param>
        /// <param name="baseUnitOfWork">The interface surface of the <see cref="IBaseUnitOfWork"/> registered service for distributed access across other services/classes.</param>
        /// <param name="commonScraperRepository">The interface surface of the <see cref="ICommonScraperRepository"/> registered service for distributed access across other services/classes.</param>
        /// <param name="instagramDatasetRepository">The interface surface of the <see cref="IInstagramDatasetRepository"/> registered service for distributed access across other services/classes.</param>
        /// <param name="instagramProfileDatasetRepository">The interface surface of the <see cref="IInstagramProfileDatasetRepository"/> registered service for distributed access across other services/classes.</param>
        /// <param name="scraperRunRepository">The interface surface of the <see cref="IScraperRunRepository"/> registered service for distributed access across other services/classes.</param>
        public InstagramDatasetService(Lazy<IApiFyExternalService> apiFyExternalService,
            Lazy<IBaseUnitOfWork> baseUnitOfWork,
            Lazy<ICommonScraperRepository> commonScraperRepository,
            Lazy<IInstagramDatasetRepository> instagramDatasetRepository,
            Lazy<IInstagramProfileDatasetRepository> instagramProfileDatasetRepository,
            Lazy<IScraperRunRepository> scraperRunRepository) : base(apiFyExternalService, baseUnitOfWork)
        {
            _commonScraperRepository = commonScraperRepository ?? throw new ArgumentNullException(nameof(commonScraperRepository));
            _instagramDatasetRepository = instagramDatasetRepository ?? throw new ArgumentNullException(nameof(instagramDatasetRepository));
            _instagramProfileDatasetRepository = instagramProfileDatasetRepository ?? throw new ArgumentNullException(nameof(instagramProfileDatasetRepository));
            _scraperRunRepository = scraperRunRepository ?? throw new ArgumentNullException(nameof(scraperRunRepository));
        }

        /// <inheritdoc cref="IInstagramDatasetService.ImportAsync(string)"/>
        public async Task<List<InstagramScraperDataset>> ImportAsync(string datasetDataId)
        {
            ScraperRun existingRun = await CheckIfScraperRunExists(datasetDataId, _scraperRunRepository.Value);

            CommonScraper existingScraper = await CheckIfCommonScraperExists(existingRun.ScraperId, _commonScraperRepository.Value);

            CheckIfScraperIsAllowed(existingScraper.Name, Enum.GetName(DatasetType.EnumDatasetType.Instagram_Scraper).Replace('_', '-'));

            List<InstagramProfileScraperDataset> datasetParents = await _instagramProfileDatasetRepository.Value.ListAsync();

            List<InstagramScraperDataset> result = await _apiFyExternalService.Value.GetInstagramScraperDatasetItemsAsync(datasetDataId, existingRun, datasetParents);

            if (result?.Count > 0)
            {
                result = [.. await _instagramDatasetRepository.Value.AddRangeAsync(result)];

                await _baseUnitOfWork.Value.CommitAsync();

                return result;
            }

            return null;
        }

        /// <inheritdoc cref="IInstagramDatasetService.ImportAsync(string)"/>
        public async Task<List<ScraperRun>> ImportAllAsync(Guid scraperId)
        {
            List<ScraperRun> runs = await CheckForAvailableRuns(scraperId, _scraperRunRepository.Value);

            for (int i = 0; i < runs.Count; i++)
            {
                try
                {
                    await ImportAsync(runs[i].DatasetId);
                    runs[i].Imported = true;
                }
                catch (Exception ex)
                {
                    runs[i].ImportingError = ex.Message;
                    _scraperRunRepository.Value.Update(runs[i]);
                    continue;
                }
            }

            await _baseUnitOfWork.Value.CommitAsync();

            return runs;
        }

        /// <inheritdoc cref="IInstagramDatasetService.ListAsync(string, string, DateTimeOffset?, int?, int, int)"/>
        public async Task<ListItems<List<InstagramScraperDataset>>> ListAsync(string datasetId, string inputUrl, DateTimeOffset? timestampStart, int? status, int page, int pageSize)
        {
            List<Expression<Func<InstagramScraperDataset, bool>>> predicate = [];

            if (!string.IsNullOrWhiteSpace(datasetId))
            {
                ScraperRun existingModel = await CheckIfScraperRunExists(datasetId, _scraperRunRepository.Value);
                predicate.Add(x => x.RunId == existingModel.Id);
            }

            if (!string.IsNullOrWhiteSpace(inputUrl))
                predicate.Add(x => x.InputUrl.ToLower().Contains(inputUrl.ToLower()));

            if (timestampStart.HasValue)
                predicate.Add(x => x.Timestamp >= timestampStart.Value);

            if (status.HasValue)
                predicate.Add(x => x.Status == status.Value);
            else
                predicate.Add(x => x.Status == (int)BaseStatus.EnumStatus.Active);

            List<InstagramScraperDataset> result = await _instagramDatasetRepository.Value.ListAsync(predicate);

            if (result?.Count > 0)
            {
                result = [.. result.OrderBy(x => x.InputUrl).ThenByDescending(x => x.Timestamp)];
                int total = result.Count;
                result = [.. result.Skip((page - 1) * pageSize).Take(pageSize)];
                int showing = result.Count;
                return new ListItems<List<InstagramScraperDataset>>(page, pageSize, total, showing, result);
            }

            return null;
        }

        /// <inheritdoc cref="IInstagramDatasetService.ListByRunAsync(Guid, int, int)"/>
        public async Task<ListItems<List<InstagramScraperDataset>>> ListByRunAsync(Guid runId, int page, int pageSize)
        {
            List<InstagramScraperDataset> result = await _instagramDatasetRepository.Value.ListAsync([x => x.RunId == runId]);

            if (result?.Count > 0)
            {
                result = [.. result.OrderBy(x => x.InputUrl).ThenByDescending(x => x.Timestamp)];
                int total = result.Count;
                result = [.. result.Skip((page - 1) * pageSize).Take(pageSize)];
                int showing = result.Count;
                return new ListItems<List<InstagramScraperDataset>>(page, pageSize, total, showing, result);
            }

            return null;
        }
    }
}
