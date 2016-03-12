using Fnio.Lib.HtmlQuery.Node;
using System.Collections.Generic;
using System.Linq;

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
            return e.ClassNames.Contains(className);
        }

        public static bool ContainsClassNames(this HtmlElement e, IEnumerable<string> classNames)
        {
            var classNameSet = new HashSet<string>(e.ClassNames);
            return classNames.All(name => classNameSet.Contains(name));
        }

        public static bool ContainsClassNames(this HtmlElement e, params string[] classNames)
        {
            var classNameSet = new HashSet<string>(e.ClassNames);
            return classNames.All(name => classNameSet.Contains(name));
        }
    }
}
