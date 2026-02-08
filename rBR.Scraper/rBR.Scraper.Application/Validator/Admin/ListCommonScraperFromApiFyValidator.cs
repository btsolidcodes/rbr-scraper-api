using FluentValidation.Results;
using rBR.BaseLibraries.Application.Properties;
using rBR.Scraper.Application.Interface.Validator.Admin;
using rBR.Scraper.Application.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace rBR.Scraper.Application.Validator.Admin
{
    /// <inheritdoc cref="IListCommonScraperFromApiFyValidator"/>
    public class ListCommonScraperFromApiFyValidator : IListCommonScraperFromApiFyValidator
    {
        /// <summary>
        /// The <see cref="ListCommonScraperFromApiFyValidator"/>'s constructor.
        /// </summary>
        public ListCommonScraperFromApiFyValidator()
        {
            BaseValidationErrorMessages.Culture = CultureInfo.CurrentCulture;
            ValidationErrors.Culture = CultureInfo.CurrentCulture;
        }

        /// <inheritdoc cref="IListCommonScraperFromApiFyValidator.Validate(DateTimeOffset?, out List{ValidationFailure})"/>
        public bool Validate(DateTimeOffset? createdAtStart, out List<ValidationFailure> validationFailures)
        {
            validationFailures = [];

            if (createdAtStart.HasValue && createdAtStart.Value.Date > DateTime.Now.Date)
                validationFailures.Add(new ValidationFailure(nameof(createdAtStart), string.Format(BaseValidationErrorMessages.InvalidFutureDate, nameof(createdAtStart))));

            return validationFailures.Count == 0;
        }
    }
}
