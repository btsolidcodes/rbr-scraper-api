using FluentValidation.Results;
using rBR.BaseLibraries.Domain.Entity;
using System;
using System.Collections.Generic;

namespace rBR.Scraper.Application.Interface.Validator.Admin
{
    /// <summary>
    /// The class that implements the <see cref="CommonScraper"/> listing validations.
    /// </summary>
    public interface IListCommonScraperValidator
    {
        /// <summary>
        /// The method that applies the validation rules.
        /// </summary>
        /// <param name="createdAtStart">The starting date for the createdAt date of the scraper.</param>
        /// <param name="status">The scrapers' status id.</param>
        /// <param name="validationFailures">A list of validation errors to support the validation results.</param>
        /// <returns>
        /// <list type="bullet">
        /// <item>
        /// <description><see langword="True"/> if the object is valid;</description>
        /// </item>
        /// <item>
        /// <description><see langword="False"/> if the object is invalid.</description>
        /// </item>
        /// </list>
        /// </returns>
        bool Validate(DateTimeOffset? createdAtStart, int? status, out List<ValidationFailure> validationFailures);
    }
}
