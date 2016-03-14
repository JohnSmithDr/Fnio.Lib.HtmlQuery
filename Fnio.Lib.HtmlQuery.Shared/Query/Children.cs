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
        /// Get all child elements in child nodes.
        /// </summary>
        public static IEnumerable<HtmlElement> Children(this HtmlElement element)
        {
            return element.ChildNodes.OfType<HtmlElement>();
        }

        /// <summary>
        /// Get all child elements based on a predicate.
        /// </summary>
        public static IEnumerable<HtmlElement> Children(this HtmlElement element, Func<HtmlElement, bool> predicate)
        {
            return element.Children().Where(predicate);
        }

        /// <summary>
        /// Try to get a element node with specific id from child nodes.
        /// </summary>
        public static HtmlElement ChildById(this HtmlElement element, string id)
        {
            return element.Children()?.Where(s => s.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Get child elements with specific class name from child nodes.
        /// </summary>
        public static IEnumerable<HtmlElement> ChildrenByClassName(this HtmlElement element, string className)
        {
            return element.Children()?.Where(c => c.ClassNames.Contains(className));
        }

        /// <summary>
        /// Get child elements with specific tag name from child nodes.
        /// </summary>
        public static IEnumerable<HtmlElement> ChildrenByTagName(this HtmlElement element, string tagName)
        {
            tagName = tagName.ToLowerInvariant();
            return element.Children()?.Where(n => n.TagName == tagName);
        }

        
    }
}
