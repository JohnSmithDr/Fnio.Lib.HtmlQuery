using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Fnio.Lib.HtmlQuery.Node;

namespace Fnio.Lib.HtmlQuery.Universal.UnitTest
{
    [TestClass]
    public partial class HtmlQueryTest
    {
        public HtmlDocument Doc { get; } = HtmlParser.Parse(TestData.SimpleHtmlDoc);
    }
}
