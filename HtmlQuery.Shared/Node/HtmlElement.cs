using System;
using System.Collections.Generic;
using System.Linq;

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
        public string Id => Attributes["id"];

        /// <summary>
        /// Get all class names of element.
        /// </summary>
        public IEnumerable<string> ClassNames => Attributes["class"]?.Split(ClassNameSeperators, StringSplitOptions.RemoveEmptyEntries)
                                                 ?? Enumerable.Empty<string>();

        /// <summary>
        /// Get all child nodes of element.
        /// </summary>
        public IReadOnlyList<HtmlNode> ChildNodes => _childNodes;

        /// <summary>
        /// Get attribute value of specific attribute name.
        /// </summary>
        public string this[string attributeName] => Attributes?[attributeName];

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="doc">the document that the element belongs to</param>
        /// <param name="tagName">element tag name</param>
        /// <param name="attributes">attributes of the element</param>
        /// <param name="childNodes">child nodes of the element</param>
        internal HtmlElement(HtmlDocument doc, string tagName, HtmlAttributes attributes, IEnumerable<HtmlNode> childNodes)
        {
            Document = doc;
            TagName = tagName.ToLowerInvariant();
            Attributes = attributes ?? new HtmlAttributes();
            AppendChildNodes(childNodes);
        }

        /// <summary>
        /// Append a node to child node list.
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws when trying to append null as child.</exception>
        /// <exception cref="HtmlDomException">Throws when trying to append node that is not created by the same html document.</exception>
        public HtmlElement AppendChild(HtmlNode child)
        {
            if (child == null)
            {
                throw new ArgumentNullException(nameof(child));
            }

            if (Document != child.Document)
            {
                throw new HtmlDomException("Try to add node that is not created by current document");
            }

            child.Parent = this;
            _childNodes.Add(child);
            return this;
        }

        /// <summary>
        /// Append nodes to child node list.
        /// </summary>
        public HtmlElement AppendChildNodes(IEnumerable<HtmlNode> childNodes)
        {
            if (childNodes == null)
            {
                return this;
            }

            foreach (var child in childNodes)
            {
                AppendChild(child);
            }

            return this;
        }

        /// <summary>
        /// Get the inner text value of current element.
        /// </summary>
        public string Text() => string.Join(" ", InnerTextNodes().Select(n => n.Text.Trim())).Trim();

        /// <summary>
        /// Get a sequence of text nodes with depth-first traversal.
        /// </summary>
        private IEnumerable<HtmlTextNode> InnerTextNodes()
        {
            return ChildNodes
                .SelectMany(n =>
                {
                    var r = Enumerable.Empty<HtmlTextNode>();
                    if (n is HtmlElement) r = r.Concat((n as HtmlElement).InnerTextNodes());
                    if (n is HtmlTextNode) r = r.Concat(new[] { n as HtmlTextNode });
                    return r;
                });
        }

        /// <summary>
        /// List that host all child nodes of element.
        /// </summary>
        private readonly List<HtmlNode> _childNodes = new List<HtmlNode>();

        private static readonly char[] ClassNameSeperators = { ' ' };
    }
}
