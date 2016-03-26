using System.Collections.Generic;

namespace Fnio.Lib.HtmlQuery
{
    internal static class Utils
    {
        public static int IndexOf<T>(this IEnumerable<T> source, T item)
        {
            var index = 0;
            foreach (var one in source)
            {
                if (one.Equals(item))
                {
                    return index;
                }
                ++index;
            }
            return -1;
        }
    }
}
