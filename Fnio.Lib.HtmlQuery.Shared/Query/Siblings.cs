using Fnio.Lib.HtmlQuery.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fnio.Lib.HtmlQuery
{
    public static partial class HtmlQuery
    {
        public static IEnumerable<HtmlNode> GetSiblings(this HtmlNode n)
        {
            return (n.Parent as HtmlElement)?.Children().Where(c => c != n);
        }

        public static IEnumerable<HtmlNode> GetSiblings(this HtmlNode n, Func<HtmlNode, bool> predicate)
        {
            return n.GetSiblings().Where(predicate);
        }

        public static HtmlElement GetSiblingById(this HtmlNode n, string id)
        {
            return n.GetSiblings()?.OfType<HtmlElement>().Where(c => c.Id == id).FirstOrDefault();
        }

        public static IEnumerable<HtmlElement> GetSiblingsByClassName(this HtmlNode n, string className)
        {
            return n.GetSiblings()?.OfType<HtmlElement>().Where(c => c.ClassNames.Contains(className));
        }

        public static IEnumerable<HtmlElement> GetSiblingsByTagName(this HtmlNode n, string tagName)
        {
            tagName = tagName.ToLowerInvariant();
            return n.GetSiblings()?.OfType<HtmlElement>().Where(c => c.TagName == tagName);
        }

        public static IEnumerable<HtmlNode> GetNodesBeforeSelf(this HtmlNode n)
        {
            var nodes = (n.Parent as HtmlElement)?.ChildNodes;

            if (nodes == null || nodes.Any() == false) {
                return Enumerable.Empty<HtmlNode>();
            }

            var index = nodes.ToList().IndexOf(n);

            if (index < 0)
            {
                return Enumerable.Empty<HtmlNode>();
            }

            return nodes.Take(index);
        }

        public static IEnumerable<HtmlNode> GetNodesBeforeSelf(this HtmlNode n, Func<HtmlNode, bool> predicate)
        {
            return n.GetNodesBeforeSelf().Where(predicate);
        }

        public static IEnumerable<HtmlNode> GetNodesAfterSelf(this HtmlNode n)
        {
            var nodes = (n.Parent as HtmlElement)?.ChildNodes;

            if (nodes == null || nodes.Any() == false)
            {
                return Enumerable.Empty<HtmlNode>();
            }

            var index = nodes.ToList().IndexOf(n);

            if (index < 0)
            {
                return Enumerable.Empty<HtmlNode>();
            }

            return nodes.Skip(index + 1);
        }

        public static IEnumerable<HtmlNode> GetNodesAfterSelf(this HtmlNode n, Func<HtmlNode, bool> predicate)
        {
            return n.GetNodesAfterSelf().Where(predicate);
        }

        public static IEnumerable<HtmlElement> GetElementsBeforeSelf(this HtmlNode n)
        {
            return n.GetNodesBeforeSelf().OfType<HtmlElement>();
        }

        public static IEnumerable<HtmlElement> GetElementsBeforeSelf(this HtmlNode n, Func<HtmlElement, bool> predicate)
        {
            return n.GetNodesBeforeSelf().OfType<HtmlElement>().Where(predicate);
        }

        public static IEnumerable<HtmlElement> GetElementsAfterSelf(this HtmlNode n)
        {
            return n.GetNodesAfterSelf().OfType<HtmlElement>();

        }

        public static IEnumerable<HtmlElement> GetElementsAfterSelf(this HtmlNode n, Func<HtmlElement, bool> predicate)
        {
            return n.GetElementsAfterSelf().OfType<HtmlElement>().Where(predicate);
        }

        public static HtmlNode GetPreviousSibling(this HtmlNode n)
        {
            return n.GetNodesBeforeSelf().LastOrDefault();
        }

        public static HtmlNode GetNextSibling(this HtmlNode n)
        {
            return n.GetNodesAfterSelf().FirstOrDefault();
        }

    }
}
