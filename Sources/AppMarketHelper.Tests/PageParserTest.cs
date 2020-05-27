using System.Threading.Tasks;
using AppMarketHelper.PlayGoogleCom;
using AppMarketHelper.PlayGoogleCom.Requests;
using AppMarketHelper.PlayGoogleCom.Responses;
using NUnit.Framework;

namespace AppMarketHelper.Tests
{
    [TestFixture]
    internal class PageParserTest : BaseTest
    {
        private readonly PageParser _pageParser = new PageParser();


        [Test]
        [TestCase("com.chainartsoft.gohamster")]
        public async Task ParsePageTest(string pkgId)
        {
            var args = new AppPageGetRequest
            {
                Query = new AppPageGetRequest.QueryArgs()
                {
                    Id = pkgId,
                    LanguageCode = "en",
                    GovernmentCode = "us"
                }
            };
            var result = await _pageParser.TryParsePageAsync(args);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.Result.Title.Length > 0);
            Assert.IsTrue(result.Result.DescriptionHTML.Length > 0);
            Assert.IsTrue(result.Result.HeaderImage.Length > 0);
            Assert.IsTrue(result.Result.Genre.Length > 0);
            Assert.IsTrue(result.Result.MinInstalls > 0);
            Assert.IsTrue(result.Result.Price >= 0);
            Assert.IsTrue(result.Result.Currency.Length > 0);
        }
    }
}
