using rBR.BaseLibraries.Domain.Entity;
using System;

namespace rBR.Scraper.Application.Model.Admin.View
{
    /// <summary>
    /// The class that holds the visualization properties for <see cref="CommonScraper"/> objects fom APIFY..
    /// </summary>
    public class CommonScraperFromApiFyViewModel
    {
        /// <summary>
        /// The <see cref="CommonScraperFromApiFyViewModel"/>'s constructor.
        /// </summary>
        /// <param name="dataModel">The data model <see cref="CommonScraper"/> object that is base for the view model <see cref="CommonScraperViewModel"/>'s creation.</param>
        public CommonScraperFromApiFyViewModel(CommonScraper dataModel)
        {
            DataId = dataModel.DataId;
            Title = dataModel.Title;
            Description = dataModel.Description;
            Name = dataModel.Name;
            CreatedAt = dataModel.CreatedAt;
            ModifiedAt = dataModel.ModifiedAt;
        }

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
