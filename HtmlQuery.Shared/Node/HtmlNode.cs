using System;

namespace Fnio.Lib.HtmlQuery.Node
{
    /// <summary>
    /// Abstract html node.
    /// </summary>
    public abstract class HtmlNode
    {
        /// <summary>
        /// Get base uri of node.
        /// </summary>
        public Uri BaseUri => Document.BaseUri;

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
