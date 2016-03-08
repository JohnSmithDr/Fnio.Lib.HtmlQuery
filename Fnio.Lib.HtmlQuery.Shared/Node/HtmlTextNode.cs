using System;
using System.Collections.Generic;
using System.Text;

namespace Fnio.Lib.HtmlQuery.Node
{
    public class HtmlTextNode : HtmlNode
    {
        public string Text { get; private set; }

        internal HtmlTextNode(HtmlDocument doc, string text)
        {
            this.Document = doc;
            this.Text = text;
        }
    }
}
