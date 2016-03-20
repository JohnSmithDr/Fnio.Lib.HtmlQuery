using Fnio.Lib.HtmlQuery.Node;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Web.Http;

namespace Fnio.Lib.HtmlQuery
{
    public static partial class HtmlLoader
    {
        public static IAsyncOperationWithProgress<string, HttpProgress> LoadHtmlAsync(HttpClient httpClient, Uri uri)
        {
            return AsyncInfo.Run<string, HttpProgress>(async (c, p) =>
            {
                var response = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseContentRead).AsTask(c, p);
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
                        charset = match?.Groups[1].Value.Trim();
                        Debug.WriteLine("Charset from html meta tag: " + charset);
                    }

                    // or default charset is utf-8
                    //
                    if (string.IsNullOrEmpty(charset))
                    {
                        charset = "utf-8";
                    }

                    var encoding = CodePagesEncodingProvider.Instance.GetEncoding(charset) ?? Encoding.UTF8;
                    var buff = await response.Content.ReadAsBufferAsync();
                    var data = buff.ToArray();
                    var result = encoding.GetString(data, 0, data.Length);
                    return result;
                }
            });
        }

        public static IAsyncOperationWithProgress<HtmlDocument, HttpProgress> LoadHtmlDocumentAsync(HttpClient httpClient, Uri uri)
        {
            return LoadHtmlDocumentAsync(httpClient, uri, null);
        }

        public static IAsyncOperationWithProgress<HtmlDocument, HttpProgress> LoadHtmlDocumentAsync(HttpClient httpClient, Uri uri, IEnumerable<string> tagFilters)
        {
            return AsyncInfo.Run<HtmlDocument, HttpProgress>(async (c, p) =>
            {
                var html = await LoadHtmlAsync(httpClient, uri).AsTask(c, p);
                var htmlDoc = HtmlParser.Parse(html, tagFilters);
                htmlDoc.Url = uri;
                return htmlDoc;
            });
        }
    }
}
