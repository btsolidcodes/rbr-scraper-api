using FluentValidation.Results;

using System;
using System.Collections.Generic;

namespace rBR.Scraper.Application.Interface.Validator.Admin
{
    /// <summary>
    /// The class that implements the <see cref="ScraperRun"/> listing validations.
    /// </summary>
    public interface IListScraperRunValidator
    {
        /// <summary>
        /// The method that applies the validation rules.
        /// </summary>
        /// <param name="scraperId">The runs scraper id.</param>
        /// <param name="startedAtstart">The starting date for the startedAt date.</param>
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
        /// <param name="imported">The flag indicating if the Scraper Run Dataset was already imported:
        /// <list type="bullet">
        /// <item>
        /// <description>True: if it was already imported;</description>
        /// </item>
        /// <item>
        /// <description>False: if it was not imported yet;</description>
        /// </item>
        /// <item>
        /// <description>Null: if it is not to be filtered by this property.</description>
        /// </item>
        /// </list>
        /// </param>
        /// <param name="importingError">The flag indicating if the Scraper Run Dataset was already imported and there was an error:
        /// <list type="bullet">
        /// <item>
        /// <description>True: if it was already imported and there was an error;</description>
        /// </item>
        /// <item>
        /// <description>False: if it was already imported and there was not an error;</description>
        /// </item>
        /// <item>
        /// <description>Null: if it is not to be filtered by this property.</description>
        /// </item>
        /// </list>
        /// </param>
        /// </returns>
        bool Validate(Guid scraperId, DateTimeOffset? startedAtstart, bool? imported, bool? importingError, out List<ValidationFailure> validationFailures);
    }
}
