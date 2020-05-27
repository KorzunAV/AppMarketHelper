using AppMarketHelper.PlayGoogleCom.Requests;
using AppMarketHelper.PlayGoogleCom.Responses;
using ExtendedHttpClient;
using ExtendedHttpClient.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AppMarketHelper.PlayGoogleCom
{
    public class DeveloperPageParser : BasePageParser
    {
        private const string Url = "https://play.google.com/store/apps/developer";

        private readonly HttpClient _client;


        public DeveloperPageParser()
            : this(new HttpClient()) { }

        public DeveloperPageParser(HttpClient restApiClient)
        {
            _client = restApiClient;
        }

        public async Task<OperationResult<DevInfo>> TryParsePageAsync(DevPageGetRequest args, CancellationToken token = default(CancellationToken))
        {
            var response = await _client.GetAsync<DevPageGetRequest, string>(Url, args, token);

            if (response.IsSuccess)
            {
                var devInfo = new DevInfo
                {
                    DevId = args.Query.Id
                };

                var html = response.Result;
                var matches = InitDataCallbackRegex.Matches(html);

                foreach (Match match in matches)
                {
                    var dsId = InitDataCallbackKeyRegex.Match(match.Value);
                    var dsValues = InitDataCallbackValueRegex.Match(match.Value);

                    if (dsId.Success && dsValues.Success)
                    {
                        var key = dsId.Groups[1].Value;
                        var gArr = dsValues.Groups[1].Value;
                        var jToken = JsonConvert.DeserializeObject(gArr);

                        switch (key)
                        {
                            case "ds:3":
                            {
                                var jArr = (JArray)jToken;
                                var comments = TryGetValue<JArray>(jArr, 0, 1, 0, 0, 0);
                                devInfo.AppIds = TryGetValues<string>(comments, 12, 0).ToArray();
                                break;
                            }
                        }
                    }
                }

                return new OperationResult<DevInfo>(devInfo);
            }

            return new OperationResult<DevInfo>(response.Exception);
        }
    }
}
