using System;
using System.Collections.Generic;
using System.Linq;

namespace Fnio.Lib.HtmlQuery.Node
{
    public class HtmlDocument
    {
        public Uri BaseUri { get; internal set; }

        public HtmlElement Root { get; private set; }

        public HtmlElement Head => Root?.Children().FirstOrDefault(c => c.TagName == "head");

        public HtmlElement Body => Root?.Children().FirstOrDefault(c => c.TagName == "body");

        public string Title => Head?.Children().FirstOrDefault(c => c.TagName == "title")?.Text()?.Trim();

        public HtmlElement CreateElement(string tag) => new HtmlElement(this, tag, null, null);

        public HtmlElement CreateElement(string tag, HtmlAttributes attributes) => new HtmlElement(this, tag, attributes, null);

        public HtmlElement CreateElement(string tag, IEnumerable<HtmlNode> childNodes) => new HtmlElement(this, tag, null, childNodes);

        public HtmlElement CreateElement(string tag, params HtmlNode[] childNodes) => new HtmlElement(this, tag, null, childNodes);

        public HtmlElement CreateElement(string tag, HtmlAttributes attributes, IEnumerable<HtmlNode> childNodes) => new HtmlElement(this, tag, attributes, childNodes);

        public HtmlTextNode CreateTextNode(string text) => new HtmlTextNode(this, text);

        internal void SetRoot(HtmlElement element)
        {
            if (this != element.Document)
            {
                throw new HtmlDomException("Element is not created by current document");
            }
                
            Root = element;
        }

    }
}
