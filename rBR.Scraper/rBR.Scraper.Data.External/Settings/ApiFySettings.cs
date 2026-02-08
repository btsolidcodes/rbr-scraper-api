namespace rBR.Scraper.Data.External.Settings
{
    /// <summary>
    /// The class that brings together the properties for the API FY APIs,
    /// </summary>
    public class ApiFySettings
    {
        /// <summary>
        /// The APIFY API access token.
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// The APIFY API Base URL.
        /// </summary>
        public string UrlBase { get; set; }
        /// <summary>
        /// The APIFY endpoint for getting an actor.
        /// </summary>
        public string EndpointGetActor { get; set; }
        /// <summary>
        /// The APIFY endpoint for listing actors.
        /// </summary>
        public string EndpointListActors { get; set; }
        /// <summary>
        /// The APIFY endpoint for listing runs.
        /// </summary>
        public string EndpointListRuns { get; set; }
        /// <summary>
        /// The APIFY endpoint for getting a dataset.
        /// </summary>
        public string EndpointGetDatasetItems { get; set; }
    }
}
