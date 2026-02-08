using Microsoft.AspNetCore.Mvc;
using rBR.BaseLibraries.Application.Properties;
using rBR.BaseLibraries.Service.Utils;
using rBR.Scraper.Application.Model.Admin.Request;

namespace rBR.Scraper.Service.Utils
{
    /// <summary>
    /// The application's class for extending and customizing the <see cref="ControllerBase"/>.
    /// </summary>
    public class CustomControllerBase : BaseCustomControllerBase
    {
        internal const string SupportPolicy = "rBR Support";
        internal const string AdminPolicy = "rBR Admin";
        internal const string ClientPolicy = "rBR Client";
        internal const string UserPolicy = "rBR User";

        /// <summary>
        /// The method that fixes the UTC offset of the scrapers by a number of hours.
        /// </summary>
        /// <param name="scrapers">The request model containing the scrapers.</param>
        /// <param name="hours">The amount of hours to be fixed.</param>
        internal void TreatScrapersDateTimeOffset(ref CreateCommonScraperRequestModel scrapers, int hours)
        {
            for (int i = 0; i < scrapers.Scrapers.Count; i++)
            {
                scrapers.Scrapers[i].CreatedAt = scrapers.Scrapers[i].CreatedAt.AddHours(-hours);

                if (scrapers.Scrapers[i].ModifiedAt.HasValue)
                    scrapers.Scrapers[i].ModifiedAt = scrapers.Scrapers[i].ModifiedAt.Value.AddHours(-hours);
            }
        }

        /// <summary>
        /// The method that fixes the UTC offset of the runs by a number of hours.
        /// </summary>
        /// <param name="runs">The request model containing the runs.</param>
        /// <param name="hours">The amount of hours to be fixed.</param>
        internal void TreatRunsDateTimeOffsset(ref CreateScraperRunRequestModel runs, int hours)
        {
            for (int i = 0; i < runs.Runs.Count; i++)
            {
                runs.Runs[i].StartedAt = runs.Runs[i].StartedAt.AddHours(-hours);
                runs.Runs[i].FinishedAt = runs.Runs[i].FinishedAt.AddHours(-hours);
            }
        }

        /// <summary>
        /// The method that checks if a page size is greather than the maximum allowed value.
        /// </summary>
        /// <param name="pageSize">The page size given by the user.</param>
        /// <param name="maxValue">The maximum allowed value.</param>
        internal static void CheckPageSize(int pageSize, int maxValue)
        {
            if (pageSize > maxValue)
                throw new(string.Format(BaseValidationErrorMessages.InvalidMaximumPageSize, maxValue));
        }

        /// <summary>
        /// The method that checks if the page respects the minimum value of a page.
        /// </summary>
        /// <param name="page">The given page.</param>
        /// <param name="minValue">The minimum allowed value.</param>
        internal static void CheckMininumPage(int page, int minValue)
        {
            if (page < 1)
                throw new(string.Format(BaseValidationErrorMessages.InvalidMinimumPage, minValue));
        }
    }
}
