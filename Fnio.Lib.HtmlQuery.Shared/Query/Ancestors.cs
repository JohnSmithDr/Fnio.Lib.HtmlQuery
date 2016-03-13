using Fnio.Lib.HtmlQuery.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fnio.Lib.HtmlQuery
{
    public static partial class HtmlQuery
    {
        public static IEnumerable<HtmlElement> GetAncestors(this HtmlNode n)
        {
            return GetAncestorsEnumerable(n).Reverse();
        }

        public static IEnumerable<HtmlElement> GetAncestors(this HtmlNode n, Func<HtmlElement, bool> predicate)
        {
            return n.GetAncestors().Where(predicate);
        }

        public static HtmlElement GetAncestorById(this HtmlNode n, string id)
        {
            return n.GetAncestors().Where(s => s.Id == id).FirstOrDefault();
        }

        public static IEnumerable<HtmlElement> GetAncestorsByClassName(this HtmlNode n, string className)
        {
            return n.GetAncestors().Where(c => c.ClassNames.Contains(className));
        }

        public static IEnumerable<HtmlElement> GetAncestorsByTagName(this HtmlNode n, string tagName)
        {
            tagName = tagName.ToLowerInvariant();
            return n.GetAncestors().Where(s => s.TagName == tagName);
        }

        private static IEnumerable<HtmlElement> GetAncestorsEnumerable(HtmlNode n)
        {
            var parent = n.Parent as HtmlElement;
            while(parent != null)
            {
                yield return parent;
                parent = parent.Parent as HtmlElement;
            }
        }
    }
}
