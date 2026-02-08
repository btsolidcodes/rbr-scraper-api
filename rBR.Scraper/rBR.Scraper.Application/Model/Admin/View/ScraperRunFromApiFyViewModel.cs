using rBR.BaseLibraries.Domain.Entity;
using System;

namespace rBR.Scraper.Application.Model.Admin.View
{
    /// <summary>
    /// The class that holds the visualization properties for <see cref="ScraperRun"/> objects fom APIFY.
    /// </summary>
    public class ScraperRunFromApiFyViewModel
    {
        /// <summary>
        /// The <see cref="ScraperRunFromApiFyViewModel"/>'s constructor.
        /// </summary>
        /// <param name="dataModel">The data model <see cref="ScraperRun"/> object that is base for the view model <see cref="ScraperRunFromApiFyViewModel"/>'s creation.</param>
        public ScraperRunFromApiFyViewModel(ScraperRun dataModel)
        {
            ScraperId = dataModel.ScraperId;
            DataId = dataModel.DataId;
            DataStatus = dataModel.DataStatus;
            DatasetId = dataModel.DatasetId;
            StartedAt = dataModel.StartedAt;
            FinishedAt = dataModel.FinishedAt;
        }

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
