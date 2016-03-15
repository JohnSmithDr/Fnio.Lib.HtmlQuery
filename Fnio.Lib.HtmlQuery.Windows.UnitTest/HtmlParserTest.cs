using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System.Linq;

namespace Fnio.Lib.HtmlQuery.Windows.UnitTest
{
    [TestClass]
    public class HtmlParserTest
    {
        [TestMethod]
        public void TestParseHtml()
        {
            var htmlDoc = HtmlParser.Parse(TestData.SimpleHtmlDoc);
            htmlDoc.Root.Should().NotBeNull();
            htmlDoc.Head.Should().NotBeNull();
            htmlDoc.Body.Should().NotBeNull();
            htmlDoc.Title.Should().Be("Example");
        }

        [TestMethod]
        public void TestParseHtmlWithTagFilter()
        {
            var tagFilter = new string[] { "meta", "link", "script" };
            var htmlDoc = HtmlParser.Parse(TestData.SimpleHtmlDoc, tagFilter);
            htmlDoc.Root.Should().NotBeNull();
            htmlDoc.Head.Should().NotBeNull();
            htmlDoc.Body.Should().NotBeNull();
            htmlDoc.GetElementsByTagName("meta").Should().HaveCount(0);
            htmlDoc.GetElementsByTagName("link").Should().HaveCount(0);
            htmlDoc.GetElementsByTagName("script").Should().HaveCount(0);
        }
    }
}
