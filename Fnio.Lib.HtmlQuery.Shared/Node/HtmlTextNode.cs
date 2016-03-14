using System;
using System.Collections.Generic;
using System.Text;

namespace Fnio.Lib.HtmlQuery.Node
{
    /// <summary>
    /// Html text node.
    /// </summary>
    public class HtmlTextNode : HtmlNode
    {
        /// <summary>
        /// Get the text value of current node.
        /// </summary>
        public string Text { get; private set; }

        internal HtmlTextNode(HtmlDocument doc, string text)
        {
            this.Document = doc;
            this.Text = text;
        }
    }
}
