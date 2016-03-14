using FluentAssertions;
using Fnio.Lib.HtmlQuery.Node;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fnio.Lib.HtmlQuery.UnitTest
{
    public partial class HtmlQueryTest
    {
        [TestMethod]
        public void TestAncestors()
        {
            Doc.GetElementById("logo").Ancestors().Select(s => s.TagName).Should().Equal("html", "body", "header");
            Doc.GetElementById("home-link").Ancestors().Select(s => s.TagName).Should().Equal("html", "body", "nav");
            Doc.GetElementsByClassName("info").FirstOrDefault().Ancestors(s => s.TagName == "div").Should().HaveCount(1);
            Doc.GetElementsByClassName("info").FirstOrDefault().Ancestors(s => s.TagName == "p").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestAncestorById()
        {
            Doc.GetElementById("home-link").AncestorById("navigation").Should().NotBeNull();
            Doc.GetElementById("logo").AncestorById("header").Should().BeNull();
        }

        [TestMethod]
        public void TestAncestorsByClassName()
        {
            Doc.GetElementById("home-link").AncestorsByClassName("container").Should().HaveCount(1);
            Doc.GetElementById("logo").AncestorsByClassName("container").Should().HaveCount(1);
            Doc.GetElementById("container").AncestorsByClassName("wrapper").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestAncestorsByTagName()
        {
            Doc.GetElementById("home-link").AncestorsByTagName("nav").Should().HaveCount(1);
            Doc.GetElementById("logo").AncestorsByTagName("body").Should().HaveCount(1);
            Doc.GetElementById("container").AncestorsByTagName("div").Should().HaveCount(0);
        }
    }
}
