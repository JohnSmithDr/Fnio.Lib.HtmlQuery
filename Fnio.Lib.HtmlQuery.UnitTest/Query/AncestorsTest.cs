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
        public void TestGetAncestors()
        {
            Doc.GetElementById("logo").GetAncestors().Select(s => s.TagName).Should().Equal("html", "body", "header");
            Doc.GetElementById("home-link").GetAncestors().Select(s => s.TagName).Should().Equal("html", "body", "nav");
            Doc.GetElementsByClassName("info").FirstOrDefault().GetAncestors(s => s.TagName == "div").Should().HaveCount(1);
            Doc.GetElementsByClassName("info").FirstOrDefault().GetAncestors(s => s.TagName == "p").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestGetAncestorById()
        {
            Doc.GetElementById("home-link").GetAncestorById("navigation").Should().NotBeNull();
            Doc.GetElementById("logo").GetAncestorById("header").Should().BeNull();
        }

        [TestMethod]
        public void TestGetAncestorsByClassName()
        {
            Doc.GetElementById("home-link").GetAncestorsByClassName("container").Should().HaveCount(1);
            Doc.GetElementById("logo").GetAncestorsByClassName("container").Should().HaveCount(1);
            Doc.GetElementById("container").GetAncestorsByClassName("wrapper").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestGetAncestorsByTagName()
        {
            Doc.GetElementById("home-link").GetAncestorsByTagName("nav").Should().HaveCount(1);
            Doc.GetElementById("logo").GetAncestorsByTagName("body").Should().HaveCount(1);
            Doc.GetElementById("container").GetAncestorsByTagName("div").Should().HaveCount(0);
        }
    }
}
