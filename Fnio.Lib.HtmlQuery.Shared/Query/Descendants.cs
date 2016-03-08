using Fnio.Lib.HtmlQuery.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fnio.Lib.HtmlQuery
{
    public static partial class HtmlQuery
    {
        public static HtmlElement GetDescendantById(this HtmlElement e, string id)
        {
            return e.Descendants()?.Where(s => s.Id == id).FirstOrDefault();
        }

        public static IEnumerable<HtmlElement> GetDescendantsByClassName(this HtmlElement e, string className)
        {
            return e.Descendants()?.Where(c => c.ClassNames.Contains(className));
        }

        public static IEnumerable<HtmlElement> GetDescendantsByTagName(this HtmlElement e, string tagName)
        {
            tagName = tagName.ToLowerInvariant();
            return e.Descendants()?.Where(n => n.TagName == tagName);
        }

        public static IEnumerable<HtmlElement> GetDescendants(this HtmlElement e, Func<HtmlElement, bool> predicate)
        {
            return e.Children().Where(predicate);
        }
    }
}
