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
    /// <inheritdoc cref="IListInstagramProfileDatasetValidator"/>
    public class ListInstagramProfileDatasetValidator : IListInstagramProfileDatasetValidator
    {
        /// <summary>
        /// The <see cref="ListInstagramProfileDatasetValidator"/>'s constructor.
        /// </summary>
        public ListInstagramProfileDatasetValidator()
        {
            BaseValidationErrorMessages.Culture = CultureInfo.CurrentCulture;
            ValidationErrors.Culture = CultureInfo.CurrentCulture;
        }

        /// <inheritdoc cref="IListInstagramProfileDatasetValidator.Validate(int?, int, int, out List{ValidationFailure})"/>
        public bool Validate(int? status, int page, int pageSize, out List<ValidationFailure> validationFailures)
        {
            validationFailures = [];

            if (status.HasValue && (status.Value == 0 || status.Value > Enum.GetValues(typeof(BaseStatus.EnumStatus)).Length))
                validationFailures.Add(new ValidationFailure(nameof(status), string.Format(BaseValidationErrorMessages.InvalidStatus, nameof(status), 1, Enum.GetValues(typeof(BaseStatus.EnumStatus)).Length)));

            if (page < 1)
                validationFailures.Add(new ValidationFailure(nameof(page), string.Format(BaseValidationErrorMessages.InvalidMinimumPage, 1)));

            if (pageSize > 1000)
                validationFailures.Add(new ValidationFailure(nameof(pageSize), string.Format(BaseValidationErrorMessages.InvalidMaximumPageSize, 1000)));

            return validationFailures.Count == 0;
        }
    }
}
