
using System;
using System.Collections.Generic;

namespace rBR.Scraper.Application.Model.Admin.Request
{
    /// <summary>
    /// The class that holds the required properties for creating a <see cref="List{T}"/> of <see cref="CommonScraper"/> objects.
    /// </summary>
    public class CreateCommonScraperRequestModel
    {
        /// <summary>
        /// The <see cref="List{T}"/> of <see cref="CommonScraperRequestModel"/> for creation.
        /// </summary>
        public List<CommonScraperRequestModel> Scrapers { get; set; }
    }

    /// <summary>
    /// The class that holds the required properties for creating a <see cref="CommonScraper"/> object.
    /// </summary>
    public class CommonScraperRequestModel
    {
        /// <summary>
        /// The scraper's APIFY internal id.
        /// </summary>
        public string DataId { get; set; }
        /// <summary>
        /// The actor's (version of the scraper) title.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The scraper's description.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The scraper's default name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The date and time when the actor was created in APIFY.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }
        /// <summary>
        /// The date and time when the actor was modified in APIFY.
        /// </summary>
        public DateTimeOffset? ModifiedAt { get; set; }
    }
}
