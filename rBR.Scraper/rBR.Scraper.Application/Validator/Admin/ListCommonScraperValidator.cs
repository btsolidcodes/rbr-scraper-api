using FluentValidation.Results;
using rBR.BaseLibraries.Application.Properties;
using rBR.BaseLibraries.Domain.Enumerator;
using rBR.Scraper.Application.Interface.Validator.Admin;
using rBR.Scraper.Application.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace rBR.Scraper.Application.Validator.Admin
{
    /// <inheritdoc cref="IListCommonScraperValidator"/>
    public class ListCommonScraperValidator : IListCommonScraperValidator
    {
        /// <summary>
        /// The <see cref="ListCommonScraperValidator"/>'s constructor.
        /// </summary>
        public ListCommonScraperValidator()
        {
            BaseValidationErrorMessages.Culture = CultureInfo.CurrentCulture;
            ValidationErrors.Culture = CultureInfo.CurrentCulture;
        }

        /// <inheritdoc cref="IListCommonScraperValidator.Validate(DateTimeOffset?, int?, out List{ValidationFailure})"/>
        public bool Validate(DateTimeOffset? createdAtStart, int? status, out List<ValidationFailure> validationFailures)
        {
            validationFailures = [];

            if (createdAtStart.HasValue && createdAtStart.Value.Date > DateTime.Now.Date)
                validationFailures.Add(new ValidationFailure(nameof(createdAtStart), string.Format(BaseValidationErrorMessages.InvalidFutureDate, nameof(createdAtStart))));

            if (status.HasValue && (status.Value == 0 || status.Value > Enum.GetValues(typeof(BaseStatus.EnumStatus)).Length))
                validationFailures.Add(new ValidationFailure(nameof(status), string.Format(BaseValidationErrorMessages.InvalidStatus, nameof(status), 1, Enum.GetValues(typeof(BaseStatus.EnumStatus)).Length)));

            return validationFailures.Count == 0;
        }
    }
}
