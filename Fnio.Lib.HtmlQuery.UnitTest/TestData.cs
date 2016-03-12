using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fnio.Lib.HtmlQuery.UnitTest
{
    static class TestData
    {
        public static string SimpleHtmlDoc { get; } =
            @"<!DOCTYPE html>
            <html>
                <head>
                    <meta charset=""utf-8"">
                    <title>Example</title>
                    <link href=""http://www.example.org/foo.js""></link>
                    <script src=""http://www.example.org/bar.js""></script>
                </head>
                <body>
                    <nav id=""navigation"" class=""container navs"">
                        <a href=""/"">Home</a>
                    </nav>
                    <header class=""container"">
                        <img src=""logo.png"" alt=""logo""/>
                        <h1>Example</h1>
                    </header>
                    <div id=""container"" class=""container main section"">
                        <p class=""info"">foo</p>
                        <p class=""info"">bar</p>
                    </div>
                    <footer class=""container""></footer>
                </body>
            </html>";
    }
}
