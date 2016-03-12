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
    [TestClass]
    public class HtmlElementTest
    {
        public HtmlDocument Doc { get; } = new HtmlDocument();

        [TestMethod]
        public void TestTagName()
        {
            var el = Doc.CreateElement("a");
            el.TagName.Should().Be("a");

            el = Doc.CreateElement("IMG");
            el.TagName.Should().Be("img");
        }

        [TestMethod]
        public void TestAttributes()
        {
            var el = Doc.CreateElement("div");
            el.Attributes.Should().NotBeNull().And.HaveCount(0);

            var attrs = new HtmlAttributes();
            attrs["id"] = "foo";
            attrs["class"] = "bar";
            el = Doc.CreateElement("div", attrs);
            el.Attributes.Should().BeSameAs(attrs).And.HaveCount(2);
        }

        [TestMethod]
        public void TestIndexer()
        {
            var el = Doc.CreateElement("div");

            el.Attributes["id"] = "foo";
            el["ID"].Should().Be("foo");

            el.Attributes["CLASS"] = "bar";
            el["class"].Should().Be("bar");
        }

        [TestMethod]
        public void TestId()
        {
            var el = Doc.CreateElement("div");

            el.Attributes["id"] = "foo";
            el.Id.Should().Be("foo");

            el.Attributes["ID"] = "bar";
            el.Id.Should().Be("bar");
        }

        [TestMethod]
        public void TestClassName()
        {
            var el = Doc.CreateElement("div");

            el.Attributes["class"] = "foo";
            el.ClassNames.Should().Equal("foo");

            el.Attributes["CLASS"] = "foo bar foo-bar";
            el.ClassNames.Should().Equal("foo", "bar", "foo-bar");
        }

        [TestMethod]
        public void TestChildNodes()
        {
            var el = Doc.CreateElement("div");
            el.ChildNodes.Should().HaveCount(0);

            el.AppendChild(Doc.CreateElement("p"));
            el.AppendChild(Doc.CreateTextNode("foo"));
            el.AppendChild(Doc.CreateTextNode("bar"));

            el.ChildNodes.Should().HaveCount(3);
            el.ChildNodes[0].Should().BeOfType<HtmlElement>();
            el.ChildNodes[1].Should().BeOfType<HtmlTextNode>();
            el.ChildNodes[2].Should().BeOfType<HtmlTextNode>();
        }

        [TestMethod]
        public void TestAppendChild()
        {
            var el = Doc.CreateElement("a");
            var tx = Doc.CreateTextNode("foo");
            var p = Doc.CreateElement("p");
            p.AppendChild(el).Should().Be(p);
            p.AppendChild(tx).Should().Be(p);
            p.ChildNodes.Should().HaveCount(2);
            p.ChildNodes.Should().Equal(el, tx);

            // should throw exception for null
            //
            p.Invoking(s => s.AppendChild(null)).ShouldThrow<ArgumentNullException>();

            // should throw exception for appending node that create by another document
            //
            var anotherDoc = new HtmlDocument();
            anotherDoc.CreateElement("a").Invoking(s => p.AppendChild(s)).ShouldThrow<HtmlDomException>();
            anotherDoc.CreateTextNode("bar").Invoking(s => p.AppendChild(s)).ShouldThrow<HtmlDomException>();
        }

        [TestMethod]
        public void AppendChildNodes()
        {
            var el = Doc.CreateElement("a");
            var tx = Doc.CreateTextNode("foo");
            var p = Doc.CreateElement("p");
            p.AppendChildNodes(new HtmlNode[] { el, tx }).Should().Be(p);
            p.ChildNodes.Should().HaveCount(2);
            p.ChildNodes.Should().Equal(el, tx);
        }

        [TestMethod]
        public void TestChildren()
        {
            var ul = Doc.CreateElement("ul", 
                Doc.CreateElement("li",
                    Doc.CreateElement("a", 
                        Doc.CreateTextNode("foo"))),
                Doc.CreateElement("li",
                    Doc.CreateElement("a", 
                        Doc.CreateTextNode("bar")))
                );

            ul.Children().Should().HaveCount(2);
            ul.Children().Select(s => s.TagName).Should().Equal("li", "li");
        }

        [TestMethod]
        public void TestDescendants()
        {
            var ul = Doc.CreateElement("ul",
                Doc.CreateElement("li",
                    Doc.CreateElement("a",
                        Doc.CreateTextNode("foo"))),
                Doc.CreateElement("li",
                    Doc.CreateElement("a",
                        Doc.CreateTextNode("bar")))
                );

            ul.Descendants().Should().HaveCount(4);
            ul.Descendants().Select(s => s.TagName).Should().Equal("li", "a", "li", "a");
        }

        [TestMethod]
        public void TestText()
        {
            var spanFoo = Doc.CreateElement("span", Doc.CreateTextNode(" foo "));
            var spanBar = Doc.CreateElement("span", Doc.CreateTextNode(" bar "));
            var div = Doc.CreateElement("div", spanFoo, spanBar);
            div.Text().Should().Be("foo bar");
        }
    }
}
