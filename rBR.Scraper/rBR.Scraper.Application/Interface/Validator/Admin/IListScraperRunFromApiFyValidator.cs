using FluentValidation.Results;

using System;
using System.Collections.Generic;

namespace rBR.Scraper.Application.Interface.Validator.Admin
{
    /// <summary>
    /// The class that implements the <see cref="ScraperRun"/> listing validations.
    /// </summary>
    public interface IListScraperRunFromApiFyValidator
    {
        /// <summary>
        /// The method that applies the validation rules.
        /// </summary>
        /// <param name="actorId">The id of the actor that owns the runs.</param>
        /// <param name="startedAtStart">The starting date for the startedAt date of the run.</param>
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
        bool Validate(string actorId, DateTimeOffset? startedAtStart, out List<ValidationFailure> validationFailures);
    }
}
