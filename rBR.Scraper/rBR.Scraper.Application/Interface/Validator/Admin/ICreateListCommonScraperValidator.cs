using rBR.BaseLibraries.Application.Validator;
using rBR.Scraper.Application.Model.Admin.Request;

namespace rBR.Scraper.Application.Interface.Validator.Admin
{
    /// <summary>
    /// The class that implements the <see cref="CreateCommonScraperRequestModel"/> model validations.
    /// </summary>
    public interface ICreateListCommonScraperValidator : IBaseValidator<CreateCommonScraperRequestModel>
    {
    }
}
