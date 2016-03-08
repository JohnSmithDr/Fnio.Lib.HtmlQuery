using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fnio.Lib.HtmlQuery.Node
{
    public class HtmlElement : HtmlNode
    {
        public string TagName { get; private set; }

        public HtmlAttributes Attributes { get; private set; }

        public string Id
        {
            get { return this.Attributes["id"]; }
        }

        public IEnumerable<string> ClassNames
        {
            get
            {
                return this.Attributes["class"]?.Split(ClassNameSeperators, StringSplitOptions.RemoveEmptyEntries)
                    ?? Enumerable.Empty<string>();
            }
        }

        public IEnumerable<HtmlNode> ChildNodes
        {
            get { return this._childNodes; }
        }

        public string this[string key]
        {
            get { return this.Attributes?[key]; }
        }

        internal HtmlElement(HtmlDocument doc, string tagName, HtmlAttributes attributes, IEnumerable<HtmlNode> childNodes)
        {
            this.Document = doc;
            this.TagName = tagName.ToLowerInvariant();
            this.Attributes = attributes;
            this.AppendChildNodes(childNodes);
        }

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

        public IEnumerable<HtmlElement> Children()
        {
            return this.ChildNodes.OfType<HtmlElement>();
        }

        public IEnumerable<HtmlElement> Descendants()
        {
            return this.Children().SelectMany(e =>
            {
                return new HtmlElement[] { e }.Concat(e.Descendants());
            });
        }

        public string Text()
        {
            var texts = this.InnerTextNodes().Select(n => n.Text);
            var sb = new StringBuilder();
            foreach (var text in texts)
            {
                sb.Append(text);
                sb.Append(' ');
            }
            return sb.ToString().Trim();
        }

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

        private List<HtmlNode> _childNodes = new List<HtmlNode>();

        private static readonly char[] ClassNameSeperators = new char[] { ' ' };
    }
}
