using System.Collections.Generic;

namespace rBR.Scraper.Domain.Settings
{
    /// <summary>
    /// The class that brings together the properties for the Scrapers configurations.
    /// </summary>
    public class ScraperSettings
    {
        /// <summary>
        /// The list of ids of the scrapers that are active and configured in the application.
        /// </summary>
        public List<string> ActiveConfiguredScrapers { get; set; }
    }
}
