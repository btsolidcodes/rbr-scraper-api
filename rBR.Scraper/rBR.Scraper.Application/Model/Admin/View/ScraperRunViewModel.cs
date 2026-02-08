using rBR.BaseLibraries.Domain.Entity;
using rBR.Scraper.Application.Model.User.View;
using System;
using System.Collections.Generic;

namespace rBR.Scraper.Application.Model.Admin.View
{
    /// <summary>
    /// The class that holds the visualization properties for <see cref="ScraperRun"/> objects.
    /// </summary>
    public class ScraperRunViewModel : CommonViewModel
    {
        /// <summary>
        /// The <see cref="ScraperRunViewModel"/>'s constructor.
        /// </summary>
        /// <param name="dataModel">The data model <see cref="ScraperRun"/> object that is base for the view model <see cref="ScraperRunViewModel"/>'s creation.</param>
        public ScraperRunViewModel(ScraperRun dataModel) : base(dataModel.Id, dataModel.Status, dataModel.Created, dataModel.Modified)
        {
            DataId = dataModel.DataId;
            DataStatus = dataModel.DataStatus;
            DatasetId = dataModel.DatasetId;
            StartedAt = dataModel.StartedAt;
            FinishedAt = dataModel.FinishedAt;
            Imported = dataModel.Imported;
            ImportingError = !string.IsNullOrWhiteSpace(dataModel.ImportingError) ? (dataModel.ImportingError.Contains("|") ? [.. dataModel.ImportingError.Split("|")] : new List<string> { dataModel.ImportingError }) : null;
            Scraper = new CommonScraperViewModel(dataModel.Scraper);
        }

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
        /// <summary>
        /// The flag that indicates if a Run was imported.
        /// </summary>
        public bool Imported { get; set; }
        /// <summary>
        /// The description of a possible error while importing a Run.
        /// </summary>
        public List<string> ImportingError { get; set; }

        /// <summary>
        /// The navigation property of the Scraper that owns this Run.
        /// </summary>
        public CommonScraperViewModel Scraper { get; set; }
    }
}
