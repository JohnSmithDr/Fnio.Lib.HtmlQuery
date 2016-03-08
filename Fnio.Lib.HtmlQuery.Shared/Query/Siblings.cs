using Fnio.Lib.HtmlQuery.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fnio.Lib.HtmlQuery
{
    public static partial class HtmlQuery
    {
        public static IEnumerable<HtmlNode> Siblings(this HtmlNode n)
        {
            return (n.Parent as HtmlElement)?.Children().Where(c => c != n);
        }

        public static IEnumerable<HtmlNode> GetSiblings(this HtmlNode n, Func<HtmlNode, bool> predicate)
        {
            return n.Siblings().Where(predicate);
        }

        public static HtmlElement GetSiblingById(this HtmlNode n, string id)
        {
            return n.Siblings()?.OfType<HtmlElement>().Where(c => c.Id == id).FirstOrDefault();
        }

        public static IEnumerable<HtmlElement> GetSiblingsByClassName(this HtmlNode n, string className)
        {
            return n.Siblings()?.OfType<HtmlElement>().Where(c => c.ClassNames.Contains(className));
        }

        public static IEnumerable<HtmlElement> GetSiblingsByTagName(this HtmlNode n, string tagName)
        {
            tagName = tagName.ToLowerInvariant();
            return n.Siblings()?.OfType<HtmlElement>().Where(c => c.TagName == tagName);
        }

        public static IEnumerable<HtmlNode> GetNodesBeforeSelf()
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<HtmlNode> GetNodesAfterSelf()
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<HtmlNode> GetElementsBeforeSelf()
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<HtmlNode> GetElementsAfterSelf()
        {
            throw new NotImplementedException();
        }
    }
}
