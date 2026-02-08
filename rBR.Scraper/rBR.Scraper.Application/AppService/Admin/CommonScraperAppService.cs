using rBR.BaseLibraries.Domain.Entity;
using rBR.Scraper.Application.Interface.AppService.Admin;
using rBR.Scraper.Application.Model.Admin.Request;
using rBR.Scraper.Application.Model.Admin.View;
using rBR.Scraper.Domain.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rBR.Scraper.Application.AppService.Admin
{
    /// <inheritdoc cref="ICommonScraperAppService"/>
    public class CommonScraperAppService : ICommonScraperAppService
    {
        private readonly Lazy<ICommonScraperService> _commonScraperService;

        /// <summary>
        /// The <see cref="CommonScraperAppService"/>'s constructor.
        /// </summary>
        /// <param name="commonScraperService">The interface surface of the <see cref="ICommonScraperService"/> registered service for distributed access across other services/classes.</param>
        public CommonScraperAppService(Lazy<ICommonScraperService> commonScraperService)
        {
            _commonScraperService = commonScraperService ?? throw new ArgumentNullException(nameof(commonScraperService));
        }

        /// <inheritdoc cref="ICommonScraperAppService.CreateAsync(CreateCommonScraperRequestModel)"/>
        public async Task<List<CommonScraperViewModel>> CreateAsync(CreateCommonScraperRequestModel requestModel)
        {
            try
            {
                List<CommonScraper> dataModel = requestModel.Scrapers.Select(x => new CommonScraper(x.DataId, x.Title, x.Description, x.Name, x.CreatedAt, x.ModifiedAt)).ToList();

                dataModel = await _commonScraperService.Value.CreateAsync(dataModel);

                return dataModel?.Count > 0 ? [.. dataModel.Select(x => new CommonScraperViewModel(x))] : null;
            }
            finally
            {
                _commonScraperService.Value.Dispose();
            }
        }

        /// <inheritdoc cref="ICommonScraperAppService.GetAsync(Guid)"/>
        public async Task<CommonScraperViewModel> GetAsync(Guid id)
        {
            try
            {
                CommonScraper dataModel = await _commonScraperService.Value.GetAsync(id);

                return dataModel != null ? new CommonScraperViewModel(dataModel) : null;
            }
            finally
            {
                _commonScraperService.Value.Dispose();
            }
        }

        /// <inheritdoc cref="ICommonScraperAppService.GetFromApiFyAsync(string)"/>
        public async Task<CommonScraperFromApiFyViewModel> GetFromApiFyAsync(string id)
        {
            try
            {
                CommonScraper dataModel = await _commonScraperService.Value.GetFromApiFyAsync(id);

                return dataModel != null ? new CommonScraperFromApiFyViewModel(dataModel) : null;
            }
            finally
            {
                _commonScraperService.Value.Dispose();
            }
        }

        /// <inheritdoc cref="ICommonScraperAppService.ListAsync(string, DateTimeOffset?, int?)"/>
        public async Task<List<CommonScraperViewModel>> ListAsync(string title, DateTimeOffset? createdAtStart, int? status)
        {
            try
            {
                List<CommonScraper> dataModel = await _commonScraperService.Value.ListAsync(title, createdAtStart, status);

                return dataModel?.Count > 0 ? [.. dataModel.Select(x => new CommonScraperViewModel(x))] : null;
            }
            finally
            {
                _commonScraperService.Value.Dispose();
            }
        }

        /// <inheritdoc cref="ICommonScraperAppService.ListFromApiFyAsync(string, string, DateTimeOffset?)"/>
        public async Task<List<CommonScraperFromApiFyViewModel>> ListFromApiFyAsync(string title, string name, DateTimeOffset? createdAtStart)
        {
            try
            {
                List<CommonScraper> dataModel = await _commonScraperService.Value.ListFromApiFyAsync(title, name, createdAtStart);

                return dataModel?.Count > 0 ? [.. dataModel.Select(x => new CommonScraperFromApiFyViewModel(x))] : null;
            }
            finally
            {
                _commonScraperService.Value.Dispose();
            }
        }
    }
}
