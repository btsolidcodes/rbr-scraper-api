using rBR.BaseLibraries.Domain.Entity;
using rBR.BaseLibraries.Domain.Enumerator;
using rBR.BaseLibraries.Domain.Interface;
using rBR.Scraper.Domain.Interface.ExternalService;
using rBR.Scraper.Domain.Interface.Repository;
using rBR.Scraper.Domain.Interface.Service;
using rBR.Scraper.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace rBR.Scraper.Domain.Service
{
    /// <inheritdoc cref="IInstagramProfileDatasetService"/>
    public class InstagramProfileDatasetService : CommonService, IInstagramProfileDatasetService
    {
        private readonly Lazy<ICommonScraperRepository> _commonScraperRepository;
        private readonly Lazy<IInstagramProfileDatasetRepository> _instagramProfileDatasetRepository;
        private readonly Lazy<IScraperRunRepository> _scraperRunRepository;

        /// <summary>
        /// The <see cref="InstagramProfileDatasetService"/>'s constructor.
        /// </summary>
        /// <param name="apiFyExternalService">The interface surface of the <see cref="IApiFyExternalService"/> registered service for distributed access across other services/classes.</param>
        /// <param name="baseUnitOfWork">The interface surface of the <see cref="IBaseUnitOfWork"/> registered service for distributed access across other services/classes.</param>
        /// <param name="commonScraperRepository">The interface surface of the <see cref="ICommonScraperRepository"/> registered service for distributed access across other services/classes.</param>
        /// <param name="instagramProfileDatasetRepository">The interface surface of the <see cref="IInstagramProfileDatasetRepository"/> registered service for distributed access across other services/classes.</param>
        /// <param name="scraperRunRepository">The interface surface of the <see cref="IScraperRunRepository"/> registered service for distributed access across other services/classes.</param>
        public InstagramProfileDatasetService(Lazy<IApiFyExternalService> apiFyExternalService,
            Lazy<IBaseUnitOfWork> baseUnitOfWork,
            Lazy<ICommonScraperRepository> commonScraperRepository,
            Lazy<IInstagramProfileDatasetRepository> instagramProfileDatasetRepository,
            Lazy<IScraperRunRepository> scraperRunRepository) : base(apiFyExternalService, baseUnitOfWork)
        {
            _commonScraperRepository = commonScraperRepository ?? throw new ArgumentNullException(nameof(commonScraperRepository));
            _instagramProfileDatasetRepository = instagramProfileDatasetRepository ?? throw new ArgumentNullException(nameof(instagramProfileDatasetRepository));
            _scraperRunRepository = scraperRunRepository ?? throw new ArgumentNullException(nameof(scraperRunRepository));
        }

        /// <inheritdoc cref="IInstagramProfileDatasetService.ImportAsync(string)"/>
        public async Task<List<InstagramProfileScraperDataset>> ImportAsync(string datasetDataId)
        {
            ScraperRun existingModel = await CheckIfScraperRunExists(datasetDataId, _scraperRunRepository.Value);

            CheckIfScraperIsAllowed(existingModel.Scraper.Name, Enum.GetName(DatasetType.EnumDatasetType.Instagram_Profile_Scraper).Replace('_', '-'));

            await CheckIfCommonScraperExists(existingModel.ScraperId, _commonScraperRepository.Value);

            List<InstagramProfileScraperDataset> result = await _apiFyExternalService.Value.GetInstagramProfileScraperDatasetItemsAsync(datasetDataId, existingModel);

            if (result?.Count > 0)
            {
                result = [.. await _instagramProfileDatasetRepository.Value.AddRangeAsync(result)];

                await _baseUnitOfWork.Value.CommitAsync();

                return result;
            }

            return null;
        }

        /// <inheritdoc cref="IInstagramProfileDatasetService.ImportAllAsync(Guid)"/>
        public async Task<List<ScraperRun>> ImportAllAsync(Guid scraperId)
        {
            List<ScraperRun> runs = await CheckForAvailableRuns(scraperId, _scraperRunRepository.Value);

            for (int i = 0; i < runs.Count; i++)
            {
                try
                {
                    await ImportAsync(runs[i].DatasetId);
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

        /// <inheritdoc cref="IInstagramProfileDatasetService.ListAsync(string, string, string, int?, int, int)"/>
        public async Task<ListItems<List<InstagramProfileScraperDataset>>> ListAsync(string datasetId, string userName, string fullName, int? status, int page, int pageSize)
        {
            List<Expression<Func<InstagramProfileScraperDataset, bool>>> predicate = [];

            if (!string.IsNullOrWhiteSpace(datasetId))
            {
                ScraperRun existingModel = await CheckIfScraperRunExists(datasetId, _scraperRunRepository.Value);
                predicate.Add(x => x.RunId == existingModel.Id);
            }

            if (!string.IsNullOrWhiteSpace(userName))
                predicate.Add(x => x.UserName.ToLower().Contains(userName.ToLower()));

            if (!string.IsNullOrWhiteSpace(fullName))
                predicate.Add(x => x.FullName.ToLower().Contains(fullName.ToLower()));

            if (status.HasValue)
                predicate.Add(x => x.Status == status.Value);
            else
                predicate.Add(x => x.Status == (int)BaseStatus.EnumStatus.Active);

            List<InstagramProfileScraperDataset> result = await _instagramProfileDatasetRepository.Value.ListAsync(predicate);

            if (result?.Count > 0)
            {
                result = [.. result.OrderBy(x => x.FullName)];
                int total = result.Count;
                result = [.. result.Skip((page - 1) * pageSize).Take(pageSize)];
                int showing = result.Count;
                return new ListItems<List<InstagramProfileScraperDataset>>(page, pageSize, total, showing, result);
            }

            return null;
        }

        /// <inheritdoc cref="IInstagramProfileDatasetService.ListByRunAsync(Guid, int, int)"/>
        public async Task<ListItems<List<InstagramProfileScraperDataset>>> ListByRunAsync(Guid runId, int page, int pageSize)
        {
            List<InstagramProfileScraperDataset> result = await _instagramProfileDatasetRepository.Value.ListAsync([x => x.RunId == runId]);

            if (result?.Count > 0)
            {
                result = [.. result.OrderBy(x => x.FullName)];
                int total = result.Count;
                result = [.. result.Skip((page - 1) * pageSize).Take(pageSize)];
                int showing = result.Count;
                return new ListItems<List<InstagramProfileScraperDataset>>(page, pageSize, total, showing, result);
            }

            return null;
        }
    }
}
