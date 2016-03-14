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
        /// Get all descendant elements.
        /// </summary>
        public static IEnumerable<HtmlElement> Descendants(this HtmlElement element)
        {
            return element.Children().SelectMany(s =>
            {
                return new HtmlElement[] { s }.Concat(s.Descendants());
            });
        }

        /// <summary>
        /// Get all descendant elements based on a predicate.
        /// </summary>
        public static IEnumerable<HtmlElement> Descendants(this HtmlElement element, Func<HtmlElement, bool> predicate)
        {
            return element.Descendants().Where(predicate);
        }

        /// <summary>
        /// Try to get a descendant element node with specific id from descendant nodes.
        /// </summary>
        public static HtmlElement DescendantById(this HtmlElement element, string id)
        {
            return element.Descendants()?.Where(s => s.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Get descendant elements with specific class name.
        /// </summary>
        public static IEnumerable<HtmlElement> DescendantsByClassName(this HtmlElement element, string className)
        {
            return element.Descendants()?.Where(c => c.ClassNames.Contains(className));
        }

        /// <summary>
        /// Get descendant elements with specific tag name.
        /// </summary>
        public static IEnumerable<HtmlElement> DescendantsByTagName(this HtmlElement element, string tagName)
        {
            tagName = tagName.ToLowerInvariant();
            return element.Descendants()?.Where(n => n.TagName == tagName);
        }
    }
}
