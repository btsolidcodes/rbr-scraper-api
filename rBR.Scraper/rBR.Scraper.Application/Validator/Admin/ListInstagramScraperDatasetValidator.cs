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
    /// <inheritdoc cref="IListInstagramScraperDatasetValidator"/>
    public class ListInstagramScraperDatasetValidator : IListInstagramScraperDatasetValidator
    {
        /// <summary>
        /// The <see cref="ListInstagramScraperDatasetValidator"/>'s constructor.
        /// </summary>
        public ListInstagramScraperDatasetValidator()
        {
            BaseValidationErrorMessages.Culture = CultureInfo.CurrentCulture;
            ValidationErrors.Culture = CultureInfo.CurrentCulture;
        }

        /// <inheritdoc cref="IListInstagramScraperDatasetValidator.Validate(DateTimeOffset?, int?, int, out List{ValidationFailure})"/>
        public bool Validate(DateTimeOffset? timestampStart, int? status, int pageSize, out List<ValidationFailure> validationFailures)
        {
            validationFailures = [];

            if (timestampStart.HasValue && timestampStart.Value.Date > DateTime.Now.Date)
                validationFailures.Add(new ValidationFailure(nameof(timestampStart), string.Format(BaseValidationErrorMessages.InvalidFutureDate, nameof(timestampStart))));

            if (status.HasValue && (status.Value == 0 || status.Value > Enum.GetValues(typeof(BaseStatus.EnumStatus)).Length))
                validationFailures.Add(new ValidationFailure(nameof(status), string.Format(BaseValidationErrorMessages.InvalidStatus, nameof(status), 1, Enum.GetValues(typeof(BaseStatus.EnumStatus)).Length)));

            if (pageSize > 1000)
                validationFailures.Add(new ValidationFailure(nameof(pageSize), string.Format(BaseValidationErrorMessages.InvalidMaximumPageSize, 1000)));

            return validationFailures.Count == 0;
        }
    }
}
