using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fnio.Lib.HtmlQuery.Node;
using FluentAssertions;

namespace Fnio.Lib.HtmlQuery.UnitTest.Node
{
    [TestClass]
    public class HtmlAttributeTest
    {
        [TestMethod]
        public void TestHtmlAttribute()
        {
            var fooAttr = new HtmlAttribute("id", "foo");
            fooAttr.Name.Should().Be("id");
            fooAttr.Value.Should().Be("foo");

            var barAttr = new HtmlAttribute("ID", "bar");
            barAttr.Name.Should().Be("id");
            barAttr.Value.Should().Be("bar");
        }
    }
}
