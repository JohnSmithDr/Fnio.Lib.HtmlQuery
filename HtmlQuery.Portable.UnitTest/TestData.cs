﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fnio.Lib.HtmlQuery.Portable.UnitTest
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
                        <a id=""home-link"" href=""/"">Home</a>
                        <ul>
                            <li><a href=""/dashboard"">Dashboard</a></li>
                            <li><a href=""/settings"">Settings</a></li>
                            <li><a href=""/profile"">Profile</a></li>
                            <li><a href=""/help"">Help</a></li>
                        </ul>
                    </nav>
                    <header class=""container"">
                        <img id=""logo"" src=""logo.png"" alt=""logo""/>
                        <h1 class=""header"">Example</h1>
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
