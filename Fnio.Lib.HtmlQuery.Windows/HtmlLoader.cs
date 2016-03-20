using Fnio.Lib.HtmlQuery.Node;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fnio.Lib.HtmlQuery
{
    public static partial class HtmlLoader
    {
        /// <summary>
        /// Load html text from url.
        /// </summary>
        public static Task<string> LoadHtmlAsync(HttpClient httpClient, Uri uri)
        {
            return LoadHtmlAsync(httpClient, uri, CancellationToken.None);
        }

        /// <summary>
        /// Load html text from url.
        /// </summary>
        public static async Task<string> LoadHtmlAsync(HttpClient httpClient, Uri uri, CancellationToken cancellationToken)
        {
            var response = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseContentRead, cancellationToken);
            using (response)
            {
                string charset = null;

                // try get charset from content-type
                //
                if (response.Content.Headers.ContentType != null)
                {
                    charset = response.Content.Headers.ContentType.CharSet;
                    Debug.WriteLine("Charset from http header content-type: " + charset);
                }

                // try get charset from meta tag
                //
                if (string.IsNullOrEmpty(charset))
                {
                    var html = await response.Content.ReadAsStringAsync();
                    var match = MetaCharsetRegex.Match(html);
                    charset = match.Groups[1].Value.Trim();
                    Debug.WriteLine("Charset from html meta tag: " + charset);
                }

                // or default charset is utf-8
                //
                if (string.IsNullOrEmpty(charset))
                {
                    charset = "utf-8";
                }

                var encoding = Encoding.GetEncoding(charset);
                var data = await response.Content.ReadAsByteArrayAsync();
                var result = encoding.GetString(data, 0, data.Length);
                return result;
            }
        }

        /// <summary>
        /// Load html document from url.
        /// </summary>
        public static Task<HtmlDocument> LoadHtmlDocumentAsync(HttpClient httpClient, Uri uri)
        {
            return LoadHtmlDocumentAsync(httpClient, uri, CancellationToken.None, null);
        }

        /// <summary>
        /// Load html document from url.
        /// </summary>
        public static Task<HtmlDocument> LoadHtmlDocumentAsync(HttpClient httpClient, Uri uri, CancellationToken cancellationToken)
        {
            return LoadHtmlDocumentAsync(httpClient, uri, cancellationToken, null);
        }

        /// <summary>
        /// Load html document from url, with tag filters.
        /// </summary>
        public static Task<HtmlDocument> LoadHtmlDocumentAsync(HttpClient httpClient, Uri uri, IEnumerable<string> tagFilters)
        {
            return LoadHtmlDocumentAsync(httpClient, uri, CancellationToken.None, tagFilters);
        }

        /// <summary>
        /// Load html document from url, with tag filters.
        /// </summary>
        public static async Task<HtmlDocument> LoadHtmlDocumentAsync(HttpClient httpClient, Uri uri, CancellationToken cancellationToken, IEnumerable<string> tagFilters)
        {
            var html = await LoadHtmlAsync(httpClient, uri, cancellationToken);
            var htmlDoc = HtmlParser.Parse(html, tagFilters);
            htmlDoc.Url = uri;
            return htmlDoc;
        }
    }
}
