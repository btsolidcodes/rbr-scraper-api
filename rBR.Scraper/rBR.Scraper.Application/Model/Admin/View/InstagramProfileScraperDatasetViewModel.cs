using Newtonsoft.Json.Linq;
using rBR.BaseLibraries.Domain.Entity;
using rBR.Scraper.Application.Model.User.View;
using System;

namespace rBR.Scraper.Application.Model.Admin.View
{
    /// <summary>
    /// The class that holds the visualization properties for <see cref="InstagramProfileScraperDataset"/> objects fom APIFY.
    /// </summary>
    public class InstagramProfileScraperDatasetViewModel : CommonViewModel
    {
        /// <summary>
        /// The <see cref="InstagramProfileScraperDatasetViewModel"/>'s constructor
        /// </summary>
        /// <param name="dataModel">The data model <see cref="InstagramProfileScraperDataset"/> object that is base for the view model <see cref="InstagramProfileScraperDatasetViewModel"/>'s creation.</param>
        public InstagramProfileScraperDatasetViewModel(InstagramProfileScraperDataset dataModel) : base(dataModel.Id, dataModel.Status, dataModel.Created, dataModel.Modified)
        {
            DataId = dataModel.DataId;
            InputUrl = dataModel.InputUrl;
            Url = dataModel.Url;
            UserName = dataModel.UserName;
            FullName = dataModel.FullName;
            FollowersCount = dataModel.FollowersCount;
            Verified = dataModel.Verified;
            Timestamp = dataModel.Timestamp;
            Data = JObject.FromObject(dataModel.FullObjectStructured);
        }

        /// <summary>
        /// The int?ernal id of the data collected by the scraper.
        /// </summary>
        public string DataId { get; set; }
        /// <summary>
        /// The URL of the page that was monitored by the scraper.
        /// </summary>
        public string InputUrl { get; set; }
        /// <summary>
        /// The URL of the object that was collected by the scrapper.
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// The user name of the account that owns the profile.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// The full name of the profile's account.
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// The amount of followers of the profile.
        /// </summary>
        public int? FollowersCount { get; set; }
        /// <summary>
        /// The flag indicating if the profile account is verified.
        /// </summary>
        public bool? Verified { get; set; }
        /// <summary>
        /// The date and time when the profile information was collected.
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }
        /// <summary>
        /// The complete data from the dataset item.
        /// </summary>
        public JObject Data { get; set; }
    }
}
