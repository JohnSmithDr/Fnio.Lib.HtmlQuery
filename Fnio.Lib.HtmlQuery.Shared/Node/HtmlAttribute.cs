using System;
using System.Collections.Generic;
using System.Text;

namespace Fnio.Lib.HtmlQuery.Node
{
    public class HtmlAttribute
    {
        public HtmlAttribute(string name, string value)
        {
            this.Name = name.ToLowerInvariant();
            this.Value = value;
        }

        public string Name { get; private set; }

        public string Value { get; private set; }
    }
}
