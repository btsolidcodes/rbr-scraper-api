using FluentValidation.Results;
using rBR.BaseLibraries.Application.Properties;
using rBR.Scraper.Application.Interface.Validator.Admin;
using rBR.Scraper.Application.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace rBR.Scraper.Application.Validator.Admin
{
    /// <inheritdoc cref="IListScraperRunFromApiFyValidator"/>
    public class ListScraperRunFromApiFyValidator : IListScraperRunFromApiFyValidator
    {
        /// <summary>
        /// The <see cref="ListScraperRunFromApiFyValidator"/>'s constructor.
        /// </summary>
        public ListScraperRunFromApiFyValidator()
        {
            BaseValidationErrorMessages.Culture = CultureInfo.CurrentCulture;
            ValidationErrors.Culture = CultureInfo.CurrentCulture;
        }

        /// <inheritdoc cref="IListScraperRunFromApiFyValidator.Validate(string, DateTimeOffset?, out List{ValidationFailure})"/>
        public bool Validate(string actorId, DateTimeOffset? startedAtStart, out List<ValidationFailure> validationFailures)
        {
            validationFailures = [];

            if (string.IsNullOrWhiteSpace(actorId))
                validationFailures.Add(new ValidationFailure(nameof(actorId), string.Format(BaseValidationErrorMessages.InvalidEmptyField, nameof(actorId))));

            if (startedAtStart.HasValue && startedAtStart.Value.Date > DateTime.Now.Date)
                validationFailures.Add(new ValidationFailure(nameof(startedAtStart), string.Format(BaseValidationErrorMessages.InvalidFutureDate, nameof(startedAtStart))));

            return validationFailures.Count == 0;
        }
    }
}
