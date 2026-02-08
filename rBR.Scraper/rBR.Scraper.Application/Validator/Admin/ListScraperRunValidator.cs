using FluentValidation.Results;
using rBR.BaseLibraries.Application.Properties;
using rBR.Scraper.Application.Interface.Validator.Admin;
using rBR.Scraper.Application.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace rBR.Scraper.Application.Validator.Admin
{
    /// <inheritdoc cref="IListScraperRunValidator"/>
    public class ListScraperRunValidator : IListScraperRunValidator
    {
        /// <summary>
        /// The <see cref="ListScraperRunValidator"/>'s constructor.
        /// </summary>
        public ListScraperRunValidator()
        {
            BaseValidationErrorMessages.Culture = CultureInfo.CurrentCulture;
            ValidationErrors.Culture = CultureInfo.CurrentCulture;
        }

        /// <inheritdoc cref="IListScraperRunValidator.Validate(Guid, DateTimeOffset?, bool?, bool?, out List{ValidationFailure})"/>
        public bool Validate(Guid scraperId, DateTimeOffset? startedAtstart, bool? imported, bool? importingError, out List<ValidationFailure> validationFailures)
        {
            validationFailures = [];

            if (scraperId.Equals(Guid.Empty))
                validationFailures.Add(new ValidationFailure(nameof(scraperId), string.Format(BaseValidationErrorMessages.InvalidEmptyGuid, nameof(scraperId))));

            if (startedAtstart.HasValue && startedAtstart.Value.Date > DateTime.Now.Date)
                validationFailures.Add(new ValidationFailure(nameof(startedAtstart), string.Format(BaseValidationErrorMessages.InvalidFutureDate, nameof(startedAtstart))));

            if (imported.HasValue && imported.Value && importingError.HasValue && importingError.Value)
                validationFailures.Add(new ValidationFailure(nameof(importingError), string.Format(ValidationErrors.InvalidImportedWithError, nameof(importingError))));

            return validationFailures.Count == 0;
        }
    }
}