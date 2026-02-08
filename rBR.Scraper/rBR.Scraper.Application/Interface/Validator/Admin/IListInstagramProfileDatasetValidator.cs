using FluentValidation.Results;
using rBR.BaseLibraries.Domain.Entity;
using System.Collections.Generic;

namespace rBR.Scraper.Application.Interface.Validator.Admin
{
    /// <summary>
    /// The class that implements the <see cref="InstagramProfileScraperDataset"/> listing validations.
    /// </summary>
    public interface IListInstagramProfileDatasetValidator
    {
        /// <summary>
        /// The method that applies the validation rules.
        /// </summary>
        /// <param name="status">The items' status id.</param>
        /// <param name="page">The page that will be listed.</param>
        /// <param name="pageSize">The size of the page that will be listed.</param>
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
        bool Validate(int? status, int page, int pageSize, out List<ValidationFailure> validationFailures);
    }
}
