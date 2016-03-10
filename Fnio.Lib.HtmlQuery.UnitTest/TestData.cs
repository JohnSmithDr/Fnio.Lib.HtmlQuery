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
                    <nav>
                        <a href=""/"">Home</a>
                    </nav>
                    <header>
                        <img src=""logo.png"" alt=""logo""/>
                        <h1>Example</h1>
                    </header>
                    <div>
                        <p>foo</p>
                        <p>bar</p>
                    </div>
                    <footer></footer>
                </body>
            </html>";
    }
}
