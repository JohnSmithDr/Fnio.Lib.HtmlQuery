using Fnio.Lib.HtmlQuery.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fnio.Lib.HtmlQuery
{
    public static partial class HtmlQuery
    {
        internal static IEnumerable<HtmlElement> ToEnumerable(this HtmlElement e)
        {
            var self = new HtmlElement[] { e };
            return self.Concat(e.Descendants());
        }

        public static bool HasClassName(this HtmlElement e, string className)
        {
            throw new NotImplementedException();
        }

        public static bool ContainsClassNames(this HtmlElement e, IEnumerable<string> classNames)
        {
            throw new NotImplementedException();
        }

        public static bool ContainsClassNames(this HtmlElement e, params string[] classNames)
        {
            throw new NotImplementedException();
        }
    }
}
