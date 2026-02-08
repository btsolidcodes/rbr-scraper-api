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
    [OpenApiTag("Admin - Instagram Profile Datasets")]
    [ApiVersion("1.0", Deprecated = false)]
    [Route("API/v{version:apiVersion}/[area]/[controller]")]
    [ApiController]
    public class InstagramProfileDatasetController : CustomControllerBase
    {
        private readonly Lazy<IInstagramProfileDatasetAppService> _instagramProfileDatasetAppService;
        private readonly Lazy<IListInstagramProfileDatasetValidator> _listInstagramProfileDatasetValidator;

        /// <summary>
        /// The <see cref="InstagramProfileDatasetController"/>'s constructor.
        /// </summary>
        /// <param name="instagramProfileDatasetAppService">The interface surface of the <see cref="IInstagramProfileDatasetAppService"/> registered service for distributed access across other services/classes.</param>
        /// <param name="listInstagramProfileDatasetValidator">The interface surface of the <see cref="IListInstagramProfileDatasetValidator"/> registered service for distributed access across other services/classes.</param>
        public InstagramProfileDatasetController(Lazy<IInstagramProfileDatasetAppService> instagramProfileDatasetAppService, Lazy<IListInstagramProfileDatasetValidator> listInstagramProfileDatasetValidator)
        {
            _instagramProfileDatasetAppService = instagramProfileDatasetAppService ?? throw new ArgumentNullException(nameof(instagramProfileDatasetAppService));
            _listInstagramProfileDatasetValidator = listInstagramProfileDatasetValidator ?? throw new ArgumentNullException(nameof(listInstagramProfileDatasetValidator));
        }

        /// <summary>
        /// The endpoint for importing a dataset from APIFY.
        /// </summary>
        /// <param name="datasetDataId">The APIFY id of the dataset.</param>
        /// <returns>
        /// <list type="number">
        /// <item>
        /// <description>A List of InstagramProfileScraperDatasetViewModel objects if the process is successful, and</description>
        /// </item>
        /// <item>
        /// <description>A BaseErrorResponse object if the process fails.</description>
        /// </item>
        /// </list>
        /// </returns>
        [HttpPost("{datasetDataId}")]
        [ProducesResponseType(typeof(List<InstagramProfileScraperDatasetViewModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ImportAsync([FromRoute] string datasetDataId)
        {
            CheckEmptyString(nameof(datasetDataId), datasetDataId);

            List<InstagramProfileScraperDatasetViewModel> result = await _instagramProfileDatasetAppService.Value.ImportAsync(datasetDataId);

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

            List<ScraperRunViewModel> result = await _instagramProfileDatasetAppService.Value.ImportAllAsync(scraperId);

            return result?.Count > 0 ? Ok(result) : NoContent();
        }

        /// <summary>
        /// The endpoint for listing Instagram Profile Scraper Dataset items ordered by their Full Name.
        /// </summary>
        /// <param name="datasetId">The id of the dataset that owns the items that will be listed.</param>
        /// <param name="userName">The user name of the dataset profile items (or part of it).</param>
        /// <param name="fullName">The full name of the dataset profile items (or part of it).</param>
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
        /// <description>A List of InstagramProfileScraperDatasetViewModel objects type for the listed objects;</description>
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
        [ProducesResponseType(typeof(ListItemsViewModel<List<InstagramProfileScraperDatasetViewModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListAsync([FromQuery] string datasetId, [FromQuery] string userName, [FromQuery] string fullName, [FromQuery] int? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 100)
        {
            if (!_listInstagramProfileDatasetValidator.Value.Validate(status, page, pageSize, out ValidationErrors))
                return UnprocessableEntity(new BaseErrorResponse(ValidationErrors));

            ListItemsViewModel<List<InstagramProfileScraperDatasetViewModel>> result = await _instagramProfileDatasetAppService.Value.ListAsync(datasetId, userName, fullName, status, page, pageSize);

            return result?.Total > 0 ? Ok(result) : NoContent();
        }

        /// <summary>
        /// The endpoint for listing Instagram Profile Scraper Dataset items ordered by their Full Name.
        /// </summary>
        /// <param name="runId">The id of the run that owns the dataset items.</param>
        /// <param name="page">The page that will be listed.</param>
        /// <param name="pageSize">The size of the page that will be listed.</param>
        /// <returns>
        /// <list type="number">
        /// <item>
        /// <description>A List of InstagramProfileScraperDatasetViewModel objects type for the listed objects;</description>
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
        [ProducesResponseType(typeof(ListItemsViewModel<List<InstagramProfileScraperDatasetViewModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListByRunAsync([FromQuery] Guid runId, [FromQuery] int page = 1, [FromQuery] int pageSize = 100)
        {
            CheckEmptyGuid(nameof(runId), runId);
            CheckMininumPage(page, 1);
            CheckPageSize(pageSize, 1000);

            ListItemsViewModel<List<InstagramProfileScraperDatasetViewModel>> result = await _instagramProfileDatasetAppService.Value.ListByRunAsync(runId, page, pageSize);

            return result?.Total > 0 ? Ok(result) : NoContent();
        }
    }
}
