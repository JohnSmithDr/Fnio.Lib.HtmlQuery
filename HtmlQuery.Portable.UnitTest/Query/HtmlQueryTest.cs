using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Fnio.Lib.HtmlQuery.Node;

namespace Fnio.Lib.HtmlQuery.Portable.UnitTest
{
    [TestClass]
    public partial class HtmlQueryTest
    {
        public HtmlDocument Doc { get; } = HtmlParser.Parse(TestData.SimpleHtmlDoc);
    }
}
