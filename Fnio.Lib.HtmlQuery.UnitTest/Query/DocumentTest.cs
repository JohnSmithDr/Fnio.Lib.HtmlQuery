using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fnio.Lib.HtmlQuery.Node;
using FluentAssertions;
using System.Linq;

namespace Fnio.Lib.HtmlQuery.UnitTest
{
    public partial class HtmlQueryTest
    {
        [TestMethod]
        public void TestGetElementById()
        {
            Doc.GetElementById("logo").TagName.Should().Be("img");
            Doc.GetElementById("home-link").TagName.Should().Be("a");
            Doc.GetElementById("navigation").TagName.Should().Be("nav");
            Doc.GetElementById("container").TagName.Should().Be("div");
        }

        [TestMethod]
        public void TestGetElementsByClassName()
        {
            var el = Doc.GetElementsByClassName("header");
            el.Should().HaveCount(1);
            el.FirstOrDefault().TagName.Should().Be("h1");
            el.FirstOrDefault().Text().Should().Be("Example");

            el = Doc.GetElementsByClassName("info");
            el.Should().HaveCount(2);
            el.Select(s => s.TagName).Should().Equal("p", "p");
            el.Select(s => s.Text()).Should().Equal("foo", "bar");

            el = Doc.GetElementsByClassName("container");
            el.Should().HaveCount(4);
            el.Select(s => s.TagName).Should().Equal("nav", "header", "div", "footer");

            Doc.GetElementsByClassName("navs").Should().HaveCount(1);
            Doc.GetElementsByClassName("section").Should().HaveCount(1);
            Doc.GetElementsByClassName("main").Should().HaveCount(1);
            Doc.GetElementsByClassName("wrapper").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestGetElementsByTagName()
        {
            var el = Doc.Body.DescendantsByTagName("a");
            el.Should().HaveCount(5);
            el.Select(s => s.Text()).Should().Equal("Home", "Dashboard", "Settings", "Profile", "Help");

            Doc.GetElementsByTagName("header").Should().HaveCount(1);
            Doc.GetElementsByTagName("footer").Should().HaveCount(1);
            Doc.GetElementsByTagName("table").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestGetElements()
        {
            Doc.GetElements(s => s.TagName == "a" && s["href"] == @"/").Should().HaveCount(1);
            Doc.GetElements(s => s.TagName == "a" && s["href"] != @"/").Should().HaveCount(4);
            Doc.GetElements(s => s.HasClassName("container")).Should().HaveCount(4);
            Doc.GetElements(s => s["class"] == "container").Should().HaveCount(2);
            Doc.GetElements(s => s["class"] == "wrapper").Should().HaveCount(0);
            Doc.GetElements(s => s.Id == "navigation" && s.HasClassName("navigation")).Should().HaveCount(0);
            Doc.GetElements(s => s.Id == "container" && s.HasClassName("container")).Should().HaveCount(1);
        }
    }
}
