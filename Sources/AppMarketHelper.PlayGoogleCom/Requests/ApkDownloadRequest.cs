using ExtendedHttpClient.Common.Attributes;

namespace AppMarketHelper.PlayGoogleCom.Requests
{
    public class ApkDownloadRequest
    {
        [Header("User-Agent")]
        public string UserAgent => "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36";

        [Header("X-DFE-Device-Id")]
        public string DeviceId { get; set; }

        [Header("X-DFE-Client-Id")]
        public string ClientId => "am-android-google";

        [Header("X-DFE-Content-Filters")]
        public string ContentFilters { get; set; }

        [Header("X-DFE-Encoded-Targets")]
        public string EncodedTargets { get; set; }

        [Header("X-DFE-Network-Type")]
        public string NetworkType { get; set; } = "4";

        [Header("X-DFE-UserLanguages")]
        public string UserLanguages { get; set; }

        [Header("X-DFE-Request-Params")]
        public string RequestParams { get; set; } = "timeoutMs=30000";

        [Header("X-DFE-Device-Checkin-Consistency-Token")]
        public string DeviceCheckinConsistencyToken { get; set; }

        [Header("X-DFE-Device-Config-Token")]
        public string DeviceConfigToken { get; set; }

        [Header("X-DFE-Cookie")]
        public string Cookie { get; set; }

        [Header("X-DFE-MCCMNC")]
        public string MCCMNC { get; set; }

        [Header("Accept-Language")]
        public string AcceptLanguage { get; set; }

        [Header("Authorization", IsAuthorization = true)]
        public string GoogleLoginAuth => string.IsNullOrEmpty(AuthToken) ? string.Empty : $"GoogleLogin auth={AuthToken}";

        public string AuthToken { get; set; }
    }
}