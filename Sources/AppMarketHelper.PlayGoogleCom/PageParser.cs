using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AppMarketHelper.PlayGoogleCom.Requests;
using AppMarketHelper.PlayGoogleCom.Responses;
using ExtendedHttpClient;
using ExtendedHttpClient.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AppMarketHelper.PlayGoogleCom
{
    public class PageParser
    {
        private const string DetailsUrl = "https://play.google.com/store/apps/details";

        private static readonly Regex InitDataCallbackRegex = new Regex(@"AF_initDataCallback[\s\S]*?<\/script");
        private static readonly Regex InitDataCallbackKeyRegex = new Regex(@"'(ds:.*?)'");
        private static readonly Regex InitDataCallbackValueRegex = new Regex(@"return ([\s\S]*?)}}\);<\/");
        private static readonly Regex NotNumbersRegex = new Regex(@"[^\d]");

        private readonly HttpClient _client;


        public PageParser()
            : this(new HttpClient()) { }

        public PageParser(HttpClient restApiClient)
        {
            _client = restApiClient;
        }


        public async Task<OperationResult<AppInfo>> TryParsePageAsync(AppPageGetRequest args, CancellationToken token = default(CancellationToken))
        {
            var response = await _client.GetAsync<AppPageGetRequest, string>(DetailsUrl, args, token);

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
                    var dsId = InitDataCallbackKeyRegex.Match(match.Value);
                    var dsValues = InitDataCallbackValueRegex.Match(match.Value);

                    if (dsId.Success && dsValues.Success)
                    {
                        var key = dsId.Groups[1].Value;
                        var gArr = dsValues.Groups[1].Value;
                        var jToken = JsonConvert.DeserializeObject(gArr);
                        try
                        {
                            switch (key)
                            {
                                case "ds:3":
                                {
                                    var jArr = (JArray)jToken;
                                    appInfo.Price = TryGetValue<long>(jArr, 0, 2, 0, 0, 0, 1, 0, 0) / 1000000;
                                    appInfo.Currency = TryGetValue<string>(jArr, 0, 2, 0, 0, 0, 1, 0, 1);
                                    appInfo.PriceText = TryGetValue<string>(jArr, 0, 2, 0, 0, 0, 1, 0, 2);

                                    appInfo.Free = appInfo.Price == 0;

                                    break;
                                }
                                case "ds:5":
                                {
                                    var jArr = (JArray)jToken;
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
                                    var jArr = (JArray)jToken;

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
                                case "ds:7":
                                {
                                    var jArr = (JArray)jToken;

                                    break;
                                }
                                case "ds:8":
                                {
                                    var jArr = (JArray)jToken;
                                    appInfo.Size = TryGetValue<string>(jArr, 0);
                                    appInfo.Version = TryGetValue<string>(jArr, 1);
                                    appInfo.AndroidVersionText = TryGetValue<string>(jArr, 2);

                                    appInfo.AndroidVersion = NormalizeAndroidVersion(appInfo.AndroidVersionText);

                                    break;
                                }
                                case "ds:19":
                                {
                                    var jArr = (JArray)jToken;
                                    var comments = TryGetValue<JArray>(jArr, 0);
                                    appInfo.Comments = TryGetValues<string>(comments, 4).ToArray();

                                    break;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            var m = e.Message;
                        }
                    }
                }

                return new OperationResult<AppInfo>(appInfo);
            }

            return new OperationResult<AppInfo>(response.Exception);
        }


        private T TryGetValue<T>(JArray array, params int[] ids)
        {
            var ch = array;
            for (var i = 0; i < ids.Length; i++)
            {
                var id = ids[i];
                if (ch.HasValues && ch.Count > id)
                {
                    var bf = ch[id];
                    if (bf.Type == JTokenType.Array)
                    {
                        ch = (JArray)bf;
                    }
                    else if (i == ids.Length - 1)
                    {
                        return bf.Value<T>();
                    }
                    else
                    {
                        return default(T);
                    }
                }
                else
                {
                    return default(T);
                }
            }

            return ch.Value<T>();
        }

        private IEnumerable<T> TryGetValues<T>(JArray array, params int[] ids)
        {
            foreach (var item in array)
            {
                if (item.Type == JTokenType.Null)
                    continue;

                if (item.Type != JTokenType.Array)
                {
                    if (ids.Length == 0)
                    {
                        yield return item.Value<T>();
                    }
                    else
                    {
                        break;
                    }
                }

                var ch = (JArray)item;
                for (var i = 0; i < ids.Length; i++)
                {
                    var id = ids[i];
                    if (ch.HasValues && ch.Count >= id)
                    {
                        var bf = ch[id];
                        if (bf.Type == JTokenType.Array)
                        {
                            ch = (JArray)ch[id];
                        }
                        else if (i == ids.Length - 1)
                        {
                            yield return bf.Value<T>();
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
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
