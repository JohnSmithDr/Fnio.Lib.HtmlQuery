using System;
using System.Collections.Generic;
using System.Text;

namespace Fnio.Lib.HtmlQuery.Node
{
    public abstract class HtmlNode
    {
        public HtmlNode Parent { get; internal set; }

        public HtmlDocument Document { get; internal set; }
    }
}
