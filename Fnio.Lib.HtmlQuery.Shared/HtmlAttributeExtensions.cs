using System;

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

        private static string CombineUrl(Uri baseUrl, string relativeOrAbsoluteUrl) => 
            new Uri(baseUrl, relativeOrAbsoluteUrl).ToString();
    }
}
