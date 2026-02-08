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
    /// <inheritdoc cref="IScraperRunAppService"/>
    public class ScraperRunAppService : IScraperRunAppService
    {
        private readonly Lazy<IScraperRunService> _scraperRunService;

        /// <summary>
        /// The <see cref="ScraperRunAppService"/>'s constructor.
        /// </summary>
        /// <param name="scraperRunService">The interface surface of the <see cref="IScraperRunService"/> registered service for distributed access across other services/classes.</param>
        public ScraperRunAppService(Lazy<IScraperRunService> scraperRunService)
        {
            _scraperRunService = scraperRunService ?? throw new ArgumentNullException(nameof(scraperRunService));
        }

        /// <inheritdoc cref="IScraperRunAppService.CreateAsync(CreateScraperRunRequestModel)"/>
        public async Task<List<ScraperRunViewModel>> CreateAsync(CreateScraperRunRequestModel requestModel)
        {
            try
            {
                List<ScraperRun> dataModel = [.. requestModel.Runs.Select(x => new ScraperRun(x.DataId, x.DataStatus, x.DatasetId, x.StartedAt, x.FinishedAt, x.ScraperId))];

                dataModel = await _scraperRunService.Value.CreateAsync(dataModel);

                return dataModel?.Count > 0 ? [.. dataModel.Select(x => new ScraperRunViewModel(x))] : null;
            }
            finally
            {
                _scraperRunService.Value.Dispose();
            }
        }

        /// <inheritdoc cref="IScraperRunAppService.GetAsync(Guid)"/>
        public async Task<ScraperRunViewModel> GetAsync(Guid id)
        {
            try
            {
                ScraperRun dataModel = await _scraperRunService.Value.GetAsync(id);

                return dataModel != null ? new ScraperRunViewModel(dataModel) : null;
            }
            finally
            {
                _scraperRunService.Value.Dispose();
            }
        }

        /// <inheritdoc cref="IScraperRunAppService.ListAsync(Guid, string, DateTimeOffset?, bool?, bool?)"/>
        public async Task<List<ScraperRunViewModel>> ListAsync(Guid scraperId, string dataStatus, DateTimeOffset? startedAtstart, bool? imported, bool? importingError)
        {
            try
            {
                List<ScraperRun> dataModel = await _scraperRunService.Value.ListAsync(scraperId, dataStatus, startedAtstart, imported, importingError);

                return dataModel?.Count > 0 ? [.. dataModel.Select(x => new ScraperRunViewModel(x))] : null;
            }
            finally
            {
                _scraperRunService.Value.Dispose();
            }
        }

        /// <inheritdoc cref="IScraperRunAppService.ListFromApiFyAsync(string, string, DateTimeOffset?)"/>
        public async Task<List<ScraperRunFromApiFyViewModel>> ListFromApiFyAsync(string actorId, string dataStatus, DateTimeOffset? startedAtstart)
        {
            try
            {
                List<ScraperRun> dataModel = await _scraperRunService.Value.ListFromApiFyAsync(actorId, dataStatus, startedAtstart);

                return dataModel?.Count > 0 ? [.. dataModel.Select(x => new ScraperRunFromApiFyViewModel(x))] : null;
            }
            finally
            {
                _scraperRunService.Value.Dispose();
            }
        }
    }
}
