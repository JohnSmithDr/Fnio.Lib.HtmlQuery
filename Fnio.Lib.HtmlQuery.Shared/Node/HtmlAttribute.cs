using System;
using System.Collections.Generic;
using System.Text;

namespace Fnio.Lib.HtmlQuery.Node
{
    /// <summary>
    /// Html attribute.
    /// </summary>
    public class HtmlAttribute
    {
        public HtmlAttribute(string name, string value)
        {
            this.Name = name.ToLowerInvariant();
            this.Value = value;
        }

        /// <summary>
        /// Get attribute name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Get attribute value.
        /// </summary>
        public string Value { get; private set; }
    }
}
