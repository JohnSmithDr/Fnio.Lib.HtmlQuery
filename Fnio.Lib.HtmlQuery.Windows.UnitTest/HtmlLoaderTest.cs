using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Fnio.Lib.HtmlQuery.Windows.UnitTest
{
    [TestClass]
    public class HtmlLoaderTest
    {
        [TestMethod]
        public async Task TestLoadHtmlAsync()
        {
            var html = await HtmlLoader.LoadHtmlAsync(new Uri("http://www.baidu.com"));
            html.Should().MatchRegex(@"<title>百度一下，你就知道<\/title>");
        }

        [TestMethod]
        public async Task TestLoadHtmlDocumentAsync()
        {
            var htmlDoc = await HtmlLoader.LoadHtmlDocumentAsync(new Uri("http://www.baidu.com"));
            htmlDoc.Title.Should().Match("百度一下，你就知道");
        }
    }
}
