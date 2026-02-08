using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using rBR.BaseLibraries.Application.Properties;
using rBR.BaseLibraries.Service.Utils;
using rBR.Scraper.Application.Interface.AppService.Admin;
using rBR.Scraper.Application.Interface.Validator.Admin;
using rBR.Scraper.Application.Model.Admin.View;
using rBR.Scraper.Service.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rBR.Scraper.Service.Areas.Admin.Controllers.V_1_0
{
    /// <summary>
    /// The controller for admin operations regarding the application's datasets.
    /// </summary>
    [Authorize(Policy = AdminPolicy)]
    [Area("Admin")]
    [OpenApiTag("Admin - Instagram Datasets")]
    [ApiVersion("1.0", Deprecated = false)]
    [Route("API/v{version:apiVersion}/[area]/[controller]")]
    [ApiController]
    public class InstagramDatasetController : CustomControllerBase
    {
        private readonly Lazy<IInstagramDatasetAppService> _instagramDatasetAppService;
        private readonly Lazy<IListInstagramScraperDatasetValidator> _listInstagramScraperDatasetValidator;

        /// <summary>
        /// The <see cref="InstagramDatasetController"/>'s constructor.
        /// </summary>
        /// <param name="instagramDatasetAppService">The interface surface of the <see cref="IInstagramDatasetAppService"/> registered service for distributed access across other services/classes.</param>
        /// <param name="listInstagramScraperDatasetValidator">The interface surface of the <see cref="IListInstagramScraperDatasetValidator"/> registered service for distributed access across other services/classes.</param>
        public InstagramDatasetController(Lazy<IInstagramDatasetAppService> instagramDatasetAppService, Lazy<IListInstagramScraperDatasetValidator> listInstagramScraperDatasetValidator)
        {
            _instagramDatasetAppService = instagramDatasetAppService ?? throw new ArgumentNullException(nameof(instagramDatasetAppService));
            _listInstagramScraperDatasetValidator = listInstagramScraperDatasetValidator ?? throw new ArgumentNullException(nameof(listInstagramScraperDatasetValidator));
        }

        /// <summary>
        /// The endpoint for importing a dataset from APIFY.
        /// </summary>
        /// <param name="datasetDataId">The APIFY id of the dataset.</param>
        /// <returns>
        /// <list type="number">
        /// <item>
        /// <description>A List of InstagramScraperDatasetViewModel objects if the process is successful, and</description>
        /// </item>
        /// <item>
        /// <description>A BaseErrorResponse object if the process fails.</description>
        /// </item>
        /// </list>
        /// </returns>
        [HttpPost("{datasetDataId}")]
        [ProducesResponseType(typeof(List<InstagramScraperDatasetViewModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ImportAsync([FromRoute] string datasetDataId)
        {
            CheckEmptyString(nameof(datasetDataId), datasetDataId);

            List<InstagramScraperDatasetViewModel> result = await _instagramDatasetAppService.Value.ImportAsync(datasetDataId);

            return result != null ? Created($"{RequestRoute()}", result) : StatusCode(500, new BaseErrorResponse(500, BaseExceptionMessages.HTTP_500));
        }

        /// <summary>
        /// The endpoint for importing all of the available datasets of a given scraper.
        /// </summary>
        /// <param name="scraperId">The id of the scraper.</param>
        /// <returns>
        /// <list type="number">
        /// <item>
        /// <description>A List of ScraperRunViewModel objects if the process is successful, and</description>
        /// </item>
        /// <item>
        /// <description>A NoContent object type, if there is no available datasets, and</description>
        /// </item>
        /// <item>
        /// <description>A BaseErrorResponse object if the process fails.</description>
        /// </item>
        /// </list>
        /// </returns>
        [HttpPost("[action]/{scraperId}")]
        [ProducesResponseType(typeof(List<ScraperRunViewModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ImportAllAsync([FromRoute] Guid scraperId)
        {
            CheckEmptyGuid(nameof(scraperId), scraperId);

            List<ScraperRunViewModel> result = await _instagramDatasetAppService.Value.ImportAllAsync(scraperId);

            return result?.Count > 0 ? Ok(result) : NoContent();
        }

        /// <summary>
        /// The endpoint for listing Instagram Scraper Dataset items ordered by their Input Url and by their Timestamp Desc.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="inputUrl">The url of the items of the dataset.</param>
        /// <param name="timestampStart">The starting date for the timestamp of the dataset items.</param>
        /// <param name="status">The items' status id:
        /// <list type="bullet">
        /// <item>
        /// <description>1: Active;</description>
        /// </item>
        /// <item>
        /// <description>2: Inactive.</description>
        /// </item>
        /// </list>
        /// </param>
        /// <param name="page">The page that will be listed.</param>
        /// <param name="pageSize">The size of the page that will be listed.</param>
        /// <returns>
        /// <list type="number">
        /// <item>
        /// <description>A List of InstagramScraperDatasetViewModel objects type for the listed objects;</description>
        /// </item>
        /// <item>
        /// <description>A NoContent object type, if no object is found, and</description>
        /// </item>
        /// <item>
        /// <description>An ErrorResponse object if the process fails.</description>
        /// </item>
        /// </list>
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<InstagramScraperDatasetViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListAsync([FromQuery] string datasetId, [FromQuery] string inputUrl, [FromQuery] DateTimeOffset? timestampStart, [FromQuery] int? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 100)
        {
            if (!_listInstagramScraperDatasetValidator.Value.Validate(timestampStart, status, pageSize, out ValidationErrors))
                return UnprocessableEntity(new BaseErrorResponse(ValidationErrors));

            ListItemsViewModel<List<InstagramScraperDatasetViewModel>> result = await _instagramDatasetAppService.Value.ListAsync(datasetId, inputUrl, timestampStart, status, page, pageSize);

            return result?.Total > 0 ? Ok(result) : NoContent();
        }

        /// <summary>
        /// The endpoint for listing Instagram Scraper Dataset items ordered by their Input Url.
        /// </summary>
        /// <param name="runId">The id of the run that owns the dataset items.</param>
        /// <param name="page">The page that will be listed.</param>
        /// <param name="pageSize">The size of the page that will be listed.</param>
        /// <returns>
        /// <list type="number">
        /// <item>
        /// <description>A List of InstagramScraperDatasetViewModel objects type for the listed objects;</description>
        /// </item>
        /// <item>
        /// <description>A NoContent object type, if no object is found, and</description>
        /// </item>
        /// <item>
        /// <description>An ErrorResponse object if the process fails.</description>
        /// </item>
        /// </list>
        /// </returns>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(ListItemsViewModel<List<InstagramScraperDatasetViewModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListByRunAsync([FromQuery] Guid runId, [FromQuery] int page = 1, [FromQuery] int pageSize = 100)
        {
            CheckEmptyGuid(nameof(runId), runId);
            CheckMininumPage(page, 1);
            CheckPageSize(page, 1000);

            ListItemsViewModel<List<InstagramScraperDatasetViewModel>> result = await _instagramDatasetAppService.Value.ListByRunAsync(runId, page, pageSize);

            return result?.Total > 0 ? Ok(result) : NoContent();
        }
    }
}
