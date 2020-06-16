using System.Collections.Generic;
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
        private const string Host = "https://play.google.com";
        private const string DeveloperUrl = Host + "/store/apps/developer";
        private const string DevUrl = Host + "/store/apps/dev";
        private readonly Regex _urlVersionRegex = new Regex("^[0-9]*$");

        private readonly HttpClient _client;


        public DeveloperPageParser()
            : this(new HttpClient()) { }

        public DeveloperPageParser(HttpClient restApiClient)
        {
            _client = restApiClient;
        }

        public async Task<OperationResult<DevInfo>> TryParsePageAsync(DevPageGetRequest args, CancellationToken token = default(CancellationToken))
        {
            var isv2 = _urlVersionRegex.IsMatch(args.Query.Id);

            var response = await _client.GetAsync<DevPageGetRequest, string>(isv2 ? DevUrl : DeveloperUrl, args, token).ConfigureAwait(true);

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
                    var jsonData = InitDataRegex.Match(match.Value);
                    if (!jsonData.Success)
                        continue;

                    var data = JsonConvert.DeserializeObject<InitData>(jsonData.Groups[1].Value);

                    if (data.IsError || string.IsNullOrEmpty(data.Key))
                        continue;

                    switch (data.Key)
                    {
                        case "ds:3":
                        {
                            var jArr = (JArray)data.Data;
                            var apps = TryGetValue<JArray>(jArr, 0, 1, 0, 0, 0);
                            devInfo.AppIds = TryGetValues<string>(apps, 12, 0).ToArray();

                            var more = TryGetValue<string>(jArr, 0, 1, 0, 0, 3, 4, 2);
                            if (!string.IsNullOrEmpty(more) && more.StartsWith("/store/"))
                            {
                                var appIds = await TryGetNextPageAsync(more, token).ConfigureAwait(true);
                                if (appIds.IsSuccess)
                                {
                                    appIds.Result.AddRange(devInfo.AppIds);
                                    devInfo.AppIds = appIds.Result.Distinct().ToArray();
                                }
                            }

                            break;
                        }
                    }
                }

                return new OperationResult<DevInfo>(devInfo);
            }

            return new OperationResult<DevInfo>(response.Exception);
        }



        private async Task<OperationResult<List<string>>> TryGetNextPageAsync(string url, CancellationToken token)
        {
            var args = new DevPageGetRequest();
            var response = await _client.GetAsync<DevPageGetRequest, string>(Host + url, args, token).ConfigureAwait(true);

            if (response.IsSuccess)
            {
                var appIds = new List<string>();

                var html = response.Result;
                var matches = InitDataCallbackRegex.Matches(html);

                foreach (Match match in matches)
                {
                    var jsonData = InitDataRegex.Match(match.Value);
                    if (!jsonData.Success)
                        continue;

                    var data = JsonConvert.DeserializeObject<InitData>(jsonData.Groups[1].Value);

                    if (data.IsError || string.IsNullOrEmpty(data.Key))
                        continue;

                    switch (data.Key)
                    {
                        case "ds:3":
                        {
                            var jArr = (JArray)data.Data;
                            var apps = TryGetValue<JArray>(jArr, 0, 1, 0, 0, 0);
                            appIds = TryGetValues<string>(apps, 12, 0).ToList();

                            //TODO: KOA Pagination not supported :( Please add if know how

                            break;
                        }
                    }
                }

                return new OperationResult<List<string>>(appIds);
            }

            return new OperationResult<List<string>>(response.Exception);
        }
    }
}
