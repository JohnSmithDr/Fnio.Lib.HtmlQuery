using System;
using System.Collections.Generic;
using System.Linq;

namespace Fnio.Lib.HtmlQuery.Node
{
    public static class HtmlAttributeExtensions
    {
        public static string Href(this HtmlElement e)
        {
            var href = e["href"]?.Trim() ?? string.Empty;
            return string.IsNullOrEmpty(href) ? href : CombineUrl(e.Document.Url, href);
        }

        public static string Src(this HtmlElement e)
        {
            var src = e["src"]?.Trim() ?? string.Empty;
            return string.IsNullOrEmpty(src) ? src : CombineUrl(e.Document.Url, src);
        }

        public static bool HasClassName(this HtmlElement e, string className)
        {
            return e.ClassNames.Contains(className);
        }

        public static bool HasClassNames(this HtmlElement e, params string[] classNames)
        {
            var init = e.ClassNames.ToArray();
            var set = new HashSet<string>(init);
            foreach (var n in classNames)
                if (!set.Contains(n)) return false;
            return true;
        }

        public static bool HasAttribute(this HtmlElement e, string attributeName)
        {
            return e.Attributes.GetAttribute(attributeName) == null;
        }

        private static string CombineUrl(Uri baseUrl, string relativeOrAbsoluteUrl)
        {
            return new Uri(baseUrl, relativeOrAbsoluteUrl).ToString();
        }
    }
}
