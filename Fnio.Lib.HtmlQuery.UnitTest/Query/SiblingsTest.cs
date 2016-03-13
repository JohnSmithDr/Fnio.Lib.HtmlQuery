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
        public void TestGetSiblings()
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
                var el = Doc.Body.GetChildrenByTagName(p.Key).FirstOrDefault();
                var siblings = el.GetSiblings();
                siblings.Should().HaveCount(p.Value.Length);
                siblings.All(s => s is HtmlElement).Should().BeTrue();
                siblings.OfType<HtmlElement>().Select(s => s.TagName).Should().Equal(p.Value);
            }

            var title = Doc.Head.GetChildrenByTagName("title").FirstOrDefault();
            title.GetSiblings(s => (s as HtmlElement)?.TagName == "meta").Should().HaveCount(1);
            title.GetSiblings(s => (s as HtmlElement)?.TagName == "link").Should().HaveCount(1);
            title.GetSiblings(s => (s as HtmlElement)?.TagName == "script").Should().HaveCount(1);
            title.GetSiblings(s => (s as HtmlElement)?.TagName == "style").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestGetSiblingById()
        {
            var header = Doc.Body.GetChildrenByTagName("header").FirstOrDefault();
            header.GetSiblingById("navigation").Should().NotBeNull();
            header.GetSiblingById("container").Should().NotBeNull();
            header.GetSiblingById("wrapper").Should().BeNull();
        }

        [TestMethod]
        public void TestGetSiblingsByClassName()
        {
            var nav = Doc.Body.GetChildById("navigation");
            nav.GetSiblingsByClassName("container").Should().HaveCount(3);
            nav.GetSiblingsByClassName("wrapper").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestGetSiblingsByTagName()
        {
            var nav = Doc.Body.GetChildById("navigation");
            nav.GetSiblingsByTagName("header").Should().HaveCount(1);
            nav.GetSiblingsByTagName("footer").Should().HaveCount(1);
            nav.GetSiblingsByTagName("wrapper").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestGetNodesBeforeSelf()
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
                var el = Doc.Body.GetChildrenByTagName(p.Key).FirstOrDefault();
                var siblings = el.GetNodesBeforeSelf();
                siblings.Should().HaveCount(p.Value.Length);
                siblings.OfType<HtmlElement>().Select(s => s.TagName).Should().Equal(p.Value);
            }

            var title = Doc.Head.GetChildrenByTagName("title").FirstOrDefault();
            title.GetNodesBeforeSelf(s => (s as HtmlElement)?.TagName == "meta").Should().HaveCount(1);
            title.GetNodesBeforeSelf(s => (s as HtmlElement)?.TagName == "link").Should().HaveCount(0);
            title.GetNodesBeforeSelf(s => (s as HtmlElement)?.TagName == "script").Should().HaveCount(0);
            title.GetNodesBeforeSelf(s => (s as HtmlElement)?.TagName == "style").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestGetNodesAfterSelf()
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
                var el = Doc.Body.GetChildrenByTagName(p.Key).FirstOrDefault();
                var siblings = el.GetNodesAfterSelf();
                siblings.Should().HaveCount(p.Value.Length);
                siblings.OfType<HtmlElement>().Select(s => s.TagName).Should().Equal(p.Value);
            }

            var title = Doc.Head.GetChildrenByTagName("title").FirstOrDefault();
            title.GetNodesAfterSelf(s => (s as HtmlElement)?.TagName == "meta").Should().HaveCount(0);
            title.GetNodesAfterSelf(s => (s as HtmlElement)?.TagName == "link").Should().HaveCount(1);
            title.GetNodesAfterSelf(s => (s as HtmlElement)?.TagName == "script").Should().HaveCount(1);
            title.GetNodesAfterSelf(s => (s as HtmlElement)?.TagName == "style").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestGetElementsBeforeSelf()
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
                var el = Doc.Body.GetChildrenByTagName(p.Key).FirstOrDefault();
                var siblings = el.GetElementsBeforeSelf();
                siblings.Should().HaveCount(p.Value.Length);
                siblings.Select(s => s.TagName).Should().Equal(p.Value);
            }

            var title = Doc.Head.GetChildrenByTagName("title").FirstOrDefault();
            title.GetElementsBeforeSelf(s => s.TagName == "meta").Should().HaveCount(1);
            title.GetElementsBeforeSelf(s => s.TagName == "link").Should().HaveCount(0);
            title.GetElementsBeforeSelf(s => s.TagName == "script").Should().HaveCount(0);
            title.GetElementsBeforeSelf(s => s.TagName == "style").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestGetElementsAfterSelf()
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
                var el = Doc.Body.GetChildrenByTagName(p.Key).FirstOrDefault();
                var siblings = el.GetElementsAfterSelf();
                siblings.Should().HaveCount(p.Value.Length);
                siblings.Select(s => s.TagName).Should().Equal(p.Value);
            }

            var title = Doc.Head.GetChildrenByTagName("title").FirstOrDefault();
            title.GetElementsAfterSelf(s => s.TagName == "meta").Should().HaveCount(0);
            title.GetElementsAfterSelf(s => s.TagName == "link").Should().HaveCount(1);
            title.GetElementsAfterSelf(s => s.TagName == "script").Should().HaveCount(1);
            title.GetElementsAfterSelf(s => s.TagName == "style").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestGetPreviousSibling()
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
                var el = Doc.Body.GetChildrenByTagName(p.Key).FirstOrDefault();
                var prev = el.GetPreviousSibling();

                if (string.IsNullOrEmpty(p.Value))
                {
                    prev.Should().BeNull();
                    continue;
                }

                (prev as HtmlElement).TagName.Should().Be(p.Value);
            }
        }

        [TestMethod]
        public void TestGetNextSibling()
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
                var el = Doc.Body.GetChildrenByTagName(p.Key).FirstOrDefault();
                var next = el.GetNextSibling();

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
