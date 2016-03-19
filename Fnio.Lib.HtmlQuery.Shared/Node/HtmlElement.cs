using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fnio.Lib.HtmlQuery.Node
{
    /// <summary>
    /// Html element node.
    /// </summary>
    public class HtmlElement : HtmlNode
    {
        /// <summary>
        /// Get tag name.
        /// </summary>
        public string TagName { get; private set; }

        /// <summary>
        /// Get attributes.
        /// </summary>
        public HtmlAttributes Attributes { get; }

        /// <summary>
        /// Get id of element, if defined.
        /// </summary>
        public string Id
        {
            get { return this.Attributes["id"]; }
        }

        /// <summary>
        /// Get all class names of element.
        /// </summary>
        public IEnumerable<string> ClassNames
        {
            get
            {
                return this.Attributes["class"]?.Split(ClassNameSeperators, StringSplitOptions.RemoveEmptyEntries)
                    ?? Enumerable.Empty<string>();
            }
        }

        /// <summary>
        /// Get all child nodes of element.
        /// </summary>
        public IReadOnlyList<HtmlNode> ChildNodes
        {
            get { return this._childNodes; }
        }

        /// <summary>
        /// Get attribute value of specific attribute name.
        /// </summary>
        public string this[string attributeName]
        {
            get { return this.Attributes?[attributeName]; }
        }

        internal HtmlElement(HtmlDocument doc, string tagName, HtmlAttributes attributes, IEnumerable<HtmlNode> childNodes)
        {
            this.Document = doc;
            this.TagName = tagName.ToLowerInvariant();
            this.Attributes = attributes ?? new HtmlAttributes();
            this.AppendChildNodes(childNodes);
        }

        /// <summary>
        /// Append a node to child node list.
        /// </summary>
        public HtmlElement AppendChild(HtmlNode child)
        {
            if (child == null)
                throw new ArgumentNullException(nameof(child));

            if (this.Document != child.Document)
                throw new HtmlDomException("Try to add node that is not created by current document");

            child.Parent = this;
            this._childNodes.Add(child);
            return this;
        }

        /// <summary>
        /// Append nodes to child node list.
        /// </summary>
        public HtmlElement AppendChildNodes(IEnumerable<HtmlNode> childNodes)
        {
            if (childNodes != null)
            {
                foreach (var child in childNodes)
                {
                    this.AppendChild(child);
                }
            }
            return this;
        }

        /// <summary>
        /// Get the inner text value of current element.
        /// </summary>
        public string Text()
        {
            var texts = this.InnerTextNodes().Select(n => n.Text.Trim());
            var sb = new StringBuilder();
            foreach (var text in texts)
            {
                sb.Append(text);
                sb.Append(' ');
            }
            return sb.ToString().Trim();
        }

        /// <summary>
        /// Get a sequence of text nodes with depth-first traversal.
        /// </summary>
        private IEnumerable<HtmlTextNode> InnerTextNodes()
        {
            return this.ChildNodes
                .SelectMany(n =>
                {
                    var r = Enumerable.Empty<HtmlTextNode>();
                    if (n is HtmlElement) r = r.Concat((n as HtmlElement).InnerTextNodes());
                    if (n is HtmlTextNode) r = r.Concat(new HtmlTextNode[] { n as HtmlTextNode });
                    return r;
                });
        }

        /// <summary>
        /// List that host all child nodes of element.
        /// </summary>
        private readonly List<HtmlNode> _childNodes = new List<HtmlNode>();

        private static readonly char[] ClassNameSeperators = new char[] { ' ' };
    }
}
