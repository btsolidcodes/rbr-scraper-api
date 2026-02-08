using FluentValidation;
using rBR.BaseLibraries.Application.Properties;
using rBR.BaseLibraries.Application.Validator;
using rBR.Scraper.Application.Interface.Validator.Admin;
using rBR.Scraper.Application.Model.Admin.Request;
using rBR.Scraper.Application.Properties;
using System.Globalization;

namespace rBR.Scraper.Application.Validator.Admin
{
    /// <inheritdoc cref="ICreateListCommonScraperValidator"/>
    public class CreateListCommonScraperValidator : BaseValidator<CreateCommonScraperRequestModel>, ICreateListCommonScraperValidator
    {
        /// <summary>
        /// The <see cref="CreateListCommonScraperValidator"/>'s constructor containing the model's validations rules.
        /// </summary>
        public CreateListCommonScraperValidator()
        {
            ValidationErrors.Culture = CultureInfo.CurrentCulture;

            RuleFor(x => x.Scrapers).NotEmpty().WithMessage(BaseValidationErrorMessages.NotEmpty);
            RuleForEach(x => x.Scrapers).SetValidator(new CommonScraperValidator());
        }
    }
}
