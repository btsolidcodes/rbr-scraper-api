using FluentValidation;
using rBR.BaseLibraries.Application.Properties;
using rBR.Scraper.Application.Model.Admin.Request;
using rBR.Scraper.Application.Properties;
using System;
using System.Globalization;

namespace rBR.Scraper.Application.Validator.Admin
{
    /// <summary>
    /// The class that implements the <see cref="CommonScraperRequestModel"/> model validations.
    /// </summary>
    public class CommonScraperValidator : AbstractValidator<CommonScraperRequestModel>
    {
        /// <summary>
        /// The <see cref="CommonScraperValidator"/>'s constructor containing the model's validations rules.
        /// </summary>
        public CommonScraperValidator()
        {
            ValidationErrors.Culture = CultureInfo.CurrentCulture;

            RuleFor(x => x.DataId).NotEmpty().WithMessage(BaseValidationErrorMessages.NotEmpty);
            RuleFor(x => x.DataId).MaximumLength(50).WithMessage(BaseValidationErrorMessages.MaximumLength);

            RuleFor(x => x.Title).NotEmpty().WithMessage(BaseValidationErrorMessages.NotEmpty);
            RuleFor(x => x.Title).MaximumLength(100).WithMessage(BaseValidationErrorMessages.MaximumLength);

            RuleFor(x => x.Description).NotEmpty().WithMessage(BaseValidationErrorMessages.NotEmpty);
            RuleFor(x => x.Description).MaximumLength(65535).WithMessage(BaseValidationErrorMessages.MaximumLength);

            RuleFor(x => x.Name).NotEmpty().WithMessage(BaseValidationErrorMessages.NotEmpty);
            RuleFor(x => x.Name).MaximumLength(100).WithMessage(BaseValidationErrorMessages.MaximumLength);

            RuleFor(x => x.CreatedAt).LessThan(x => DateTime.Now).WithMessage(string.Format(BaseValidationErrorMessages.InvalidFutureDate, nameof(CommonScraperRequestModel.CreatedAt)));

            RuleFor(x => x.ModifiedAt).LessThan(x => DateTime.Now).When(x => x.ModifiedAt.HasValue).WithMessage(string.Format(BaseValidationErrorMessages.InvalidFutureDate, nameof(CommonScraperRequestModel.ModifiedAt)));
        }
    }
}
