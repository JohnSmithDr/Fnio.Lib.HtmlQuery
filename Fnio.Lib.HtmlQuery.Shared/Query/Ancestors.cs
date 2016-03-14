using Fnio.Lib.HtmlQuery.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fnio.Lib.HtmlQuery
{
    public static partial class HtmlQuery
    {
        /// <summary>
        /// Get ancestor elements.
        /// </summary>
        public static IEnumerable<HtmlElement> Ancestors(this HtmlNode node)
        {
            return GetAncestorsEnumerable(node).Reverse();
        }

        /// <summary>
        /// Get ancestor elements based on a predicate.
        /// </summary>
        public static IEnumerable<HtmlElement> Ancestors(this HtmlNode node, Func<HtmlElement, bool> predicate)
        {
            return node.Ancestors().Where(predicate);
        }

        /// <summary>
        /// Try to get an ancestor element with specific id.
        /// </summary>
        public static HtmlElement AncestorById(this HtmlNode node, string id)
        {
            return node.Ancestors().Where(s => s.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Get ancestor elements by specific class name.
        /// </summary>
        public static IEnumerable<HtmlElement> AncestorsByClassName(this HtmlNode node, string className)
        {
            return node.Ancestors().Where(c => c.ClassNames.Contains(className));
        }

        /// <summary>
        /// Get ancestor elements by specific tag name.
        /// </summary>
        public static IEnumerable<HtmlElement> AncestorsByTagName(this HtmlNode node, string tagName)
        {
            tagName = tagName.ToLowerInvariant();
            return node.Ancestors().Where(s => s.TagName == tagName);
        }

        private static IEnumerable<HtmlElement> GetAncestorsEnumerable(HtmlNode node)
        {
            var parent = node.Parent as HtmlElement;
            while(parent != null)
            {
                yield return parent;
                parent = parent.Parent as HtmlElement;
            }
        }
    }
}
