using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fnio.Lib.HtmlQuery.Node;
using FluentAssertions;
using System.Linq;

namespace Fnio.Lib.HtmlQuery.Windows.UnitTest
{
    public partial class HtmlQueryTest
    {
        [TestMethod]
        public void TestDescendantById()
        {
            Doc.Body.DescendantById("logo").TagName.Should().Be("img");
            Doc.Body.DescendantById("home-link").TagName.Should().Be("a");
            Doc.Body.DescendantById("container").TagName.Should().Be("div");
        }

        [TestMethod]
        public void TestDescendantsByClassName()
        {
            var el = Doc.Body.DescendantsByClassName("header");
            el.Should().HaveCount(1);
            el.FirstOrDefault().TagName.Should().Be("h1");
            el.FirstOrDefault().Text().Should().Be("Example");

            el = Doc.Body.DescendantsByClassName("info");
            el.Should().HaveCount(2);
            el.Select(s => s.TagName).Should().Equal("p", "p");
            el.Select(s => s.Text()).Should().Equal("foo", "bar");

            Doc.Body.DescendantsByClassName("container").Should().HaveCount(4);
            Doc.Body.DescendantsByClassName("containers").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestDescendantsByTagName()
        {
            var el = Doc.Body.DescendantsByTagName("a");
            el.Should().HaveCount(5);
            el.Select(s => s.Text()).Should().Equal("Home", "Dashboard", "Settings", "Profile", "Help");

            Doc.Body.DescendantsByTagName("table").Should().HaveCount(0);
            Doc.Body.DescendantsByTagName("header").Should().HaveCount(1);
            Doc.Body.DescendantsByTagName("footer").Should().HaveCount(1);
        }

        [TestMethod]
        public void TestDescendants()
        {
            var d = Doc.GetElementById("navigation").Descendants();
            d.Should().HaveCount(10);
            d.Select(s => s.TagName).Should().Equal("a", "ul", "li", "a", "li", "a", "li", "a", "li", "a");

            Doc.Body.Descendants(s => s.TagName == "a" && s["href"] == @"/").Should().HaveCount(1);
            Doc.Body.Descendants(s => s.TagName == "a" && s["href"] != @"/").Should().HaveCount(4);
            Doc.Body.Descendants(s => s.HasClassName("container")).Should().HaveCount(4);
            Doc.Body.Descendants(s => s["class"] == "container").Should().HaveCount(2);
            Doc.Body.Descendants(s => s["class"] == "wrapper").Should().HaveCount(0);
        }
    }
}
