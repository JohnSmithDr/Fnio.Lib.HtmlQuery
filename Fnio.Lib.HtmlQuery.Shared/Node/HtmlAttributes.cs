using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Fnio.Lib.HtmlQuery.Node
{
    /// <summary>
    /// Html attribute collection.
    /// </summary>
    public class HtmlAttributes : IEnumerable<HtmlAttribute>
    {
        /// <summary>
        /// Dictionary that host all attributes.
        /// </summary>
        private Dictionary<string, HtmlAttribute> _attrDict = new Dictionary<string, HtmlAttribute>();

        /// <summary>
        /// Get attribute value by attribute name, returns null if not there.
        /// </summary>
        public string this[string attributeName]
        {
            get { return this.GetAttribute(attributeName)?.Value; }
            set { this.SetAttribute(new HtmlAttribute(attributeName, value)); }
        }

        /// <summary>
        /// Get the number of attributes in current collection.
        /// </summary>
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

        /// <summary>
        /// Get a attribute by specific name.
        /// </summary>
        public HtmlAttribute GetAttribute(string name)
        {
            string key = name.ToLowerInvariant();
            HtmlAttribute value = null;

            this._attrDict.TryGetValue(key, out value);
            return value;
        }

        /// <summary>
        /// Add or replace the attribute in current collection.
        /// </summary>
        public void SetAttribute(HtmlAttribute attribute)
        {
            string key = attribute.Name.ToLowerInvariant();
            this._attrDict[key] = attribute;
        }

        /// <summary>
        /// Add or replace the attribute in current collection.
        /// </summary>
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
