using ExtendedHttpClient.Common.Attributes;
using Newtonsoft.Json;

namespace AppMarketHelper.PlayGoogleCom.Requests
{
    public class AppPageGetRequest
    {
        [Header("User-Agent")]
        public string UserAgent => "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36";

        [Query]
        public QueryArgs Query { get; set; }

        public class QueryArgs
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("hl")]
            public string LanguageCode { get; set; }

            [JsonProperty("gl")]
            public string GovernmentCode { get; set; }
        }
    }
}