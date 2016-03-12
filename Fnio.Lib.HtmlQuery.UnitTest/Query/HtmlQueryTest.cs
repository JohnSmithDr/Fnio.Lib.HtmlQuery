using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fnio.Lib.HtmlQuery.Node;

namespace Fnio.Lib.HtmlQuery.UnitTest
{
    [TestClass]
    public partial class HtmlQueryTest
    {
        public HtmlDocument Doc { get; } = HtmlParser.Parse(TestData.SimpleHtmlDoc);
    }
}
