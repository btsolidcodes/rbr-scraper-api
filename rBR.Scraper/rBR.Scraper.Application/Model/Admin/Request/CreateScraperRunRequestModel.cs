
using System;
using System.Collections.Generic;

namespace rBR.Scraper.Application.Model.Admin.Request
{
    /// <summary>
    /// The class that holds the required properties for creating a <see cref="List{T}"/> of <see cref="ScraperRun"/> objects.
    /// </summary>
    public class CreateScraperRunRequestModel
    {
        /// <summary>
        /// The <see cref="List{T}"/> of <see cref="ScraperRunRequestModel"/> for creation.
        /// </summary>
        public List<ScraperRunRequestModel> Runs { get; set; }
    }

    /// <summary>
    /// The class that holds the required properties for creating a <see cref="ScraperRun"/> object.
    /// </summary>
    public class ScraperRunRequestModel
    {
        /// <summary>
        /// The id of the Scraper that owns this Run.
        /// </summary>
        public Guid ScraperId { get; set; }
        /// <summary>
        /// The run's APIFY internal id.
        /// </summary>
        public string DataId { get; set; }
        /// <summary>
        /// The run's APIFY internal status.
        /// </summary>
        public string DataStatus { get; set; }
        /// <summary>
        /// The id of the dataset to where the run data was saved.
        /// </summary>
        public string DatasetId { get; set; }
        /// <summary>
        /// The date and time when the run started.
        /// </summary>
        public DateTimeOffset StartedAt { get; set; }
        /// <summary>
        /// The date and time when the run finished.
        /// </summary>
        public DateTimeOffset FinishedAt { get; set; }
    }
}
