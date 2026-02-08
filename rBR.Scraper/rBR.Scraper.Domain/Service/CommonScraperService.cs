using rBR.BaseLibraries.Domain.Entity;
using rBR.BaseLibraries.Domain.Enumerator;
using rBR.BaseLibraries.Domain.Interface;
using rBR.Scraper.Domain.Interface.ExternalService;
using rBR.Scraper.Domain.Interface.Repository;
using rBR.Scraper.Domain.Interface.Service;
using rBR.Scraper.Domain.Properties;
using rBR.Scraper.Domain.Settings;
using rBR.Scraper.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace rBR.Scraper.Domain.Service
{
    /// <inheritdoc cref="ICommonScraperService"/>
    public class CommonScraperService : CommonService, ICommonScraperService
    {
        private readonly Lazy<ScraperSettings> _scraperSettings;
        private readonly Lazy<ICommonScraperRepository> _commonScraperRepository;

        /// <summary>
        /// The <see cref="CommonScraperService"/>'s constructor.
        /// </summary>
        /// <param name="scraperSettings">The surface of the <see cref="ScraperSettings"/> registered service for distributed access across other services/classes.</param>
        /// <param name="apiFyExternalService">The interface surface of the <see cref="IApiFyExternalService"/> registered service for distributed access across other services/classes.</param>
        /// <param name="baseUnitOfWork">The interface surface of the <see cref="IBaseUnitOfWork"/> registered service for distributed access across other services/classes.</param>
        /// <param name="commonScraperRepository">The interface surface of the <see cref="ICommonScraperRepository"/> registered service for distributed access across other services/classes.</param>
        public CommonScraperService(Lazy<ScraperSettings> scraperSettings, Lazy<IApiFyExternalService> apiFyExternalService, Lazy<IBaseUnitOfWork> baseUnitOfWork, Lazy<ICommonScraperRepository> commonScraperRepository) : base(apiFyExternalService, baseUnitOfWork)
        {
            _scraperSettings = scraperSettings ?? throw new ArgumentNullException(nameof(scraperSettings));
            _commonScraperRepository = commonScraperRepository ?? throw new ArgumentNullException(nameof(commonScraperRepository));
        }

        /// <inheritdoc cref="ICommonScraperService.CreateAsync(List{CommonScraper}, bool)"/>
        public async Task<List<CommonScraper>> CreateAsync(List<CommonScraper> dataModel, bool commit = true)
        {
            SetScraperStatus(ref dataModel);

            for (int i = 0; i < dataModel.Count; i++)
                dataModel[i].Description = (await GetFromApiFyAsync(dataModel[i].DataId)).Description;

            dataModel = [.. (await _commonScraperRepository.Value.AddRangeAsync(dataModel))];

            if (dataModel != null && commit)
                await _baseUnitOfWork.Value.CommitAsync();

            return dataModel;
        }

        /// <inheritdoc cref="ICommonScraperService.GetAsync(Guid)"/>
        public async Task<CommonScraper> GetAsync(Guid id)
        {
            return await _commonScraperRepository.Value.FindAsync(id);
        }

        /// <inheritdoc cref="ICommonScraperService.GetFromApiFyAsync(string)"/>
        public async Task<CommonScraper> GetFromApiFyAsync(string id)
        {
            CommonScraper result = new();

            try
            {
                result = await _apiFyExternalService.Value.GetActorAsync(id);
            }
            catch (Exception ex)
            {
                Exception exception = new(string.Format(BusinessErrorMessages.ApiFyFailedRequest, ex.Message));
                throw;
            }

            return result;
        }

        /// <inheritdoc cref="ICommonScraperService.ListAsync(string, DateTimeOffset?, int?)"/>
        public async Task<List<CommonScraper>> ListAsync(string title, DateTimeOffset? createdAtStart, int? status)
        {
            List<Expression<Func<CommonScraper, bool>>> predicate = [];

            if (!string.IsNullOrWhiteSpace(title))
                predicate.Add(x => x.Title.ToLower().Contains(title.ToLower()));

            if (status.HasValue)
                predicate.Add(x => x.Status == status.Value);

            if (createdAtStart.HasValue)
                predicate.Add(x => x.CreatedAt.Date >= createdAtStart.Value.Date);

            List<CommonScraper> result = await _commonScraperRepository.Value.ListAsync(predicate);

            if (result?.Count > 0)
                result = [.. result.OrderBy(x => x.Status).ThenBy(x => x.Title).ThenByDescending(x => x.CreatedAt)];

            return result;
        }

        /// <inheritdoc cref="ICommonScraperService.ListFromApiFyAsync(string, string, DateTimeOffset?)"/>
        public async Task<List<CommonScraper>> ListFromApiFyAsync(string title, string name, DateTimeOffset? createdAtStart)
        {
            List<CommonScraper> result = [];

            try
            {
                result = await _apiFyExternalService.Value.ListActorsAsync();
            }
            catch (Exception ex)
            {
                Exception exception = new(string.Format(BusinessErrorMessages.ApiFyFailedRequest, ex.Message));
                throw;
            }

            if (result != null)
            {
                if (!string.IsNullOrWhiteSpace(title))
                    result = [.. result.Where(x => x.Title.ToLower().Contains(title.ToLower()))];

                if (!string.IsNullOrWhiteSpace(name))
                    result = [.. result.Where(x => x.Name.ToLower().Contains(name.ToLower()))];

                if (createdAtStart.HasValue)
                    result = [.. result.Where(x => x.CreatedAt >= createdAtStart.Value)];

                if (result.Count > 0)
                    result = [.. result.OrderBy(x => x.Title).ThenBy(x => x.Name).ThenByDescending(x => x.CreatedAt)];
            }

            return result;
        }

        /// <summary>
        /// The method for setting the scrapers' statuses by checking the active configured scrapers.
        /// </summary>
        /// <param name="dataModel">The list of scrapers to have their statuses set.</param>
        private void SetScraperStatus(ref List<CommonScraper> dataModel)
        {
            for (int i = 0; i < dataModel.Count; i++)
            {
                if (!_scraperSettings.Value.ActiveConfiguredScrapers.Contains(dataModel[i].DataId))
                    dataModel[i].Status = (int)BaseStatus.EnumStatus.Inactive;
            }
        }
    }
}
