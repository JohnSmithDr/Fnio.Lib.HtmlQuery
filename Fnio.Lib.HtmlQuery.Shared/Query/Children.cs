using Fnio.Lib.HtmlQuery.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fnio.Lib.HtmlQuery
{
    public static partial class HtmlQuery
    {
        public static HtmlElement GetChildById(this HtmlElement e, string id)
        {
            return e.Children()?.Where(s => s.Id == id).FirstOrDefault();
        }

        public static IEnumerable<HtmlElement> GetChildrenByClassName(this HtmlElement e, string className)
        {
            return e.Children()?.Where(c => c.ClassNames.Contains(className));
        }

        public static IEnumerable<HtmlElement> GetChildrenByTagName(this HtmlElement e, string tagName)
        {
            tagName = tagName.ToLowerInvariant();
            return e.Children()?.Where(n => n.TagName == tagName);
        }

        public static IEnumerable<HtmlElement> GetChildren(this HtmlElement e, Func<HtmlElement, bool> predicate)
        {
            return e.Children().Where(predicate);
        }
    }
}
