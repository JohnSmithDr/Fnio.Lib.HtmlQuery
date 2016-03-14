using CenterCLR.Sgml;
using Fnio.Lib.HtmlQuery.Node;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Fnio.Lib.HtmlQuery
{
    /// <summary>
    /// Html parser.
    /// </summary>
    public static class HtmlParser
    {
        /// <summary>
        /// Parse html document from text.
        /// </summary>
        public static HtmlDocument Parse(string html)
        {
            return Parse(html, null);
        }

        /// <summary>
        /// Parse html document from text, filtering the specific tags.
        /// </summary>
        public static HtmlDocument Parse(string html, IEnumerable<string> tagFilter)
        {
            html = TrimHtml(html);

            var xdoc = ParseXmlDocument(html);
            var htmlNode = GetHtmlNode(xdoc);

            var tagFilterSet = tagFilter != null
                ? new HashSet<string>(tagFilter.Select(s => s.ToLowerInvariant()))
                : null;

            if (htmlNode == null) return null;
            var htmlDoc = new HtmlDocument();
            htmlDoc.SetRoot(htmlNode.ToHtmlElement(htmlDoc, tagFilterSet));
            return htmlDoc;
        }

        private static string TrimHtml(string text)
        {
            var htmlEndTags = new string[] { "</html>", "</HTML>" };

            foreach (var tag in htmlEndTags)
            {
                var index = text.IndexOf(tag);

                if (index > 0 && text.Length > index + tag.Length)
                {
                    return text.Substring(0, index + tag.Length);
                }
            }

            return text;
        }

        /// <summary>
        /// Parse text from xml document.
        /// </summary>
        private static XDocument ParseXmlDocument(string html)
        {
            using (var reader = new StringReader(html))
            {
                return SgmlReader.Parse(reader);
            }
        }

        private static XElement GetHtmlNode(this XDocument xdoc)
        {
            return xdoc
                .Descendants()
                .Where(n => "html" == n.Name.LocalName.ToLowerInvariant())
                .FirstOrDefault();
        }

        private static HtmlAttributes GetHtmlAttributes(this XElement x)
        {
            return new HtmlAttributes(
                    x.Attributes().Select(a => new KeyValuePair<string, string>(a.Name.LocalName, a.Value))
                );
        }

        private static HtmlNode ToHtmlNode(this XNode x, HtmlDocument doc, HashSet<string> tagFilter)
        {
            if (x.NodeType == XmlNodeType.Element && x is XElement)
                return ToHtmlElement(x as XElement, doc, tagFilter);

            if (x.NodeType == XmlNodeType.Text && x is XText)
                return ToHtmlTextNode(x as XText, doc);

            return null;
        }

        private static HtmlElement ToHtmlElement(this XElement x, HtmlDocument doc, HashSet<string> tagFilter)
        {
            var tag = x.Name.LocalName.ToLowerInvariant();

            if (tagFilter != null && tagFilter.Contains(tag))
                return null;

            var attrs = x.GetHtmlAttributes();
            var childNodes = x.Nodes().Select(n => n.ToHtmlNode(doc, tagFilter)).Where(n => n != null);
            return doc.CreateElement(tag, attrs, childNodes);
        }

        private static HtmlTextNode ToHtmlTextNode(this XText x, HtmlDocument doc)
        {
            return string.IsNullOrWhiteSpace(x.Value) 
                ? null 
                : doc.CreateTextNode(x.Value);
        }

    }
}
