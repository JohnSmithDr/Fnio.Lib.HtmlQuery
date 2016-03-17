using FluentAssertions;
using Fnio.Lib.HtmlQuery.Node;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Fnio.Lib.HtmlQuery.Universal.UnitTest
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
