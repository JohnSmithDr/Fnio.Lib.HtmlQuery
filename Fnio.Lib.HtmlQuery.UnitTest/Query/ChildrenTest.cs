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
        public void TestChildById()
        {
            Doc.Body.ChildById("navigation").TagName.Should().Be("nav");
            Doc.Body.ChildById("container").TagName.Should().Be("div");
        }

        [TestMethod]
        public void TestChildrenByClassName()
        {
            var el = Doc.Body.ChildrenByClassName("container");
            el.Should().HaveCount(4);
            el.Select(s => s.TagName).Should().Equal("nav", "header", "div", "footer");

            Doc.Body.ChildrenByClassName("navs").Should().HaveCount(1);
            Doc.Body.ChildrenByClassName("section").Should().HaveCount(1);
            Doc.Body.ChildrenByClassName("main").Should().HaveCount(1);
            Doc.Body.ChildrenByClassName("wrapper").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestChildrenByTagName()
        {
            Doc.Body.ChildrenByTagName("div").Should().HaveCount(1);
            Doc.Body.ChildrenByTagName("header").Should().HaveCount(1);
            Doc.Body.ChildrenByTagName("footer").Should().HaveCount(1);
            Doc.Body.ChildrenByTagName("table").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestChildren()
        {
            var c = Doc.Body.Children();
            c.Should().HaveCount(4);
            c.Select(s => s.TagName).Should().Equal("nav", "header", "div", "footer");

            Doc.Body.Children(s => s.Id == "navigation" && s.HasClassName("navigation")).Should().HaveCount(0);
            Doc.Body.Children(s => s.Id == "container" && s.HasClassName("container")).Should().HaveCount(1);
        }
    }
}
