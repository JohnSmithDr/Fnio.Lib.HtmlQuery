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
        /// Get a element node with specific id.
        /// </summary>
        public static HtmlElement GetElementById(this HtmlDocument doc, string id)
        {
            return doc.Root?.AsTraversable().Where(n => n.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Get element nodes with specific class name.
        /// </summary>
        public static IEnumerable<HtmlElement> GetElementsByClassName(this HtmlDocument doc, string className)
        {
            return doc.Root?.AsTraversable().Where(n => n.ClassNames.Contains(className));
        }

        /// <summary>
        /// Get elements node with specific tag name.
        /// </summary>
        public static IEnumerable<HtmlElement> GetElementsByTagName(this HtmlDocument doc, string tagName)
        {
            tagName = tagName.ToLowerInvariant();
            return doc.Root?.AsTraversable().Where(n => n.TagName == tagName);
        }

        /// <summary>
        /// Get elements base on a predicate.
        /// </summary>
        public static IEnumerable<HtmlElement> GetElements(this HtmlDocument doc, Func<HtmlElement, bool> predicate)
        {
            return doc.Root?.AsTraversable().Where(predicate);
        }
    }
}
