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
    public class AppPageParser : BasePageParser
    {
        private const string Url = "https://play.google.com/store/apps/details";

        private readonly HttpClient _client;


        public AppPageParser()
            : this(new HttpClient()) { }

        public AppPageParser(HttpClient restApiClient)
        {
            _client = restApiClient;
        }


        public async Task<OperationResult<AppInfo>> TryParsePageAsync(AppPageGetRequest args, CancellationToken token = default(CancellationToken))
        {
            var response = await _client.GetAsync<AppPageGetRequest, string>(Url, args, token);

            if (response.IsSuccess)
            {
                var html = response.Result;
                var appInfo = new AppInfo
                {
                    AppId = args.Query.Id
                };

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
                            appInfo.Price = TryGetValue<long>(jArr, 0, 2, 0, 0, 0, 1, 0, 0) / 1000000;
                            appInfo.Currency = TryGetValue<string>(jArr, 0, 2, 0, 0, 0, 1, 0, 1);
                            appInfo.PriceText = TryGetValue<string>(jArr, 0, 2, 0, 0, 0, 1, 0, 2);

                            appInfo.Free = appInfo.Price == 0;

                            break;
                        }
                        case "ds:5":
                        {
                            var jArr = (JArray)data.Data;
                            appInfo.Title = TryGetValue<string>(jArr, 0, 0, 0);
                            appInfo.DescriptionHTML = TryGetValue<string>(jArr, 0, 10, 0, 1);
                            appInfo.Summary = TryGetValue<string>(jArr, 0, 10, 1, 1);
                            var screenshotsArr = TryGetValue<JArray>(jArr, 0, 12, 0);
                            appInfo.Screenshots = TryGetValues<string>(screenshotsArr, 3, 2).ToArray();
                            appInfo.Icon = TryGetValue<string>(jArr, 0, 12, 1, 3, 2);
                            appInfo.HeaderImage = TryGetValue<string>(jArr, 0, 12, 2, 3, 2);
                            appInfo.Video = TryGetValue<string>(jArr, 0, 12, 3, 0, 3, 2);
                            appInfo.VideoImage = TryGetValue<string>(jArr, 0, 12, 3, 1, 3, 2);
                            appInfo.ContentRating = TryGetValue<string>(jArr, 0, 12, 4, 0);
                            appInfo.ContentRatingDescription = TryGetValue<string>(jArr, 0, 12, 4, 2, 1);
                            appInfo.DeveloperInternalID = TryGetValue<string>(jArr, 0, 12, 5, 0, 0);
                            appInfo.Developer = TryGetValue<string>(jArr, 0, 12, 5, 1);
                            appInfo.DeveloperEmail = TryGetValue<string>(jArr, 0, 12, 5, 2, 0);
                            appInfo.DeveloperWebsite = TryGetValue<string>(jArr, 0, 12, 5, 3, 5, 2);
                            appInfo.DeveloperAddress = TryGetValue<string>(jArr, 0, 12, 5, 4, 0);
                            appInfo.DeveloperId = TryGetValue<string>(jArr, 0, 12, 5, 5, 4, 2);
                            appInfo.RecentChanges = TryGetValue<string>(jArr, 0, 12, 6, 1);
                            appInfo.PrivacyPolicy = TryGetValue<string>(jArr, 0, 12, 7, 2);
                            appInfo.Updated = TryGetValue<long>(jArr, 0, 12, 8, 0) * 1000;
                            appInfo.Installs = TryGetValue<string>(jArr, 0, 12, 9, 0);
                            appInfo.OffersIAP = TryGetValue<string>(jArr, 0, 12, 12, 0);
                            appInfo.Genre = TryGetValue<string>(jArr, 0, 12, 13, 0, 0);
                            appInfo.GenreId = TryGetValue<string>(jArr, 0, 12, 13, 0, 2);
                            appInfo.FamilyGenre = TryGetValue<string>(jArr, 0, 12, 13, 1, 0);
                            appInfo.FamilyGenreId = TryGetValue<string>(jArr, 0, 12, 13, 1, 2);
                            appInfo.AdSupported = TryGetValue<string>(jArr, 0, 12, 14, 0);
                            appInfo.Released = TryGetValue<string>(jArr, 0, 12, 36);


                            appInfo.MinInstalls = CleanInt(appInfo.Installs);
                            appInfo.Description = RemoveHtmlTags(appInfo.DescriptionHTML);
                            break;
                        }
                        case "ds:6":
                        {
                            var jArr = (JArray)data.Data;

                            appInfo.Score = TryGetValue<double>(jArr, 0, 6, 0, 1);
                            appInfo.ScoreText = TryGetValue<string>(jArr, 0, 6, 0, 0);
                            var histogram = TryGetValue<JArray>(jArr, 0, 6, 1);
                            appInfo.Histogram = new Histogram()
                            {
                                Marks = TryGetValues<long>(histogram, 1).ToArray()
                            };
                            appInfo.Ratings = TryGetValue<long>(jArr, 0, 6, 2, 1);
                            appInfo.Reviews = TryGetValue<long>(jArr, 0, 6, 3, 1);
                            break;
                        }
                        case "ds:8":
                        {
                            var jArr = (JArray)data.Data;
                            appInfo.Size = TryGetValue<string>(jArr, 0);
                            appInfo.Version = TryGetValue<string>(jArr, 1);
                            appInfo.AndroidVersionText = TryGetValue<string>(jArr, 2);

                            appInfo.AndroidVersion = NormalizeAndroidVersion(appInfo.AndroidVersionText);

                            break;
                        }
                        case "ds:19":
                        {
                            var jArr = (JArray)data.Data;
                            var comments = TryGetValue<JArray>(jArr, 0);
                            appInfo.Comments = TryGetValues<string>(comments, 4).ToArray();

                            break;
                        }
                    }
                }

                return new OperationResult<AppInfo>(appInfo);
            }

            return new OperationResult<AppInfo>(response.Exception);
        }



        private string NormalizeAndroidVersion(string appInfoAndroidVersionText)
        {
            var number = appInfoAndroidVersionText.Split(' ')[0];
            if (decimal.TryParse(number, out _))
                return number;
            return "VARY";
        }

        private long CleanInt(string text)
        {
            var num = NotNumbersRegex.Replace(text, "");
            return long.Parse(num);
        }

        private string RemoveHtmlTags(string text)
        {
            // preserve the line breaks when converting to text
            //const html = cheerio.load('<div>' + description.replace(/< br >/ g, '\r\n') + '</div>');
            //return cheerio.text(html('div'));

            return text;
        }
    }
}
