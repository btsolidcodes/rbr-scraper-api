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
    [OpenApiTag("Admin - Runs")]
    [ApiVersion("1.0", Deprecated = false)]
    [Route("API/v{version:apiVersion}/[area]/Run")]
    [ApiController]
    public class ScraperRunController : CustomControllerBase
    {
        private readonly Lazy<ICreateListScraperRunValidator> _createListScraperRunValidator;
        private readonly Lazy<IListScraperRunFromApiFyValidator> _listScraperRunFromApiFyValidator;
        private readonly Lazy<IListScraperRunValidator> _listScraperRunValidator;
        private readonly Lazy<IScraperRunAppService> _scraperRunAppService;

        /// <summary>
        /// The <see cref="ScraperRunController"/>'s constructor.
        /// </summary>
        /// <param name="createListScraperRunValidator">The interface surface of the <see cref="ICreateListScraperRunValidator"/> registered service for distributed access across other services/classes.</param>
        /// <param name="listScraperRunFromApiFyValidator">The interface surface of the <see cref="IListScraperRunFromApiFyValidator"/> registered service for distributed access across other services/classes.</param>
        /// <param name="listScraperRunValidator">The interface surface of the <see cref="IListScraperRunValidator"/> registered service for distributed access across other services/classes.</param>
        /// <param name="scraperRunAppService">The interface surface of the <see cref="IScraperRunAppService"/> registered service for distributed access across other services/classes.</param>
        public ScraperRunController(Lazy<ICreateListScraperRunValidator> createListScraperRunValidator,
            Lazy<IListScraperRunFromApiFyValidator> listScraperRunFromApiFyValidator,
            Lazy<IListScraperRunValidator> listScraperRunValidator,
            Lazy<IScraperRunAppService> scraperRunAppService)
        {
            _createListScraperRunValidator = createListScraperRunValidator ?? throw new ArgumentNullException(nameof(createListScraperRunValidator));
            _listScraperRunFromApiFyValidator = listScraperRunFromApiFyValidator ?? throw new ArgumentNullException(nameof(listScraperRunFromApiFyValidator));
            _listScraperRunValidator = listScraperRunValidator ?? throw new ArgumentNullException(nameof(listScraperRunValidator));
            _scraperRunAppService = scraperRunAppService ?? throw new ArgumentNullException(nameof(scraperRunAppService));
        }

        /// <summary>
        /// The endpoint for creating a list of new scraper runs objects.
        /// </summary>
        /// <param name="requestModel">The model containing the information for the creation process.</param>
        /// <returns>
        /// <list type="number">
        /// <item>
        /// <description>A List of ScraperRunViewModel objects if the process is successful, and</description>
        /// </item>
        /// <item>
        /// <description>A BaseErrorResponse object if the process fails.</description>
        /// </item>
        /// </list>
        /// </returns>
        [HttpPost]
        [ProducesResponseType(typeof(List<ScraperRunViewModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateScraperRunRequestModel requestModel)
        {
            TreatRunsDateTimeOffsset(ref requestModel, 3);

            if (!_createListScraperRunValidator.Value.Validate(requestModel, out ValidationErrors))
                return UnprocessableEntity(new BaseErrorResponse(ValidationErrors));

            List<ScraperRunViewModel> result = await _scraperRunAppService.Value.CreateAsync(requestModel);

            return result != null ? Created($"{RequestRoute()}/{result.FirstOrDefault().Id}", result) : StatusCode(500, new BaseErrorResponse(500, BaseExceptionMessages.HTTP_500));
        }

        /// <summary>
        /// The endpoint for obtaining a single run object.
        /// </summary>
        /// <param name="id">The id of the object to be retrieved.</param>
        /// <returns>
        /// <list type="number">
        /// <item>
        /// <description>A ScraperRunViewModel object type for the found object, if there is an entry for the given id;</description>
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
        [ProducesResponseType(typeof(ScraperRunViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            CheckEmptyGuid(nameof(id), id);

            ScraperRunViewModel result = await _scraperRunAppService.Value.GetAsync(id);

            return result != null ? Ok(result) : NotFound(new BaseErrorResponse(404, string.Format(BaseExceptionMessages.HTTP_404_NotFound_Id, id)));
        }

        /// <summary>
        /// The endpoint for listing runs filtered by scraper and startedat date.
        /// </summary>
        /// <param name="scraperId">The runs scraper id.</param>
        /// <param name="dataStatus">The status of the runs.</param>
        /// <param name="startedAtstart">The starting date for the startedAt date.</param>
        /// <param name="imported">The flag indicating if the Scraper Run Dataset was already imported:
        /// <list type="bullet">
        /// <item>
        /// <description>True: if it was already imported;</description>
        /// </item>
        /// <item>
        /// <description>False: if it was not imported yet;</description>
        /// </item>
        /// <item>
        /// <description>Null: if it is not to be filtered by this property.</description>
        /// </item>
        /// </list>
        /// </param>
        /// <param name="importingError">The flag indicating if the Scraper Run Dataset was already imported and there was an error:
        /// <list type="bullet">
        /// <item>
        /// <description>True: if it was already imported and there was an error;</description>
        /// </item>
        /// <item>
        /// <description>False: if it was already imported and there was not an error;</description>
        /// </item>
        /// <item>
        /// <description>Null: if it is not to be filtered by this property.</description>
        /// </item>
        /// </list>
        /// </param>
        /// <returns>
        /// <list type="number">
        /// <item>
        /// <description>A List of ScraperRunViewModel objects type for the listed objects;</description>
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
        [ProducesResponseType(typeof(List<ScraperRunViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListAsync([FromQuery] Guid scraperId, [FromQuery] string dataStatus, [FromQuery] DateTimeOffset? startedAtstart, [FromQuery] bool? imported, [FromQuery] bool? importingError)
        {
            if (!_listScraperRunValidator.Value.Validate(scraperId, startedAtstart, imported, importingError, out ValidationErrors))
                return UnprocessableEntity(new BaseErrorResponse(ValidationErrors));

            List<ScraperRunViewModel> result = await _scraperRunAppService.Value.ListAsync(scraperId, dataStatus, startedAtstart, imported, importingError);

            return result?.Count > 0 ? Ok(result) : NoContent();
        }

        /// <summary>
        /// The endpoint for listing scrapers' runs filtered by actor, status, and startedAt from APIFY.
        /// </summary>
        /// <param name="actorId">The id of the actor that owns the runs.</param>
        /// <param name="dataStatus">The status of the runs.</param>
        /// <param name="startedAtstart">The starting date for the startedAt date of the run.</param>
        /// <returns>
        /// <list type="number">
        /// <item>
        /// <description>A List of ScraperRunFromApiFyViewModel objects type for the listed objects;</description>
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
        [ProducesResponseType(typeof(List<ScraperRunFromApiFyViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListFromApiFyAsync([FromQuery] string actorId, [FromQuery] string dataStatus, [FromQuery] DateTimeOffset? startedAtstart)
        {
            if (!_listScraperRunFromApiFyValidator.Value.Validate(actorId, startedAtstart, out ValidationErrors))
                return UnprocessableEntity(new BaseErrorResponse(ValidationErrors));

            List<ScraperRunFromApiFyViewModel> result = await _scraperRunAppService.Value.ListFromApiFyAsync(actorId, dataStatus, startedAtstart);

            return result?.Count > 0 ? Ok(result) : NoContent();
        }
    }
}
