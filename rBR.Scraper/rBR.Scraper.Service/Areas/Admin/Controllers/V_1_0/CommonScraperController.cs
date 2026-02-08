using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using rBR.BaseLibraries.Application.Properties;
using rBR.BaseLibraries.Service.Utils;
using rBR.Scraper.Application.Interface.AppService.Admin;
using rBR.Scraper.Application.Interface.Validator.Admin;
using rBR.Scraper.Application.Model.Admin.Request;
using rBR.Scraper.Application.Model.Admin.View;
using rBR.Scraper.Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rBR.Scraper.Service.Areas.Admin.Controllers.V_1_0
{
    /// <summary>
    /// The controller for admin operations regarding the application's common scrapers.
    /// </summary>
    [Authorize(Policy = AdminPolicy)]
    [Area("Admin")]
    [OpenApiTag("Admin - Scrapers")]
    [ApiVersion("1.0", Deprecated = false)]
    [Route("API/v{version:apiVersion}/[area]/Scraper")]
    [ApiController]
    public class CommonScraperController : CustomControllerBase
    {
        private readonly Lazy<ICommonScraperAppService> _commonScraperAppService;
        private readonly Lazy<ICreateListCommonScraperValidator> _createListCommonScraperValidator;
        private readonly Lazy<IListCommonScraperFromApiFyValidator> _listCommonScraperFromApiFyValidator;
        private readonly Lazy<IListCommonScraperValidator> _listCommonScraperValidator;

        /// <summary>
        /// The <see cref="CommonScraperController"/>'s constructor.
        /// </summary>
        /// <param name="commonScraperAppService">The interface surface of the <see cref="ICommonScraperAppService"/> registered service for distributed access across other services/classes.</param>
        /// <param name="createListCommonScraperValidator">The interface surface of the <see cref="ICreateListCommonScraperValidator"/> registered service for distributed access across other services/classes.</param>
        /// <param name="listCommonScraperFromApiFyValidator">The interface surface of the <see cref="IListCommonScraperFromApiFyValidator"/> registered service for distributed access across other services/classes.</param>
        /// <param name="listCommonScraperValidator">The interface surface of the <see cref="IListCommonScraperValidator"/> registered service for distributed access across other services/classes.</param>
        public CommonScraperController(Lazy<ICommonScraperAppService> commonScraperAppService,
            Lazy<ICreateListCommonScraperValidator> createListCommonScraperValidator,
            Lazy<IListCommonScraperFromApiFyValidator> listCommonScraperFromApiFyValidator,
            Lazy<IListCommonScraperValidator> listCommonScraperValidator)
        {
            _commonScraperAppService = commonScraperAppService ?? throw new ArgumentNullException(nameof(commonScraperAppService));
            _createListCommonScraperValidator = createListCommonScraperValidator ?? throw new ArgumentNullException(nameof(createListCommonScraperValidator));
            _listCommonScraperFromApiFyValidator = listCommonScraperFromApiFyValidator ?? throw new ArgumentNullException(nameof(listCommonScraperFromApiFyValidator));
            _listCommonScraperValidator = listCommonScraperValidator ?? throw new ArgumentNullException(nameof(listCommonScraperValidator));
        }

        /// <summary>
        /// The endpoint for creating a list of new scraper objects.
        /// </summary>
        /// <param name="requestModel">The model containing the information for the creation process.</param>
        /// <returns>
        /// <list type="number">
        /// <item>
        /// <description>A List of CommonScraperViewModel objects if the process is successful, and</description>
        /// </item>
        /// <item>
        /// <description>A BaseErrorResponse object if the process fails.</description>
        /// </item>
        /// </list>
        /// </returns>
        [HttpPost]
        [ProducesResponseType(typeof(List<CommonScraperViewModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCommonScraperRequestModel requestModel)
        {
            TreatScrapersDateTimeOffset(ref requestModel, 3);

            if (!_createListCommonScraperValidator.Value.Validate(requestModel, out ValidationErrors))
                return UnprocessableEntity(new BaseErrorResponse(ValidationErrors));

            List<CommonScraperViewModel> result = await _commonScraperAppService.Value.CreateAsync(requestModel);

            return result != null ? Created($"{RequestRoute()}/{result.FirstOrDefault().Id}", result) : StatusCode(500, new BaseErrorResponse(500, BaseExceptionMessages.HTTP_500));
        }

        /// <summary>
        /// The endpoint for obtaining a single scraper object.
        /// </summary>
        /// <param name="id">The id of the object to be retrieved.</param>
        /// <returns>
        /// <list type="number">
        /// <item>
        /// <description>A CommonScraperViewModel object type for the found object, if there is an entry for the given id;</description>
        /// </item>
        /// <item>
        /// <description>A NotFound object type, if no object is found, and</description>
        /// </item>
        /// <item>
        /// <description>An ErrorResponse object if the process fails.</description>
        /// </item>
        /// </list>
        /// </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CommonScraperViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            CheckEmptyGuid(nameof(id), id);

            CommonScraperViewModel result = await _commonScraperAppService.Value.GetAsync(id);

            return result != null ? Ok(result) : NotFound(new BaseErrorResponse(404, string.Format(BaseExceptionMessages.HTTP_404_NotFound_Id, id)));
        }

        /// <summary>
        /// The endpoint for obtaining a single scraper object from APIFY.
        /// </summary>
        /// <param name="id">The id of the object to be retrieved.</param>
        /// <returns>
        /// <list type="number">
        /// <item>
        /// <description>A CommonScraperViewModel object type for the found object, if there is an entry for the given id;</description>
        /// </item>
        /// <item>
        /// <description>A NotFound object type, if no object is found, and</description>
        /// </item>
        /// <item>
        /// <description>An ErrorResponse object if the process fails.</description>
        /// </item>
        /// </list>
        /// </returns>
        [HttpGet("[action]/{id}")]
        [ProducesResponseType(typeof(CommonScraperFromApiFyViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFromApiFyAsync([FromRoute] string id)
        {
            CheckEmptyString(nameof(id), id);

            CommonScraperFromApiFyViewModel result = await _commonScraperAppService.Value.GetFromApiFyAsync(id);

            return result != null ? Ok(result) : NotFound(new BaseErrorResponse(404, string.Format(BaseExceptionMessages.HTTP_404_NotFound_Id, id)));
        }

        /// <summary>
        /// The endpoint for listing scrapers filtered by title, createdAt, and status.
        /// </summary>
        /// <param name="title">The title of the scraper (or part of it).</param>
        /// <param name="createdAtStart">The starting date for the createdAt date of the scraper.</param>
        /// <param name="status">The scrapers' status id:
        /// <list type="bullet">
        /// <item>
        /// <description>1: Active;</description>
        /// </item>
        /// <item>
        /// <description>2: Inactive.</description>
        /// </item>
        /// </list>
        /// </param>
        /// <returns>
        /// <list type="number">
        /// <item>
        /// <description>A List of CommonScraperViewModel objects type for the listed objects;</description>
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
        [ProducesResponseType(typeof(List<CommonScraperViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListAsync([FromQuery] string title, [FromQuery] DateTimeOffset? createdAtStart, [FromQuery] int? status)
        {
            if (!_listCommonScraperValidator.Value.Validate(createdAtStart, status, out ValidationErrors))
                return UnprocessableEntity(new BaseErrorResponse(ValidationErrors));

            List<CommonScraperViewModel> result = await _commonScraperAppService.Value.ListAsync(title, createdAtStart, status);

            return result?.Count > 0 ? Ok(result) : NoContent();
        }

        /// <summary>
        /// The endpoint for listing scrapers  filtered by title, name, and createdAt from APIFY.
        /// </summary>
        /// <param name="title">The title of the scraper (or part of it).</param>
        /// <param name="name">The name of the scraper (or part of it).</param>
        /// <param name="createdAtStart">The starting date for the createdAt date of the scraper.</param>
        /// <returns>
        /// <list type="number">
        /// <item>
        /// <description>A List of CommonScraperFromApiFyViewModel objects type for the listed objects;</description>
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
        [ProducesResponseType(typeof(List<CommonScraperFromApiFyViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListFromApiFyAsync([FromQuery] string title, [FromQuery] string name, [FromQuery] DateTimeOffset? createdAtStart)
        {
            if (!_listCommonScraperFromApiFyValidator.Value.Validate(createdAtStart, out ValidationErrors))
                return UnprocessableEntity(new BaseErrorResponse(ValidationErrors));

            List<CommonScraperFromApiFyViewModel> result = await _commonScraperAppService.Value.ListFromApiFyAsync(title, name, createdAtStart);

            return result?.Count > 0 ? Ok(result) : NoContent();
        }
    }
}
