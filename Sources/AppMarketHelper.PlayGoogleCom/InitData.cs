using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AppMarketHelper.PlayGoogleCom
{
    public class InitData
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("isError")]
        public bool IsError { get; set; }

        //[JsonProperty("hash")]
        //public string Hash { get; set; }

        [JsonProperty("data")]
        public JToken Data { get; set; }
    }
}