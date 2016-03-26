using Fnio.Lib.HtmlQuery.Node;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fnio.Lib.HtmlQuery
{
    public static partial class HtmlQuery
    {
        /// <summary>
        /// Get all descendant elements.
        /// </summary>
        public static IEnumerable<HtmlElement> Descendants(this HtmlElement element) 
            => element.Children().SelectMany(s => new[] { s }.Concat(s.Descendants()));

        /// <summary>
        /// Get all descendant elements based on a predicate.
        /// </summary>
        public static IEnumerable<HtmlElement> Descendants(this HtmlElement element, Func<HtmlElement, bool> predicate) 
            => Descendants(element).Where(predicate);

        /// <summary>
        /// Try to get a descendant element node with specific id from descendant nodes.
        /// </summary>
        public static HtmlElement DescendantById(this HtmlElement element, string id) 
            => Descendants(element).FirstOrDefault(s => s.Id == id);

        /// <summary>
        /// Get descendant elements with specific class name.
        /// </summary>
        public static IEnumerable<HtmlElement> DescendantsByClassName(this HtmlElement element, string className) 
            => Descendants(element)?.Where(c => c.ClassNames.Contains(className));

        /// <summary>
        /// Get descendant elements with specific tag name.
        /// </summary>
        public static IEnumerable<HtmlElement> DescendantsByTagName(this HtmlElement element, string tagName)
        {
            tagName = tagName.ToLowerInvariant();
            return Descendants(element)?.Where(n => n.TagName == tagName);
        }

    }
}
