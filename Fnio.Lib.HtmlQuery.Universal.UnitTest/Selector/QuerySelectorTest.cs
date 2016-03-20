using System.Linq;
using FluentAssertions;
using Fnio.Lib.HtmlQuery.Node;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Fnio.Lib.HtmlQuery.Universal.UnitTest
{
    [TestClass]
    public class QuerySelectorTest
    {
        public HtmlDocument Doc { get; } = HtmlParser.Parse(TestData.SimpleHtmlDoc);

        [TestMethod]
        public void TestQueryAll()
        {
            var all = Doc.QuerySelector("*");
            all.First().TagName.Should().Be("html");

            Doc.QuerySelector("head").First().QuerySelector("*").Should().HaveCount(5);
        }

        [TestMethod]
        public void TestQueryId()
        {
            var nav = Doc.QuerySelector("#navigation").First();
            nav.TagName.Should().Be("nav");
            nav.ClassNames.Should().Equal("container", "navs");

            nav.QuerySelector("#home-link").First().TagName.Should().Be("a");
            nav.QuerySelector("#links").Should().HaveCount(0);

            Doc.QuerySelector("#navy").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestQueryClass()
        {
            var containers = Doc.QuerySelector(".container");
            containers.Should().HaveCount(4);
            containers.Select(s => s.TagName).Should().Equal("nav", "header", "div", "footer");

            Doc.QuerySelector("div").First().QuerySelector(".info").Should().HaveCount(2);
            Doc.QuerySelector(".wrapper").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestQueryTag()
        {
            Doc.QuerySelector("table").Should().HaveCount(0);
            Doc.QuerySelector("header").Should().HaveCount(1);
            Doc.QuerySelector("footer").Should().HaveCount(1);
            Doc.QuerySelector("li").Should().HaveCount(4);
            Doc.QuerySelector("a").Should().HaveCount(5);
            Doc.QuerySelector("ul").First().QuerySelector("a").Should().HaveCount(4);
        }

        [TestMethod]
        public void TestQueryAttribute()
        {
            Doc.QuerySelector("[charset]").Should().HaveCount(1);
            Doc.QuerySelector("[href]").Should().HaveCount(6);
            Doc.QuerySelector("a[href]").Should().HaveCount(5);
            Doc.QuerySelector("[input]").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestQueryAttributeWithValue()
        {
            Doc.QuerySelector("[href=/]").Should().HaveCount(1);
            Doc.QuerySelector("[alt=logo]").Should().HaveCount(1);
            Doc.QuerySelector("[data=foo]").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestQueryAttributeWithValueNot()
        {
            Doc.QuerySelector("a[href!=#]").Should().HaveCount(5);
            Doc.QuerySelector("a[href!=/]").Should().HaveCount(4);
            Doc.QuerySelector("img[alt!=logo]").Should().HaveCount(0);
            Doc.QuerySelector("div[data!=foo]").Should().HaveCount(1);
            Doc.QuerySelector("meta[charset!=utf-8]").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestQueryAttributeWithValueStarting()
        {
            Doc.QuerySelector("a[href^=/]").Should().HaveCount(5);
            Doc.QuerySelector("[href^=#]").Should().HaveCount(0);
            Doc.QuerySelector("[src^=http]").Should().HaveCount(1);
        }

        [TestMethod]
        public void TestQueryAttributeWithValueEnding()
        {
            Doc.QuerySelector("img[src$=js]").Should().HaveCount(0);
            Doc.QuerySelector("a[href$=/]").Should().HaveCount(1);
            Doc.QuerySelector("[id$=foo]").Should().HaveCount(0);
            Doc.QuerySelector("[class$=er]").Should().HaveCount(3);
            Doc.QuerySelector("[src$=js]").Should().HaveCount(1);
        }

        [TestMethod]
        public void TestQueryAttributeWithValueContaining()
        {
            Doc.QuerySelector("[id*=-]").Should().HaveCount(1);
            Doc.QuerySelector("[class*=_]").Should().HaveCount(0);
            Doc.QuerySelector("[class*=container]").Should().HaveCount(4);
        }

        [TestMethod]
        public void TestQueryAttributeWithValueMatching()
        {
            Doc.QuerySelector("[href~=\\w+]").Should().HaveCount(5);
            Doc.QuerySelector("[href~=[w]+]").Should().HaveCount(1);
            Doc.QuerySelector("[id~=\\d+]").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestQueryIndexLessThen()
        {
            Doc.QuerySelector(".info:lt(5)").Should().HaveCount(2);
            Doc.QuerySelector("div:lt(2)").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestQueryIndexGreaterThen()
        {
            Doc.QuerySelector(".info:gt(5)").Should().HaveCount(0);
            Doc.QuerySelector("div:gt(1)").Should().HaveCount(1);
        }

        [TestMethod]
        public void TestQueryIndexEquals()
        {
            Doc.QuerySelector(".info:eq(0)").Should().HaveCount(1);
            Doc.QuerySelector(".info:eq(1)").Should().HaveCount(1);
            Doc.QuerySelector(".info:eq(2)").Should().HaveCount(0);
            Doc.QuerySelector("div:eq(2)").Should().HaveCount(1);
        }

        [TestMethod]
        public void TestQueryContainsText()
        {
            Doc.QuerySelector("h1:contains(Example)").Should().HaveCount(1);
            Doc.QuerySelector("p:contains(foo)").Should().HaveCount(1);
            Doc.QuerySelector("p:contains(bar)").Should().HaveCount(1);
            Doc.QuerySelector(":contains(main)").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestQueryMatchesRegex()
        {
            Doc.QuerySelector("p:matches(\\w+)").Should().HaveCount(2);
            Doc.QuerySelector("p:matches(\\d+)").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestQueryOr()
        {
            Doc.QuerySelector("nav, header, footer, section").Should().HaveCount(3);
            Doc.QuerySelector("nav, header, footer, .section").Should().HaveCount(4);
        }

        [TestMethod]
        public void TestQueryChildren()
        {
            Doc.QuerySelector("nav > a").Should().HaveCount(1);
            Doc.QuerySelector("div > p").Should().HaveCount(2);
            Doc.QuerySelector("div > span").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestQueryDescendants()
        {
            Doc.QuerySelector("nav a").Should().HaveCount(5);
            Doc.QuerySelector("nav li a").Should().HaveCount(4);
            Doc.QuerySelector("nav li > a").Should().HaveCount(4);
            Doc.QuerySelector("div p").Should().HaveCount(2);
            Doc.QuerySelector("div span").Should().HaveCount(0);
        }

        [TestMethod]
        public void TestQueryNextBy()
        {
            Doc.QuerySelector("head title + meta").Should().HaveCount(0);
            var q = Doc.QuerySelector("head meta + title");
            q.Should().HaveCount(1);
            q.First().TagName.Should().Be("title");

            q = Doc.QuerySelector("head meta + title + link");
            q.Should().HaveCount(1);
            q.First().TagName.Should().Be("link");
        }
    }
}
