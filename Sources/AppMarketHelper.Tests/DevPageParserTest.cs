using System.Linq;
using AppMarketHelper.PlayGoogleCom;
using AppMarketHelper.PlayGoogleCom.Requests;
using NUnit.Framework;
using System.Threading.Tasks;

namespace AppMarketHelper.Tests
{
    [TestFixture]
    internal class DevPageParserTest : BaseTest
    {
        private readonly DeveloperPageParser _pageParser = new DeveloperPageParser();


        [Test]
        [TestCase("Chainart")]
        public async Task ParsePageTest(string id)
        {
            var args = new DevPageGetRequest
            {
                Query = new DevPageGetRequest.QueryArgs()
                {
                    Id = id,
                    LanguageCode = "en",
                    GovernmentCode = "us"
                }
            };
            var result = await _pageParser.TryParsePageAsync(args);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.Result.AppIds.Any());
        }
    }
}
