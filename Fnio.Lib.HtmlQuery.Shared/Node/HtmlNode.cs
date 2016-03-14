using System;
using System.Collections.Generic;
using System.Text;

namespace Fnio.Lib.HtmlQuery.Node
{
    /// <summary>
    /// Abstract html node.
    /// </summary>
    public abstract class HtmlNode
    {
        /// <summary>
        /// Get the parent node of current node.
        /// </summary>
        public HtmlNode Parent { get; internal set; }

        /// <summary>
        /// Get the html document reference of current node.
        /// </summary>
        public HtmlDocument Document { get; internal set; }
    }
}
