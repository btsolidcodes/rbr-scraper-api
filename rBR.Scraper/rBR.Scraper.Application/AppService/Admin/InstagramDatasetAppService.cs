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
    /// <inheritdoc cref="IInstagramDatasetAppService"/>
    public class InstagramDatasetAppService : IInstagramDatasetAppService
    {
        private readonly Lazy<IInstagramDatasetService> _instagramDatasetService;

        /// <summary>
        /// The <see cref="InstagramDatasetAppService"/>'s constructor.
        /// </summary>
        /// <param name="instagramDatasetService">The interface surface of the <see cref="IInstagramDatasetService"/> registered service for distributed access across other services/classes.</param>
        public InstagramDatasetAppService(Lazy<IInstagramDatasetService> instagramDatasetService)
        {
            _instagramDatasetService = instagramDatasetService ?? throw new ArgumentNullException(nameof(instagramDatasetService));
        }

        /// <inheritdoc cref="IInstagramDatasetAppService.ImportAsync(string)"/>
        public async Task<List<InstagramScraperDatasetViewModel>> ImportAsync(string datasetDataId)
        {
            try
            {
                List<InstagramScraperDataset> dataModel = await _instagramDatasetService.Value.ImportAsync(datasetDataId);

                return dataModel?.Count > 0 ? [.. dataModel.Select(x => new InstagramScraperDatasetViewModel(x))] : null;
            }
            finally
            {
                _instagramDatasetService.Value.Dispose();
            }
        }

        /// <inheritdoc cref="IInstagramDatasetAppService.ImportAllAsync(Guid)"/>
        public async Task<List<ScraperRunViewModel>> ImportAllAsync(Guid scraperId)
        {
            try
            {
                List<ScraperRun> dataModel = await _instagramDatasetService.Value.ImportAllAsync(scraperId);

                return dataModel?.Count > 0 ? [.. dataModel.Select(x => new ScraperRunViewModel(x))] : null;
            }
            finally
            {
                _instagramDatasetService.Value.Dispose();
            }
        }

        /// <inheritdoc cref="IInstagramDatasetAppService.ListAsync(string, string, DateTimeOffset?, int?, int, int)"/>
        public async Task<ListItemsViewModel<List<InstagramScraperDatasetViewModel>>> ListAsync(string datasetId, string inputUrl, DateTimeOffset? timestampStart, int? status, int page, int pageSize)
        {
            try
            {
                ListItems<List<InstagramScraperDataset>> dataModel = await _instagramDatasetService.Value.ListAsync(datasetId, inputUrl, timestampStart, status, page, pageSize);

                if (dataModel?.Showing > 0)
                {
                    List<InstagramScraperDatasetViewModel> viewModel = [.. dataModel.DatasetItems.Select(x => new InstagramScraperDatasetViewModel(x))];

                    return new ListItemsViewModel<List<InstagramScraperDatasetViewModel>>(viewModel, page, pageSize, dataModel.Total, dataModel.Showing);
                }

                return null;
            }
            finally
            {
                _instagramDatasetService.Value.Dispose();
            }
        }

        /// <inheritdoc cref="IInstagramDatasetAppService.ListByRunAsync(Guid, int, int)"/>
        public async Task<ListItemsViewModel<List<InstagramScraperDatasetViewModel>>> ListByRunAsync(Guid runId, int page, int pageSize)
        {
            try
            {
                ListItems<List<InstagramScraperDataset>> dataModel = await _instagramDatasetService.Value.ListByRunAsync(runId, page, pageSize);

                if (dataModel?.Showing > 0)
                {
                    List<InstagramScraperDatasetViewModel> viewModel = [.. dataModel.DatasetItems.Select(x => new InstagramScraperDatasetViewModel(x))];

                    return new ListItemsViewModel<List<InstagramScraperDatasetViewModel>>(viewModel, page, pageSize, dataModel.Total, dataModel.Showing);
                }

                return null;
            }
            finally
            {
                _instagramDatasetService.Value.Dispose();
            }
        }
    }
}
