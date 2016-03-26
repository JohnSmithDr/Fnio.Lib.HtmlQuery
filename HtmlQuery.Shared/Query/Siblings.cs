using Fnio.Lib.HtmlQuery.Node;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fnio.Lib.HtmlQuery
{
    public static partial class HtmlQuery
    {
        /// <summary>
        /// Get all sibling nodes of current node.
        /// </summary>
        public static IEnumerable<HtmlNode> Siblings(this HtmlNode node)
        {
            return (node.Parent as HtmlElement)?.ChildNodes.Where(c => c != node);
        }

        /// <summary>
        /// Get all sibling nodes based on a predicate.
        /// </summary>
        public static IEnumerable<HtmlNode> Siblings(this HtmlNode node, Func<HtmlNode, bool> predicate)
        {
            return node.Siblings().Where(predicate);
        }

        /// <summary>
        /// Try to get a sibling element node with specific id.
        /// </summary>
        public static HtmlElement SiblingById(this HtmlNode node, string id)
        {
            return node.Siblings()?.OfType<HtmlElement>().Where(c => c.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Get sibling element nodes with specific class name.
        /// </summary>
        public static IEnumerable<HtmlElement> SiblingsByClassName(this HtmlNode node, string className)
        {
            return node.Siblings()?.OfType<HtmlElement>().Where(c => c.ClassNames.Contains(className));
        }

        /// <summary>
        /// Get sibling element nodes with specific tag name.
        /// </summary>
        public static IEnumerable<HtmlElement> SiblingsByTagName(this HtmlNode n, string tagName)
        {
            tagName = tagName.ToLowerInvariant();
            return n.Siblings()?.OfType<HtmlElement>().Where(c => c.TagName == tagName);
        }

        /// <summary>
        /// Get sibling nodes before current node.
        /// </summary>
        public static IEnumerable<HtmlNode> NodesBeforeSelf(this HtmlNode node)
        {
            var nodes = (node.Parent as HtmlElement)?.ChildNodes;

            if (nodes == null || nodes.Any() == false) {
                return Enumerable.Empty<HtmlNode>();
            }

            var index = nodes.ToList().IndexOf(node);

            if (index < 0)
            {
                return Enumerable.Empty<HtmlNode>();
            }

            return nodes.Take(index);
        }

        /// <summary>
        /// Get sibling nodes before current node based on a predicate.
        /// </summary>
        public static IEnumerable<HtmlNode> NodesBeforeSelf(this HtmlNode node, Func<HtmlNode, bool> predicate)
        {
            return node.NodesBeforeSelf().Where(predicate);
        }

        /// <summary>
        /// Get sibling nodes after current node.
        /// </summary>
        public static IEnumerable<HtmlNode> NodesAfterSelf(this HtmlNode node)
        {
            var nodes = (node.Parent as HtmlElement)?.ChildNodes;

            if (nodes == null || nodes.Any() == false)
            {
                return Enumerable.Empty<HtmlNode>();
            }

            var index = nodes.ToList().IndexOf(node);

            if (index < 0)
            {
                return Enumerable.Empty<HtmlNode>();
            }

            return nodes.Skip(index + 1);
        }

        /// <summary>
        /// Get sibling nodes after current node based on a predicate.
        /// </summary>
        public static IEnumerable<HtmlNode> NodesAfterSelf(this HtmlNode node, Func<HtmlNode, bool> predicate)
        {
            return node.NodesAfterSelf().Where(predicate);
        }

        /// <summary>
        /// Get sibling element nodes before current node.
        /// </summary>
        public static IEnumerable<HtmlElement> ElementsBeforeSelf(this HtmlNode node)
        {
            return node.NodesBeforeSelf().OfType<HtmlElement>();
        }

        /// <summary>
        /// Get sibling element nodes before current node based on a predicate.
        /// </summary>
        public static IEnumerable<HtmlElement> ElementsBeforeSelf(this HtmlNode node, Func<HtmlElement, bool> predicate)
        {
            return node.NodesBeforeSelf().OfType<HtmlElement>().Where(predicate);
        }

        /// <summary>
        /// Get sibling element nodes after current node.
        /// </summary>
        public static IEnumerable<HtmlElement> ElementsAfterSelf(this HtmlNode node)
        {
            return node.NodesAfterSelf().OfType<HtmlElement>();

        }

        /// <summary>
        /// Get sibling element nodes after current node based on a predicate.
        /// </summary>
        public static IEnumerable<HtmlElement> ElementsAfterSelf(this HtmlNode node, Func<HtmlElement, bool> predicate)
        {
            return node.ElementsAfterSelf().OfType<HtmlElement>().Where(predicate);
        }

        /// <summary>
        /// Get previous sibling node of current node.
        /// </summary>
        public static HtmlNode PreviousSibling(this HtmlNode node)
        {
            return node.NodesBeforeSelf().LastOrDefault();
        }

        /// <summary>
        /// Get next sibling node of current node.
        /// </summary>
        public static HtmlNode NextSibling(this HtmlNode node)
        {
            return node.NodesAfterSelf().FirstOrDefault();
        }

    }
}
