using FluentValidation.Results;
using rBR.BaseLibraries.Domain.Entity;
using System;
using System.Collections.Generic;

namespace rBR.Scraper.Application.Interface.Validator.Admin
{
    /// <summary>
    /// The class that implements the <see cref="InstagramScraperDataset"/> listing validations.
    /// </summary>
    public interface IListInstagramScraperDatasetValidator
    {
        /// <summary>
        /// The method that applies the validation rules.
        /// </summary>
        /// <param name="timestampStart">The starting date for the date of the timestamp.</param>
        /// <param name="status">The dataset items status.</param>
        /// <param name="pageSize">The page size of the listing.</param>
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
        bool Validate(DateTimeOffset? timestampStart, int? status, int pageSize, out List<ValidationFailure> validationFailures);
    }
}
