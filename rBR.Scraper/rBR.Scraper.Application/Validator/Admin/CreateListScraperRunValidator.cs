using FluentValidation;
using rBR.BaseLibraries.Application.Properties;
using rBR.BaseLibraries.Application.Validator;
using rBR.Scraper.Application.Interface.Validator.Admin;
using rBR.Scraper.Application.Model.Admin.Request;
using rBR.Scraper.Application.Properties;
using System.Globalization;

namespace rBR.Scraper.Application.Validator.Admin
{
    /// <inheritdoc cref="ICreateListScraperRunValidator"/>
    public class CreateListScraperRunValidator : BaseValidator<CreateScraperRunRequestModel>, ICreateListScraperRunValidator
    {
        /// <summary>
        /// The <see cref="CreateListScraperRunValidator"/>'s constructor containing the model's validations rules.
        /// </summary>
        public CreateListScraperRunValidator()
        {
            ValidationErrors.Culture = CultureInfo.CurrentCulture;

            RuleFor(x => x.Runs).NotEmpty().WithMessage(BaseValidationErrorMessages.NotEmpty);
            RuleForEach(x => x.Runs).SetValidator(new ScraperRunValidator());
        }
    }
}
