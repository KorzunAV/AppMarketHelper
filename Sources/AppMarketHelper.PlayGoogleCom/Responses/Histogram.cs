﻿using System.Linq;
using Newtonsoft.Json;

namespace AppMarketHelper.PlayGoogleCom.Responses
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Histogram
    {
        private static readonly long[] Default = new long[5];

        private long[] _marks;

        public long[] Marks
        {
            get => _marks != null && _marks.Length == 5 ? _marks : Default;
            set => _marks = value;
        }

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