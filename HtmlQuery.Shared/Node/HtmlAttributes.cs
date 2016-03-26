using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
        private readonly Dictionary<string, HtmlAttribute> _attrDict = new Dictionary<string, HtmlAttribute>();

        /// <summary>
        /// Get attribute value by attribute name, returns null if not there.
        /// </summary>
        public string this[string attributeName]
        {
            get { return GetAttribute(attributeName)?.Value; }
            set { SetAttribute(new HtmlAttribute(attributeName, value)); }
        }

        /// <summary>
        /// Get the number of attributes in current collection.
        /// </summary>
        public int Count => _attrDict.Count;

        /// <summary>
        /// Constructor.
        /// </summary>
        public HtmlAttributes()
        {

        }

        /// <summary>
        /// Constructor with initial values.
        /// </summary>
        public HtmlAttributes(IEnumerable<KeyValuePair<string, string>> attrs)
        {
            foreach (var attr in attrs)
            {
                SetAttribute(attr.Key, attr.Value);
            }
        }

        /// <summary>
        /// Get a attribute by specific name.
        /// </summary>
        public HtmlAttribute GetAttribute(string name)
        {
            var key = name.ToLowerInvariant();
            HtmlAttribute value;

            _attrDict.TryGetValue(key, out value);
            return value;
        }

        /// <summary>
        /// Add or replace the attribute in current collection.
        /// </summary>
        public void SetAttribute(HtmlAttribute attribute)
        {
            string key = attribute.Name.ToLowerInvariant();
            _attrDict[key] = attribute;
        }

        /// <summary>
        /// Add or replace the attribute in current collection.
        /// </summary>
        public void SetAttribute(string name, string value)
        {
            SetAttribute(new HtmlAttribute(name, value));
        }

        public IEnumerator<HtmlAttribute> GetEnumerator()
        {
            return _attrDict.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _attrDict.Values.GetEnumerator();
        }

        public override string ToString()
            => string.Join(" ", _attrDict.Keys.Select(key => GetAttribute(key).ToString()));
    }
}
