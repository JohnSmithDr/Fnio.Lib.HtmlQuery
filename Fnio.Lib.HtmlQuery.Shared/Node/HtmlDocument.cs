using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fnio.Lib.HtmlQuery.Node
{
    public class HtmlDocument
    {
        public Uri Url { get; internal set; }

        public HtmlElement Root { get; private set; }

        public HtmlElement Head
        {
            get { return this.Root?.Children().FirstOrDefault(c => c.TagName == "head"); }
        }

        public HtmlElement Body
        {
            get { return this.Root?.Children().FirstOrDefault(c => c.TagName == "body"); }
        }

        public string Title
        {
            get { return this.Head?.Children().FirstOrDefault(c => c.TagName == "title")?.Text()?.Trim(); }
        }

        public HtmlDocument() { }

        public HtmlElement CreateElement(string tag, HtmlAttributes attributes, IEnumerable<HtmlNode> childNodes)
        {
            return new HtmlElement(this, tag, attributes, childNodes);
        }

        public HtmlTextNode CreateTextNode(string text)
        {
            return new HtmlTextNode(this, text);
        }

        internal void SetRoot(HtmlElement element)
        {
            if (this != element.Document)
                throw new HtmlDomException("Element is not created by current document");
            this.Root = element;
        }

    }
}
