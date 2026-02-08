using Newtonsoft.Json.Linq;
using rBR.BaseLibraries.Domain.Entity;
using rBR.Scraper.Application.Model.User.View;
using System;

namespace rBR.Scraper.Application.Model.Admin.View
{
    /// <summary>
    /// The class that holds the visualization properties for <see cref="InstagramScraperDataset"/> objects fom APIFY.
    /// </summary>
    public class InstagramScraperDatasetViewModel : CommonViewModel
    {
        /// <summary>
        /// The <see cref="InstagramScraperDatasetViewModel"/>'s constructor
        /// </summary>
        /// <param name="dataModel">The data model <see cref="InstagramScraperDataset"/> object that is base for the view model <see cref="InstagramScraperDatasetViewModel"/>'s creation.</param>
        public InstagramScraperDatasetViewModel(InstagramScraperDataset dataModel) : base(dataModel.Id, dataModel.Status, dataModel.Created, dataModel.Modified)
        {
            DataId = dataModel.DataId;
            Url = dataModel.Url;
            InputUrl = dataModel.InputUrl;
            Timestamp = dataModel.Timestamp;
            Data = JObject.FromObject(dataModel.FullObjectStructured);
        }

        /// <summary>
        /// The internal id of the data collected by the scraper.
        /// </summary>
        public string DataId { get; set; }
        /// <summary>
        /// The URL of the object that was collected by the scrapper.
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// The URL of the page that was monitored by the scraper.
        /// </summary>
        public string InputUrl { get; set; }
        /// <summary>
        /// The date and time when the object was posted.
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }
        /// <summary>
        /// The complete data from the dataset item.
        /// </summary>
        public JObject Data { get; set; }
    }
}
