using AppMarketHelper.PlayGoogleCom;
using AppMarketHelper.PlayGoogleCom.Requests;
using NUnit.Framework;
using System.Threading.Tasks;

namespace AppMarketHelper.Tests
{
    [TestFixture]
    internal class AppPageParserTest : BaseTest
    {
        private readonly AppPageParser _appPageParser = new AppPageParser();


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
            var result = await _appPageParser.TryParsePageAsync(args);
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
