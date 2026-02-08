using FluentValidation;
using rBR.BaseLibraries.Application.Properties;
using rBR.Scraper.Application.Model.Admin.Request;
using rBR.Scraper.Application.Properties;
using System;
using System.Globalization;

namespace rBR.Scraper.Application.Validator.Admin
{
    /// <summary>
    /// The class that implements the <see cref="ScraperRunRequestModel"/> model validations.
    /// </summary>
    public class ScraperRunValidator : AbstractValidator<ScraperRunRequestModel>
    {
        /// <summary>
        /// The <see cref="ScraperRunValidator"/>'s constructor containing the model's validations rules.
        /// </summary>
        public ScraperRunValidator()
        {
            ValidationErrors.Culture = CultureInfo.CurrentCulture;

            RuleFor(x => x.ScraperId).NotEqual(Guid.Empty).WithMessage(BaseValidationErrorMessages.EmptyGuid);

            RuleFor(x => x.DataId).NotEmpty().WithMessage(BaseValidationErrorMessages.NotEmpty);
            RuleFor(x => x.DataId).MaximumLength(50).WithMessage(BaseValidationErrorMessages.MaximumLength);

            RuleFor(x => x.DataStatus).NotEmpty().WithMessage(BaseValidationErrorMessages.NotEmpty);
            RuleFor(x => x.DataStatus).MaximumLength(50).WithMessage(BaseValidationErrorMessages.MaximumLength);

            RuleFor(x => x.DatasetId).NotEmpty().WithMessage(BaseValidationErrorMessages.NotEmpty);
            RuleFor(x => x.DatasetId).MaximumLength(50).WithMessage(BaseValidationErrorMessages.MaximumLength);

            RuleFor(x => x.StartedAt).LessThan(x => DateTime.Now).WithMessage(string.Format(BaseValidationErrorMessages.InvalidFutureDate, nameof(ScraperRunRequestModel.StartedAt)));
            RuleFor(x => x.StartedAt).LessThanOrEqualTo(x => x.FinishedAt).WithMessage(BaseValidationErrorMessages.StartDateGreaterThanFinishDate);

            RuleFor(x => x.FinishedAt).LessThan(x => DateTime.Now).WithMessage(string.Format(BaseValidationErrorMessages.InvalidFutureDate, nameof(ScraperRunRequestModel.FinishedAt)));
        }
    }
}
