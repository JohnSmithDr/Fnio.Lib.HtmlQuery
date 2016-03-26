using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace Fnio.Lib.HtmlQuery.Universal.UnitTest
{
    [TestClass]
    public class HtmlLoaderTest
    {
        [TestMethod]
        public async Task TestLoadHtmlAsync()
        {
            using (var httpClient = new HttpClient())
            {
                var html = await HtmlLoader.LoadHtmlAsync(httpClient, new Uri("http://www.baidu.com"));
                html.Should().MatchRegex(@"<title>百度一下，你就知道<\/title>");
            }
        }

        [TestMethod]
        public async Task TestLoadHtmlDocumentAsync()
        {
            using (var httpClient = new HttpClient())
            {
                var htmlDoc = await HtmlLoader.LoadHtmlDocumentAsync(httpClient, new Uri("http://www.baidu.com"));
                htmlDoc.Title.Should().Match("百度一下，你就知道");
            }
        }

        [TestMethod]
        public async Task TestLoadHtmlDocumentWithTagFilterAsync()
        {
            using (var httpClient = new HttpClient())
            {
                var tagFilters = new string[] { "script", "link", "style" };
                var htmlDoc = await HtmlLoader.LoadHtmlDocumentAsync(httpClient, new Uri("http://www.baidu.com"), tagFilters);
                htmlDoc.Title.Should().Match("百度一下，你就知道");
                htmlDoc.GetElementsByTagName("script").Should().HaveCount(0);
                htmlDoc.GetElementsByTagName("link").Should().HaveCount(0);
                htmlDoc.GetElementsByTagName("style").Should().HaveCount(0);
            }
        }
    }
}
