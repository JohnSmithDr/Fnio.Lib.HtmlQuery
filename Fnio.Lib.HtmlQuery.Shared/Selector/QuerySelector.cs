using Fnio.Lib.HtmlQuery.Node;
using Fnio.Lib.HtmlQuery.Selector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fnio.Lib.HtmlQuery
{
    public static class QuerySelectorExtensions
    {
        /// <summary>
        /// Query element by css selector.
        /// </summary>
        public static IEnumerable<HtmlElement> QuerySelector(this HtmlElement root, string selector)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            selector = selector.Trim();
            if (string.IsNullOrWhiteSpace(selector))
            {
                throw new ArgumentException("cannot be null or blank", nameof(selector));
            }

            var evaluator = QueryParser.Parse(selector);
            return root.AsTraversable().Where(e => evaluator.Matches(root, e));
        }

        /// <summary>
        /// Query element by css selector.
        /// </summary>
        public static IEnumerable<HtmlElement> QuerySelector(this HtmlDocument htmlDoc, string selector)
        {
            if (htmlDoc == null)
            {
                throw new ArgumentNullException(nameof(htmlDoc));
            }

            return QuerySelector(htmlDoc.Root, selector);
        }
    }
}
