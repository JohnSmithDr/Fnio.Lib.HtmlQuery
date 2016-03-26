using Fnio.Lib.HtmlQuery.Node;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fnio.Lib.HtmlQuery
{
    public static partial class HtmlQuery
    {
        /// <summary>
        /// Get all child elements in child nodes.
        /// </summary>
        public static IEnumerable<HtmlElement> Children(this HtmlElement element)
            => element.ChildNodes.OfType<HtmlElement>();

        /// <summary>
        /// Get all child elements based on a predicate.
        /// </summary>
        public static IEnumerable<HtmlElement> Children(this HtmlElement element, Func<HtmlElement, bool> predicate) 
            => Children(element).Where(predicate);

        /// <summary>
        /// Try to get a element node with specific id from child nodes.
        /// </summary>
        public static HtmlElement ChildById(this HtmlElement element, string id) 
            => Children(element).FirstOrDefault(s => s.Id == id);

        /// <summary>
        /// Get child elements with specific class name from child nodes.
        /// </summary>
        public static IEnumerable<HtmlElement> ChildrenByClassName(this HtmlElement element, string className) 
            => Children(element)?.Where(c => c.ClassNames.Contains(className));

        /// <summary>
        /// Get child elements with specific tag name from child nodes.
        /// </summary>
        public static IEnumerable<HtmlElement> ChildrenByTagName(this HtmlElement element, string tagName)
        {
            tagName = tagName.ToLowerInvariant();
            return Children(element)?.Where(n => n.TagName == tagName);
        }
        
    }
}
