namespace Fnio.Lib.HtmlQuery.Node
{
    /// <summary>
    /// Html text node.
    /// </summary>
    public class HtmlTextNode : HtmlNode
    {
        /// <summary>
        /// Get the text value of current node.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="doc">the document that the node belongs to</param>
        /// <param name="text">text of the node</param>
        internal HtmlTextNode(HtmlDocument doc, string text)
        {
            Document = doc;
            Text = text;
        }
    }
}
