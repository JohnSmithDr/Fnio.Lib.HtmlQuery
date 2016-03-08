using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fnio.Lib.HtmlQuery.Node;
using FluentAssertions;
using System.Collections.Generic;

namespace Fnio.Lib.HtmlQuery.UnitTest.Node
{
    [TestClass]
    public class HtmlAttributesTest
    {
        [TestMethod]
        public void TestCount()
        {
            var attrs = new HtmlAttributes();
            attrs.Count.Should().Be(0);

            var dict = new Dictionary<string, string>()
            {
                { "id", "foo" },
                { "CLASS", "bar" }
            };
            attrs = new HtmlAttributes(dict);
            attrs.Count.Should().Be(2);
        }

        [TestMethod]
        public void TestIndexer()
        {
            var dict = new Dictionary<string, string>()
            {
                { "id", "foo" },
                { "CLASS", "bar" }
            };
            var attrs = new HtmlAttributes(dict);
            attrs["id"].Should().Be("foo");
            attrs["ID"].Should().Be("foo");
            attrs["class"].Should().Be("bar");
            attrs["CLASS"].Should().Be("bar");
        }

        [TestMethod]
        public void TestGetAttribute()
        {
            var dict = new Dictionary<string, string>()
            {
                { "id", "foo" },
                { "CLASS", "bar" }
            };
            var attrs = new HtmlAttributes(dict);

            var idAttr = attrs.GetAttribute("Id");
            idAttr.Name.Should().Be("id");
            idAttr.Value.Should().Be("foo");

            var classAttr = attrs.GetAttribute("Class");
            classAttr.Name.Should().Be("class");
            classAttr.Value.Should().Be("bar");
        }

        [TestMethod]
        public void TestSetAttribute()
        {
            var attrs = new HtmlAttributes();

            attrs.SetAttribute(new HtmlAttribute("id", "foo"));
            attrs["ID"].Should().Be("foo");

            attrs.SetAttribute(new HtmlAttribute("ID", "bar"));
            attrs["id"].Should().Be("bar");

            attrs.SetAttribute("class", "foo");
            attrs["CLASS"].Should().Be("foo");

            attrs.SetAttribute("CLASS", "bar");
            attrs["class"].Should().Be("bar");
        }

        [TestMethod]
        public void GetEnumerator()
        {
            var dict = new Dictionary<string, string>()
            {
                { "id", "foo" },
                { "CLASS", "bar" }
            };
            var attrs = new HtmlAttributes(dict);

            foreach (var attr in attrs)
            {
                if (attr.Name == "id") attr.Value.Should().Be("foo");
                if (attr.Name == "class") attr.Value.Should().Be("bar");
            }
        }

    }
}
