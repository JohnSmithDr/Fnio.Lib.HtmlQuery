using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fnio.Lib.HtmlQuery.Node;
using FluentAssertions;
using System.Linq;

namespace Fnio.Lib.HtmlQuery.UnitTest.Query
{
    [TestClass]
    public class DescendantsTest
    {
        public HtmlDocument Doc { get; } = HtmlParser.Parse(TestData.SimpleHtmlDoc);

        [TestMethod]
        public void TestGetDescendantById()
        {
            Doc.Body.GetDescendantById("logo").TagName.Should().Be("img");
            Doc.Body.GetDescendantById("home-link").TagName.Should().Be("a");
            Doc.Body.GetDescendantById("container").TagName.Should().Be("div");
        }

        [TestMethod]
        public void TestGetDescendantsByClassName()
        {
            var el = Doc.Body.GetDescendantsByClassName("header");
            el.Should().HaveCount(1);
            el.FirstOrDefault().TagName.Should().Be("h1");
            el.FirstOrDefault().Text().Should().Be("Example");

            el = Doc.Body.GetDescendantsByClassName("info");
            el.Should().HaveCount(2);
            el.Select(s => s.TagName).Should().Equal("p", "p");
            el.Select(s => s.Text()).Should().Equal("foo", "bar");

            Doc.Body.GetDescendantsByClassName("container").Should().HaveCount(4);
            Doc.Body.GetDescendantsByClassName("containers").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestGetDescendantsByTagName()
        {
            var el = Doc.Body.GetDescendantsByTagName("a");
            el.Should().HaveCount(5);
            el.Select(s => s.Text()).Should().Equal("Home", "Dashboard", "Settings", "Profile", "Help");

            Doc.Body.GetDescendantsByTagName("table").Should().HaveCount(0);
            Doc.Body.GetDescendantsByTagName("header").Should().HaveCount(1);
            Doc.Body.GetDescendantsByTagName("footer").Should().HaveCount(1);
        }

        [TestMethod]
        public void TestGetDescendants()
        {
            Doc.Body.GetDescendants(s => s.TagName == "a" && s["href"] == @"/").Should().HaveCount(1);
            Doc.Body.GetDescendants(s => s.TagName == "a" && s["href"] != @"/").Should().HaveCount(4);
            Doc.Body.GetDescendants(s => s.HasClassName("container")).Should().HaveCount(4);
            Doc.Body.GetDescendants(s => s["class"] == "container").Should().HaveCount(2);
            Doc.Body.GetDescendants(s => s["class"] == "wrapper").Should().HaveCount(0);
        }
    }
}
