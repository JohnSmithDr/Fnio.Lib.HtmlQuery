using Fnio.Lib.HtmlQuery.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fnio.Lib.HtmlQuery
{
    public static partial class HtmlQuery
    {
        public static HtmlElement GetElementById(this HtmlDocument doc, string id)
        {
            return doc.Root?.ToEnumerable().Where(n => n.Id == id).FirstOrDefault();
        }

        public static IEnumerable<HtmlElement> GetElementsByClassName(this HtmlDocument doc, string className)
        {
            return doc.Root?.ToEnumerable().Where(n => n.ClassNames.Contains(className));
        }

        public static IEnumerable<HtmlElement> GetElementsByTagName(this HtmlDocument doc, string tagName)
        {
            tagName = tagName.ToLowerInvariant();
            return doc.Root?.ToEnumerable().Where(n => n.TagName == tagName);
        }

        public static IEnumerable<HtmlElement> GetElements(this HtmlDocument doc, Func<HtmlElement, bool> predicate)
        {
            return doc.Root?.ToEnumerable().Where(predicate);
        }
    }
}
