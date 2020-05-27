using System.Linq;
using Newtonsoft.Json;

namespace AppMarketHelper.PlayGoogleCom.Responses
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Histogram
    {
        public long[] Marks { get; set; } = new long[5];

        [JsonProperty("total")]
        public long Total => Marks.Sum();

        [JsonProperty("1")]
        public long One => Marks[0];

        [JsonProperty("2")]
        public long Two => Marks[1];

        [JsonProperty("3")]
        public long Three => Marks[2];

        [JsonProperty("4")]
        public long Four => Marks[3];

        [JsonProperty("5")]
        public long Five => Marks[4];
    }
}