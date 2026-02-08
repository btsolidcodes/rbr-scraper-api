using rBR.BaseLibraries.Application.Validator;
using rBR.Scraper.Application.Model.Admin.Request;

namespace rBR.Scraper.Application.Interface.Validator.Admin
{
    /// <summary>
    /// The class that implements the <see cref="CreateScraperRunRequestModel"/> model validations.
    /// </summary>
    public interface ICreateListScraperRunValidator : IBaseValidator<CreateScraperRunRequestModel>
    {
    }
}
