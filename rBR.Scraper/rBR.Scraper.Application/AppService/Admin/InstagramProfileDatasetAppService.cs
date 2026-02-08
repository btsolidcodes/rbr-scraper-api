using rBR.BaseLibraries.Domain.Entity;
using rBR.Scraper.Application.Interface.AppService.Admin;
using rBR.Scraper.Application.Model.Admin.View;
using rBR.Scraper.Domain.Interface.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace rBR.Scraper.Application.AppService.Admin
{
    /// <inheritdoc cref="IInstagramProfileDatasetAppService"/>
    public class InstagramProfileDatasetAppService : IInstagramProfileDatasetAppService
    {
        private readonly Lazy<IInstagramProfileDatasetService> _instagramProfileDatasetService;

        /// <summary>
        /// The <see cref="InstagramProfileDatasetAppService"/>'s constructor.
        /// </summary>
        /// <param name="instagramProfileDatasetService">The interface surface of the <see cref="IInstagramProfileDatasetService"/> registered service for distributed access across other services/classes.</param>
        public InstagramProfileDatasetAppService(Lazy<IInstagramProfileDatasetService> instagramProfileDatasetService)
        {
            _instagramProfileDatasetService = instagramProfileDatasetService ?? throw new ArgumentNullException(nameof(instagramProfileDatasetService));
        }

        /// <inheritdoc cref="IInstagramProfileDatasetAppService.ImportAsync(string)"/>
        public async Task<List<InstagramProfileScraperDatasetViewModel>> ImportAsync(string datasetDataId)
        {
            try
            {
                List<InstagramProfileScraperDataset> dataModel = await _instagramProfileDatasetService.Value.ImportAsync(datasetDataId);

                return dataModel?.Count > 0 ? [.. dataModel.Select(x => new InstagramProfileScraperDatasetViewModel(x))] : null;
            }
            finally
            {
                _instagramProfileDatasetService.Value.Dispose();
            }
        }

        /// <inheritdoc cref="IInstagramProfileDatasetAppService.ImportAllAsync(Guid)"/>
        public async Task<List<ScraperRunViewModel>> ImportAllAsync(Guid scraperId)
        {
            try
            {
                List<ScraperRun> dataModel = await _instagramProfileDatasetService.Value.ImportAllAsync(scraperId);

                return dataModel?.Count > 0 ? [.. dataModel.Select(x => new ScraperRunViewModel(x))] : null;
            }
            finally
            {
                _instagramProfileDatasetService.Value.Dispose();
            }
        }

        /// <inheritdoc cref="IInstagramProfileDatasetAppService.ListAsync(string, string, string, int?, int, int)"/>
        public async Task<ListItemsViewModel<List<InstagramProfileScraperDatasetViewModel>>> ListAsync(string datasetId, string userName, string fullName, int? status, int page, int pageSize)
        {
            try
            {
                ListItems<List<InstagramProfileScraperDataset>> dataModel = await _instagramProfileDatasetService.Value.ListAsync(datasetId, userName, fullName, status, page, pageSize);

                if (dataModel?.Showing > 0)
                {
                    List<InstagramProfileScraperDatasetViewModel> viewModel = [.. dataModel.DatasetItems.Select(x => new InstagramProfileScraperDatasetViewModel(x))];

                    return new ListItemsViewModel<List<InstagramProfileScraperDatasetViewModel>>(viewModel, page, pageSize, dataModel.Total, dataModel.Showing);
                }

                return null;
            }
            finally
            {
                _instagramProfileDatasetService.Value.Dispose();
            }
        }

        /// <inheritdoc cref="IInstagramProfileDatasetAppService.ListByRunAsync(Guid, int, int)"/>
        public async Task<ListItemsViewModel<List<InstagramProfileScraperDatasetViewModel>>> ListByRunAsync(Guid runId, int page, int pageSize)
        {
            try
            {
                ListItems<List<InstagramProfileScraperDataset>> dataModel = await _instagramProfileDatasetService.Value.ListByRunAsync(runId, page, pageSize);

                if (dataModel?.Showing > 0)
                {
                    List<InstagramProfileScraperDatasetViewModel> viewModel = [.. dataModel.DatasetItems.Select(x => new InstagramProfileScraperDatasetViewModel(x))];

                    return new ListItemsViewModel<List<InstagramProfileScraperDatasetViewModel>>(viewModel, page, pageSize, dataModel.Total, dataModel.Showing);
                }

                return null;
            }
            finally
            {
                _instagramProfileDatasetService.Value.Dispose();
            }
        }
    }
}
