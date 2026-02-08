using Newtonsoft.Json.Linq;
using rBR.BaseLibraries.Data.External.Client;
using rBR.BaseLibraries.Domain.Entity;
using rBR.BaseLibraries.Domain.Enumerator;
using rBR.Scraper.Data.External.Settings;
using rBR.Scraper.Domain.Interface.ExternalService;
using rBR.Scraper.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace rBR.Scraper.Data.External.Service
{
    /// <inheritdoc cref="IApiFyExternalService"/>
    public class ApiFyExternalService : IApiFyExternalService
    {
        private readonly Lazy<ApiFySettings> _apiFySettings;
        private readonly Lazy<BaseClient> _baseClient;

        private IDictionary<string, string> Headers { get; set; }

        /// <summary>
        /// The <see cref="ApiFyExternalService"/>'s constructor.
        /// </summary>
        /// <param name="apiFySettings">The surface of the <see cref="ApiFySettings"/> registered service for distributed access across other services/classes.</param>
        /// <param name="baseClient">The surface of the <see cref="BaseClient"/> registered service for distributed access across other services/classes.</param>
        public ApiFyExternalService(Lazy<ApiFySettings> apiFySettings, Lazy<BaseClient> baseClient)
        {
            _apiFySettings = apiFySettings ?? throw new ArgumentNullException(nameof(apiFySettings));
            _baseClient = baseClient ?? throw new ArgumentNullException(nameof(baseClient));

            Headers = new Dictionary<string, string>() { { "Authorization", $"Bearer {_apiFySettings.Value.AccessToken}" } };
        }

        /// <inheritdoc cref="IApiFyExternalService.GetActorAsync(string)"/>
        public async Task<CommonScraper> GetActorAsync(string id)
        {
            try
            {
                Uri uri = new($"{_apiFySettings.Value.UrlBase}{string.Format(_apiFySettings.Value.EndpointGetActor, id)}");

                HttpResponseMessage httpResponseMessage = await _baseClient.Value.GetAsync(uri, Headers);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string message = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (message.Contains("errorDescription"))
                    {
                        StringBuilder sbError = new();
                        JArray jArrayError = JArray.Parse(message);
                        JObject jError = JObject.Parse(jArrayError[0].ToString());

                        if (jError.SelectToken("errorDescription") != null)
                            sbError.Append($"{jError.SelectToken("errorDescription").ToString()}.");

                        if (jError.SelectToken("url") != null)
                            sbError.Append($"|{string.Format("[{0}] - URL [{1}]", id, jError.SelectToken("url").ToString())}");
                        else
                            sbError.Append($"|{string.Format("[{0}] - URL [N/A]", id)}");
                        throw new Exception(sbError.ToString());
                    }

                    JObject jResult = JObject.Parse(message);
                    jResult = JObject.Parse(jResult.SelectToken("data").ToString());
                    return new()
                    {
                        DataId = jResult.SelectToken("id")?.ToString(),
                        CreatedAt = Convert.ToDateTime(jResult.SelectToken("createdAt")?.ToString()),
                        Description = jResult.SelectToken("description")?.ToString(),
                        ModifiedAt = Convert.ToDateTime(jResult.SelectToken("modifiedAt")?.ToString()),
                        Name = jResult.SelectToken("name")?.ToString(),
                        Title = jResult.SelectToken("title")?.ToString()
                    };
                }
                throw new Exception(string.Format(BusinessErrorMessages.ApiFyFailedRequest, httpResponseMessage.StatusCode));
            }
            catch (Exception ex)
            {
                if (!ex.Message.StartsWith(BusinessErrorMessages.ApiFyFailedRequest))
                    throw new Exception(string.Format(BusinessErrorMessages.ApiFyFailedRequest, ex.Message));
                throw;
            }
        }

        /// <inheritdoc cref="IApiFyExternalService.ListActorsAsync"/>
        public async Task<List<CommonScraper>> ListActorsAsync()
        {
            try
            {
                Uri uri = new($"{_apiFySettings.Value.UrlBase}{_apiFySettings.Value.EndpointListActors}");

                HttpResponseMessage httpResponseMessage = await _baseClient.Value.GetAsync(uri, Headers);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string message = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (message.Contains("errorDescription"))
                    {
                        StringBuilder sbError = new();
                        JArray jArrayError = JArray.Parse(message);
                        JObject jError = JObject.Parse(jArrayError[0].ToString());

                        if (jError.SelectToken("errorDescription") != null)
                            sbError.Append($"{jError.SelectToken("errorDescription").ToString()}.");

                        if (jError.SelectToken("url") != null)
                            sbError.Append($"|{string.Format("URL [{0}]", jError.SelectToken("url").ToString())}");
                        else
                            sbError.Append("URL [N/A]");
                        throw new Exception(sbError.ToString());
                    }

                    JObject jResult = JObject.Parse(message);
                    JArray jItems = jResult.SelectToken("data.items") as JArray;
                    List<CommonScraper> result = [];
                    foreach (JObject item in jItems)
                    {
                        result.Add(new()
                        {
                            DataId = item.SelectToken("id").ToString(),
                            Title = item.SelectToken("title").ToString(),
                            Name = item.SelectToken("name").ToString(),
                            Description = "Empty when listing from APIFY.",
                            Status = (int)BaseStatus.EnumStatus.Active,
                            CreatedAt = Convert.ToDateTime(item.SelectToken("createdAt").ToString()),
                            ModifiedAt = Convert.ToDateTime(item.SelectToken("modifiedAt").ToString()),
                        });
                    }
                    return result;
                }
                throw new Exception(string.Format(BusinessErrorMessages.ApiFyFailedRequest, httpResponseMessage.StatusCode));
            }
            catch (Exception ex)
            {
                if (!ex.Message.StartsWith(BusinessErrorMessages.ApiFyFailedRequest))
                    throw new Exception(string.Format(BusinessErrorMessages.ApiFyFailedRequest, ex.Message));
                throw;
            }
        }

        /// <inheritdoc cref="IApiFyExternalService.ListRunsAsync(string, string)"/>
        public async Task<List<ScraperRun>> ListRunsAsync(string actorId, string dataStatus)
        {
            try
            {
                StringBuilder sbUrl = new($"{_apiFySettings.Value.UrlBase}{string.Format(_apiFySettings.Value.EndpointListRuns, actorId)}");
                if (!string.IsNullOrWhiteSpace(dataStatus))
                    sbUrl.Append($"?status={dataStatus}");
                Uri uri = new(sbUrl.ToString());

                HttpResponseMessage httpResponseMessage = await _baseClient.Value.GetAsync(uri, Headers);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string message = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (message.Contains("errorDescription"))
                    {
                        StringBuilder sbError = new();
                        JArray jArrayError = JArray.Parse(message);
                        JObject jError = JObject.Parse(jArrayError[0].ToString());

                        if (jError.SelectToken("errorDescription") != null)
                            sbError.Append($"{jError.SelectToken("errorDescription").ToString()}.");

                        if (jError.SelectToken("url") != null)
                            sbError.Append($"|{string.Format("[{0}] - URL [{1}]", actorId, jError.SelectToken("url").ToString())}");
                        else
                            sbError.Append($"|{string.Format("[{0}] - URL [N/A]", actorId)}");
                        throw new Exception(sbError.ToString());
                    }

                    JObject jResult = JObject.Parse(message);
                    JArray jItems = jResult.SelectToken("data.items") as JArray;
                    List<ScraperRun> result = [];
                    foreach (JObject item in jItems)
                    {
                        result.Add(new ScraperRun()
                        {
                            DataId = item.SelectToken("id").ToString(),
                            DatasetId = item.SelectToken("defaultDatasetId").ToString(),
                            DataStatus = item.SelectToken("status").ToString(),
                            FinishedAt = Convert.ToDateTime(item.SelectToken("finishedAt").ToString()),
                            StartedAt = Convert.ToDateTime(item.SelectToken("startedAt").ToString())
                        });
                    }
                    return result;
                }
                throw new Exception(string.Format(BusinessErrorMessages.ApiFyFailedRequest, httpResponseMessage.StatusCode));
            }
            catch (Exception ex)
            {
                if (!ex.Message.StartsWith(BusinessErrorMessages.ApiFyFailedRequest))
                    throw new Exception(string.Format(BusinessErrorMessages.ApiFyFailedRequest, ex.Message));
                throw;
            }
        }

        /// <inheritdoc cref="IApiFyExternalService.GetInstagramScraperDatasetItemsAsync(string, ScraperRun, List{InstagramProfileScraperDataset})"/>
        public async Task<List<InstagramScraperDataset>> GetInstagramScraperDatasetItemsAsync(string datasetId, ScraperRun scraperRun, List<InstagramProfileScraperDataset> datasetParents)
        {
            try
            {
                Uri uri = new($"{_apiFySettings.Value.UrlBase}{string.Format(_apiFySettings.Value.EndpointGetDatasetItems, datasetId)}");

                HttpResponseMessage httpResponseMessage = await _baseClient.Value.GetAsync(uri, Headers);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string message = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (message.Contains("errorDescription"))
                    {
                        StringBuilder sbError = new();
                        JArray jArrayError = JArray.Parse(message);
                        JObject jError = JObject.Parse(jArrayError[0].ToString());

                        if (jError.SelectToken("errorDescription") != null)
                            sbError.Append($"{jError.SelectToken("errorDescription").ToString()}.");

                        if (jError.SelectToken("url") != null)
                            sbError.Append($"|{string.Format("[{0}] - URL [{1}]", datasetId, jError.SelectToken("url").ToString())}");
                        else
                            sbError.Append($"|{string.Format("[{0}] - URL [N/A]", datasetId)}");
                        throw new Exception(sbError.ToString());
                    }

                    JArray jResult = JArray.Parse(message);
                    List<InstagramScraperDataset> result = [];
                    foreach (JObject item in jResult)
                    {
                        InstagramProfileScraperDataset parent = datasetParents.Where(x => x.InputUrl.ToLower().Equals(item.SelectToken("inputUrl").ToString().ToLower())).OrderByDescending(x => x.Created).FirstOrDefault();
                        if (parent == null)
                            continue;

                        result.Add(new()
                        {
                            DataId = item.SelectToken("id").ToString(),
                            InputUrl = item.SelectToken("inputUrl").ToString(),
                            Timestamp = Convert.ToDateTime(item.SelectToken("timestamp").ToString()),
                            Url = item.SelectToken("url").ToString(),
                            FullObjectData = item.ToString(),
                            Created = DateTime.Now,
                            Status = (int)BaseStatus.EnumStatus.Active,
                            Run = scraperRun,
                            RunId = Guid.Empty,
                            InstagramProfile = parent,
                            InstagramProfileId = Guid.Empty
                        });
                    }

                    if (result.Count == 0)
                        throw new Exception(BusinessErrorMessages.MissingInstagramProfileForInstagram);

                    return result;
                }
                throw new Exception(string.Format(BusinessErrorMessages.ApiFyFailedRequest, httpResponseMessage.StatusCode));
            }
            catch (Exception ex)
            {
                if (!ex.Message.StartsWith(BusinessErrorMessages.ApiFyFailedRequest))
                    throw new Exception(string.Format(BusinessErrorMessages.ApiFyFailedRequest, ex.Message));
                throw;
            }
        }

        /// <inheritdoc cref="IApiFyExternalService.GetInstagramProfileScraperDatasetItemsAsync(string, ScraperRun )"/>
        public async Task<List<InstagramProfileScraperDataset>> GetInstagramProfileScraperDatasetItemsAsync(string datasetId, ScraperRun scraperRun)
        {
            try
            {
                Uri uri = new($"{_apiFySettings.Value.UrlBase}{string.Format(_apiFySettings.Value.EndpointGetDatasetItems, datasetId)}");

                HttpResponseMessage httpResponseMessage = await _baseClient.Value.GetAsync(uri, Headers);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string message = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (message.Contains("errorDescription"))
                    {
                        StringBuilder sbError = new();
                        JArray jArrayError = JArray.Parse(message);
                        JObject jError = JObject.Parse(jArrayError[0].ToString());

                        if (jError.SelectToken("errorDescription") != null)
                            sbError.Append($"{jError.SelectToken("errorDescription").ToString()}.");

                        if (jError.SelectToken("url") != null)
                            sbError.Append($"|{string.Format("[{0}] - URL [{1}]", datasetId, jError.SelectToken("url").ToString())}");
                        else
                            sbError.Append($"|{string.Format("[{0}] - URL [N/A]", datasetId)}");
                        throw new Exception(sbError.ToString());
                    }

                    JArray jResult = JArray.Parse(message);
                    List<InstagramProfileScraperDataset> result = [];
                    foreach (JObject item in jResult)
                    {
                        result.Add(new()
                        {
                            DataId = item.SelectToken("id").ToString(),
                            FollowersCount = string.IsNullOrWhiteSpace(item.SelectToken("followersCount")?.ToString()) ? 0 : Convert.ToInt32(item.SelectToken("followersCount").ToString()),
                            FullName = item.SelectToken("fullName").ToString(),
                            InputUrl = item.SelectToken("inputUrl").ToString(),
                            Url = item.SelectToken("url").ToString(),
                            UserName = item.SelectToken("username").ToString(),
                            Verified = string.IsNullOrWhiteSpace(item.SelectToken("verified").ToString()) ? null : Convert.ToBoolean(item.SelectToken("verified").ToString()),
                            FullObjectData = item.ToString(),
                            Created = DateTimeOffset.Now,
                            Status = (int)BaseStatus.EnumStatus.Active,
                            Run = scraperRun,
                            RunId = Guid.Empty,
                            Timestamp = scraperRun.FinishedAt
                        });
                    }
                    scraperRun.Imported = true;
                    return result;
                }
                throw new Exception(string.Format(BusinessErrorMessages.ApiFyFailedRequest, httpResponseMessage.StatusCode));
            }
            catch (Exception ex)
            {
                if (!ex.Message.StartsWith(BusinessErrorMessages.ApiFyFailedRequest))
                    throw new Exception(string.Format(BusinessErrorMessages.ApiFyFailedRequest, ex.Message));
                throw;
            }
        }
    }
}
