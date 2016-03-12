using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fnio.Lib.HtmlQuery.Node;
using FluentAssertions;
using System.Linq;

namespace Fnio.Lib.HtmlQuery.UnitTest.Query
{
    [TestClass]
    public class ChildrenTest
    {
        public HtmlDocument Doc { get; } = HtmlParser.Parse(TestData.SimpleHtmlDoc);

        [TestMethod]
        public void TestGetChildById()
        {
            Doc.Body.GetChildById("navigation").TagName.Should().Be("nav");
            Doc.Body.GetChildById("container").TagName.Should().Be("div");
        }

        [TestMethod]
        public void TestGetChildrenByClassName()
        {
            var el = Doc.Body.GetChildrenByClassName("container");
            el.Should().HaveCount(4);
            el.Select(s => s.TagName).Should().Equal("nav", "header", "div", "footer");

            Doc.Body.GetChildrenByClassName("navs").Should().HaveCount(1);
            Doc.Body.GetChildrenByClassName("section").Should().HaveCount(1);
            Doc.Body.GetChildrenByClassName("main").Should().HaveCount(1);
            Doc.Body.GetChildrenByClassName("wrapper").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestGetChildrenByTagName()
        {
            Doc.Body.GetChildrenByTagName("div").Should().HaveCount(1);
            Doc.Body.GetChildrenByTagName("header").Should().HaveCount(1);
            Doc.Body.GetChildrenByTagName("footer").Should().HaveCount(1);
            Doc.Body.GetChildrenByTagName("table").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestGetChildren()
        {
            Doc.Body.GetChildren(s => s.Id == "navigation" && s.HasClassName("navigation")).Should().HaveCount(0);
            Doc.Body.GetChildren(s => s.Id == "container" && s.HasClassName("container")).Should().HaveCount(1);
        }
    }
}
