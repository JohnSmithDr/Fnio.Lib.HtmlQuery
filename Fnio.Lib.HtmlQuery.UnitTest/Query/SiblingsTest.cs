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
        public void TestSiblings()
        {
            var cases = new Dictionary<string, string[]>
            {
                { "nav", new string[] { "header", "div", "footer" } },
                { "header", new string[] { "nav", "div", "footer" } },
                { "div", new string[] { "nav", "header", "footer" } },
                { "footer", new string[] { "nav", "header", "div" } },
            };

            foreach (var p in cases)
            {
                var el = Doc.Body.ChildrenByTagName(p.Key).FirstOrDefault();
                var siblings = el.Siblings();
                siblings.Should().HaveCount(p.Value.Length);
                siblings.All(s => s is HtmlElement).Should().BeTrue();
                siblings.OfType<HtmlElement>().Select(s => s.TagName).Should().Equal(p.Value);
            }

            var title = Doc.Head.ChildrenByTagName("title").FirstOrDefault();
            title.Siblings(s => (s as HtmlElement)?.TagName == "meta").Should().HaveCount(1);
            title.Siblings(s => (s as HtmlElement)?.TagName == "link").Should().HaveCount(1);
            title.Siblings(s => (s as HtmlElement)?.TagName == "script").Should().HaveCount(1);
            title.Siblings(s => (s as HtmlElement)?.TagName == "style").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestSiblingById()
        {
            var header = Doc.Body.ChildrenByTagName("header").FirstOrDefault();
            header.SiblingById("navigation").Should().NotBeNull();
            header.SiblingById("container").Should().NotBeNull();
            header.SiblingById("wrapper").Should().BeNull();
        }

        [TestMethod]
        public void TestSiblingsByClassName()
        {
            var nav = Doc.Body.ChildById("navigation");
            nav.SiblingsByClassName("container").Should().HaveCount(3);
            nav.SiblingsByClassName("wrapper").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestSiblingsByTagName()
        {
            var nav = Doc.Body.ChildById("navigation");
            nav.SiblingsByTagName("header").Should().HaveCount(1);
            nav.SiblingsByTagName("footer").Should().HaveCount(1);
            nav.SiblingsByTagName("wrapper").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestNodesBeforeSelf()
        {
            var cases = new Dictionary<string, string[]>
            {
                { "nav", new string[] { } },
                { "header", new string[] { "nav" } },
                { "div", new string[] { "nav", "header" } },
                { "footer", new string[] { "nav", "header", "div" } },
            };

            foreach (var p in cases)
            {
                var el = Doc.Body.ChildrenByTagName(p.Key).FirstOrDefault();
                var siblings = el.NodesBeforeSelf();
                siblings.Should().HaveCount(p.Value.Length);
                siblings.OfType<HtmlElement>().Select(s => s.TagName).Should().Equal(p.Value);
            }

            var title = Doc.Head.ChildrenByTagName("title").FirstOrDefault();
            title.NodesBeforeSelf(s => (s as HtmlElement)?.TagName == "meta").Should().HaveCount(1);
            title.NodesBeforeSelf(s => (s as HtmlElement)?.TagName == "link").Should().HaveCount(0);
            title.NodesBeforeSelf(s => (s as HtmlElement)?.TagName == "script").Should().HaveCount(0);
            title.NodesBeforeSelf(s => (s as HtmlElement)?.TagName == "style").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestNodesAfterSelf()
        {
            var cases = new Dictionary<string, string[]>
            {
                { "nav", new string[] { "header", "div", "footer" } },
                { "header", new string[] { "div", "footer" } },
                { "div", new string[] { "footer" } },
                { "footer", new string[] { } },
            };

            foreach (var p in cases)
            {
                var el = Doc.Body.ChildrenByTagName(p.Key).FirstOrDefault();
                var siblings = el.NodesAfterSelf();
                siblings.Should().HaveCount(p.Value.Length);
                siblings.OfType<HtmlElement>().Select(s => s.TagName).Should().Equal(p.Value);
            }

            var title = Doc.Head.ChildrenByTagName("title").FirstOrDefault();
            title.NodesAfterSelf(s => (s as HtmlElement)?.TagName == "meta").Should().HaveCount(0);
            title.NodesAfterSelf(s => (s as HtmlElement)?.TagName == "link").Should().HaveCount(1);
            title.NodesAfterSelf(s => (s as HtmlElement)?.TagName == "script").Should().HaveCount(1);
            title.NodesAfterSelf(s => (s as HtmlElement)?.TagName == "style").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestElementsBeforeSelf()
        {
            var cases = new Dictionary<string, string[]>
            {
                { "nav", new string[] { } },
                { "header", new string[] { "nav" } },
                { "div", new string[] { "nav", "header" } },
                { "footer", new string[] { "nav", "header", "div" } },
            };

            foreach (var p in cases)
            {
                var el = Doc.Body.ChildrenByTagName(p.Key).FirstOrDefault();
                var siblings = el.ElementsBeforeSelf();
                siblings.Should().HaveCount(p.Value.Length);
                siblings.Select(s => s.TagName).Should().Equal(p.Value);
            }

            var title = Doc.Head.ChildrenByTagName("title").FirstOrDefault();
            title.ElementsBeforeSelf(s => s.TagName == "meta").Should().HaveCount(1);
            title.ElementsBeforeSelf(s => s.TagName == "link").Should().HaveCount(0);
            title.ElementsBeforeSelf(s => s.TagName == "script").Should().HaveCount(0);
            title.ElementsBeforeSelf(s => s.TagName == "style").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestElementsAfterSelf()
        {
            var cases = new Dictionary<string, string[]>
            {
                { "nav", new string[] { "header", "div", "footer" } },
                { "header", new string[] { "div", "footer" } },
                { "div", new string[] { "footer" } },
                { "footer", new string[] { } },
            };

            foreach (var p in cases)
            {
                var el = Doc.Body.ChildrenByTagName(p.Key).FirstOrDefault();
                var siblings = el.ElementsAfterSelf();
                siblings.Should().HaveCount(p.Value.Length);
                siblings.Select(s => s.TagName).Should().Equal(p.Value);
            }

            var title = Doc.Head.ChildrenByTagName("title").FirstOrDefault();
            title.ElementsAfterSelf(s => s.TagName == "meta").Should().HaveCount(0);
            title.ElementsAfterSelf(s => s.TagName == "link").Should().HaveCount(1);
            title.ElementsAfterSelf(s => s.TagName == "script").Should().HaveCount(1);
            title.ElementsAfterSelf(s => s.TagName == "style").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestPreviousSibling()
        {
            var cases = new Dictionary<string, string>
            {
                { "nav", "" },
                { "header", "nav" },
                { "div", "header" },
                { "footer", "div" },
            };

            foreach (var p in cases)
            {
                var el = Doc.Body.ChildrenByTagName(p.Key).FirstOrDefault();
                var prev = el.PreviousSibling();

                if (string.IsNullOrEmpty(p.Value))
                {
                    prev.Should().BeNull();
                    continue;
                }

                (prev as HtmlElement).TagName.Should().Be(p.Value);
            }
        }

        [TestMethod]
        public void TestNextSibling()
        {
            var cases = new Dictionary<string, string>
            {
                { "nav", "header" },
                { "header", "div" },
                { "div", "footer" },
                { "footer", "" },
            };

            foreach (var p in cases)
            {
                var el = Doc.Body.ChildrenByTagName(p.Key).FirstOrDefault();
                var next = el.NextSibling();

                if (string.IsNullOrEmpty(p.Value))
                {
                    next.Should().BeNull();
                    continue;
                }

                (next as HtmlElement).TagName.Should().Be(p.Value);
            }
        }
    }
}
