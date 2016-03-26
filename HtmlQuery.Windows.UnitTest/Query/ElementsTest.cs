using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fnio.Lib.HtmlQuery.Node;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace Fnio.Lib.HtmlQuery.Windows.UnitTest
{
    public partial class HtmlQueryTest
    {
        [TestMethod]
        public void TestAsTraversable()
        {
            Doc.Head.AsTraversable().Select(s => s.TagName).Should().Equal("head", "meta", "title", "link", "script");
        }

        [TestMethod]
        public void TestHasAttribute()
        {
            var nav = Doc.Body.ChildById("navigation");
            nav.HasAttribute("id").Should().BeTrue();
            nav.HasAttribute("CLASS").Should().BeTrue();
            nav.HasAttribute("Style").Should().BeFalse();
            nav.HasAttribute().Should().BeTrue();
        }

        [TestMethod]
        public void TestHasClassName()
        {
            var nav = Doc.Body.ChildById("navigation");
            nav.HasClassName("container").Should().BeTrue();
            nav.HasClassName("navs").Should().BeTrue();
            nav.HasClassName("wrapper").Should().BeFalse();
        }

        [TestMethod]
        public void TestContainsClassNames()
        {
            var nav = Doc.Body.ChildById("navigation");
            nav.ContainsClassNames(new List<string> { "container", "navs" }).Should().BeTrue();
            nav.ContainsClassNames(new List<string> { "wrapper", "navs" }).Should().BeFalse();
            nav.ContainsClassNames(new List<string> { }).Should().BeFalse();
        }

        [TestMethod]
        public void TestContainsClassNamesWithParams()
        {
            var nav = Doc.Body.ChildById("navigation");
            nav.ContainsClassNames("container", "navs").Should().BeTrue();
            nav.ContainsClassNames("wrapper", "navs").Should().BeFalse();
            nav.ContainsClassNames().Should().BeFalse();
        }
        
    }
}
