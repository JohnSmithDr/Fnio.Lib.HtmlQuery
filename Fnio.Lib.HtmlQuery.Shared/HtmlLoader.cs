using System.Text.RegularExpressions;

namespace Fnio.Lib.HtmlQuery
{
    /// <summary>
    /// Load html document from web.
    /// </summary>
    public static partial class HtmlLoader
    {
        private static readonly Regex MetaCharsetRegex =
            new Regex("<meta(?!\\s*(?:name|value)\\s*=)[^>]*?charset\\s*=[\\s\"']*([^\\s\"'/>]*)");
    }
}
