using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Fnio.Lib.HtmlQuery.Node
{
    public class HtmlAttributes : IEnumerable<HtmlAttribute>
    {
        private Dictionary<string, HtmlAttribute> _attrDict = new Dictionary<string, HtmlAttribute>();

        public string this[string key]
        {
            get { return this.GetAttribute(key)?.Value; }
            set { this.SetAttribute(new HtmlAttribute(key, value)); }
        }

        public int Count
        {
            get { return this._attrDict.Count; }
        }

        public HtmlAttributes()
        {

        }

        public HtmlAttributes(IEnumerable<KeyValuePair<string, string>> attrs)
        {
            foreach (var attr in attrs)
            {
                this.SetAttribute(attr.Key, attr.Value);
            }
        }

        public HtmlAttribute GetAttribute(string name)
        {
            string key = name.ToLowerInvariant();
            HtmlAttribute value = null;

            this._attrDict.TryGetValue(key, out value);
            return value;
        }

        public void SetAttribute(HtmlAttribute attribute)
        {
            string key = attribute.Name.ToLowerInvariant();
            this._attrDict[key] = attribute;
        }

        public void SetAttribute(string name, string value)
        {
            this.SetAttribute(new HtmlAttribute(name, value));
        }

        public IEnumerator<HtmlAttribute> GetEnumerator()
        {
            return this._attrDict.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._attrDict.Values.GetEnumerator();
        }
    }
}
