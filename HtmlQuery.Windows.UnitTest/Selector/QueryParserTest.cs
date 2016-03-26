using System;
using FluentAssertions;
using Fnio.Lib.HtmlQuery.Selector;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fnio.Lib.HtmlQuery.Windows.UnitTest
{
    [TestClass]
    public class QueryParserTest
    {
        [TestMethod]
        public void TestParseAllElements()
        {
            var eval = QueryParser.Parse(" * ");
            eval.Should().BeOfType<Evaluator.AllElements>();
            eval.ToString().Should().Be("*");
        }

        [TestMethod]
        public void TestParseId()
        {
            var eval = QueryParser.Parse("#foo");
            eval.Should().BeOfType<Evaluator.Id>();
            eval.ToString().Should().Be("#foo");
        }

        [TestMethod]
        public void TestParseClassName()
        {
            var eval = QueryParser.Parse(".foo");
            eval.Should().BeOfType<Evaluator.Class>();
            eval.ToString().Should().Be(".foo");
        }

        [TestMethod]
        public void TestParseTagName()
        {
            var eval = QueryParser.Parse("foo");
            eval.Should().BeOfType<Evaluator.Tag>();
            eval.ToString().Should().Be("foo");
        }

        [TestMethod]
        public void TestParseAttribute()
        {
            Evaluator eval;
            CombiningEvaluator.And and;

            // [foo]
            //
            eval = QueryParser.Parse("[foo]");
            eval.Should().BeOfType<Evaluator.Attribute>();

            // foo[bar]
            //
            eval = QueryParser.Parse("foo[bar]");
            eval.Should().BeOfType<CombiningEvaluator.And>();
            eval.ToString().Should().Be("foo [bar]");

            and = (CombiningEvaluator.And) eval;
            and.Evaluators.Should().HaveCount(2);

            and.Evaluators[0].Should().BeOfType<Evaluator.Tag>();
            and.Evaluators[1].Should().BeOfType<Evaluator.Attribute>();

            // #foo[bar]
            eval = QueryParser.Parse("#foo[bar]");
            eval.Should().BeOfType<CombiningEvaluator.And>();
            eval.ToString().Should().Be("#foo [bar]");

            and = (CombiningEvaluator.And) eval;
            and.Evaluators.Should().HaveCount(2);

            and.Evaluators[0].Should().BeOfType<Evaluator.Id>();
            and.Evaluators[1].Should().BeOfType<Evaluator.Attribute>();

            // .foo[bar]
            eval = QueryParser.Parse(".foo[bar]");
            eval.Should().BeOfType<CombiningEvaluator.And>();
            eval.ToString().Should().Be(".foo [bar]");

            and = (CombiningEvaluator.And) eval;
            and.Evaluators.Should().HaveCount(2);

            and.Evaluators[0].Should().BeOfType<Evaluator.Class>();
            and.Evaluators[1].Should().BeOfType<Evaluator.Attribute>();
        }

        [TestMethod]
        public void TestParseAttributeStarting()
        {
            Evaluator eval;
            CombiningEvaluator.And and;

            // [foo]
            //
            eval = QueryParser.Parse("[^foo]");
            eval.Should().BeOfType<Evaluator.AttributeStarting>();

            // foo[bar]
            //
            eval = QueryParser.Parse("foo[^bar]");
            eval.Should().BeOfType<CombiningEvaluator.And>();
            eval.ToString().Should().Be("foo [^bar]");

            and = (CombiningEvaluator.And) eval;
            and.Evaluators.Should().HaveCount(2);

            and.Evaluators[0].Should().BeOfType<Evaluator.Tag>();
            and.Evaluators[1].Should().BeOfType<Evaluator.AttributeStarting>();

            // #foo[bar]
            eval = QueryParser.Parse("#foo[^bar]");
            eval.Should().BeOfType<CombiningEvaluator.And>();
            eval.ToString().Should().Be("#foo [^bar]");

            and = (CombiningEvaluator.And) eval;
            and.Evaluators.Should().HaveCount(2);

            and.Evaluators[0].Should().BeOfType<Evaluator.Id>();
            and.Evaluators[1].Should().BeOfType<Evaluator.AttributeStarting>();

            // .foo[bar]
            eval = QueryParser.Parse(".foo[^bar]");
            eval.Should().BeOfType<CombiningEvaluator.And>();
            eval.ToString().Should().Be(".foo [^bar]");

            and = (CombiningEvaluator.And)eval;
            and.Evaluators.Should().HaveCount(2);

            and.Evaluators[0].Should().BeOfType<Evaluator.Class>();
            and.Evaluators[1].Should().BeOfType<Evaluator.AttributeStarting>();
        }

        [TestMethod]
        public void TestParseAttributeWithValue()
        {
            Evaluator eval;
            CombiningEvaluator.And and;

            // [foo=bar]
            //
            eval = QueryParser.Parse("[foo=bar]");
            eval.Should().BeOfType<Evaluator.AttributeWithValue>();
            eval.ToString().Should().Be("[foo=bar]");

            // a[href=#]
            //
            eval = QueryParser.Parse("a[href=#]");
            eval.Should().BeOfType<CombiningEvaluator.And>();
            eval.ToString().Should().Be("a [href=#]");

            and = (CombiningEvaluator.And) eval;
            and.Evaluators.Should().HaveCount(2);

            and.Evaluators[0].Should().BeOfType<Evaluator.Tag>();
            and.Evaluators[1].Should().BeOfType<Evaluator.AttributeWithValue>();
        }

        [TestMethod]
        public void TestParseAttributeWithValueNot()
        {
            Evaluator eval;
            CombiningEvaluator.And and;

            // [foo!=bar]
            //
            eval = QueryParser.Parse("[foo!=bar]");
            eval.Should().BeOfType<Evaluator.AttributeWithValueNot>();
            eval.ToString().Should().Be("[foo!=bar]");

            // a[href!=/]
            //
            eval = QueryParser.Parse("a[href!=/]");
            eval.Should().BeOfType<CombiningEvaluator.And>();
            eval.ToString().Should().Be("a [href!=/]");

            and = (CombiningEvaluator.And) eval;
            and.Evaluators.Should().HaveCount(2);

            and.Evaluators[0].Should().BeOfType<Evaluator.Tag>();
            and.Evaluators[1].Should().BeOfType<Evaluator.AttributeWithValueNot>();
        }

        [TestMethod]
        public void TestParseAttributeWithValueStarting()
        {
            Evaluator eval;
            CombiningEvaluator.And and;

            // [foo^=bar]
            //
            eval = QueryParser.Parse("[foo^=bar]");
            eval.Should().BeOfType<Evaluator.AttributeWithValueStarting>();
            eval.ToString().Should().Be("[foo^=bar]");

            // img[src^=http]
            //
            eval = QueryParser.Parse("img[src^=http]");
            eval.Should().BeOfType<CombiningEvaluator.And>();
            eval.ToString().Should().Be("img [src^=http]");

            and = (CombiningEvaluator.And) eval;
            and.Evaluators.Should().HaveCount(2);

            and.Evaluators[0].Should().BeOfType<Evaluator.Tag>();
            and.Evaluators[1].Should().BeOfType<Evaluator.AttributeWithValueStarting>();
        }

        [TestMethod]
        public void TestParseAttributeWithValueEnding()
        {
            Evaluator eval;
            CombiningEvaluator.And and;

            // [foo$=bar]
            //
            eval = QueryParser.Parse("[foo$=bar]");
            eval.Should().BeOfType<Evaluator.AttributeWithValueEnding>();
            eval.ToString().Should().Be("[foo$=bar]");

            // img[alt$=?]
            // 
            eval = QueryParser.Parse("img[alt$=?]");
            eval.Should().BeOfType<CombiningEvaluator.And>();
            eval.ToString().Should().Be("img [alt$=?]");

            and = (CombiningEvaluator.And) eval;
            and.Evaluators.Should().HaveCount(2);

            and.Evaluators[0].Should().BeOfType<Evaluator.Tag>();
            and.Evaluators[1].Should().BeOfType<Evaluator.AttributeWithValueEnding>();
        }

        [TestMethod]
        public void TestParseAttributeWithValueContaining()
        {
            Evaluator eval;
            CombiningEvaluator.And and;

            // [foo*=bar]
            //
            eval = QueryParser.Parse("[foo*=bar]");
            eval.Should().BeOfType<Evaluator.AttributeWithValueContaining>();
            eval.ToString().Should().Be("[foo*=bar]");

            // div[data*=foo]
            // 
            eval = QueryParser.Parse("div[data*=foo]");
            eval.Should().BeOfType<CombiningEvaluator.And>();
            eval.ToString().Should().Be("div [data*=foo]");

            and = (CombiningEvaluator.And) eval;
            and.Evaluators.Should().HaveCount(2);

            and.Evaluators[0].Should().BeOfType<Evaluator.Tag>();
            and.Evaluators[1].Should().BeOfType<Evaluator.AttributeWithValueContaining>();
        }

        [TestMethod]
        public void TestParseAttributeWithValueMatching()
        {
            Evaluator eval;
            CombiningEvaluator.And and;

            // [foo~=bar]
            //
            eval = QueryParser.Parse("[foo~=bar]");
            eval.Should().BeOfType<Evaluator.AttributeWithValueMatching>();
            eval.ToString().Should().Be("[foo~=bar]");

            // li[id~=foo-\\d+]
            // 
            eval = QueryParser.Parse("li[id~=foo-\\d+]");
            eval.Should().BeOfType<CombiningEvaluator.And>();
            eval.ToString().Should().Be("li [id~=foo-\\d+]");

            and = (CombiningEvaluator.And)eval;
            and.Evaluators.Should().HaveCount(2);

            and.Evaluators[0].Should().BeOfType<Evaluator.Tag>();
            and.Evaluators[1].Should().BeOfType<Evaluator.AttributeWithValueMatching>();
        }

        [TestMethod]
        public void TestParseIndexLessThen()
        {
            Evaluator eval;
            CombiningEvaluator.And and;

            // :lt(1)
            //
            eval = QueryParser.Parse(":lt(1)");
            eval.Should().BeOfType<Evaluator.IndexLessThan>();
            eval.ToString().Should().Be(":lt(1)");

            // li:lt(10)
            // 
            eval = QueryParser.Parse("li:lt(10)");
            eval.Should().BeOfType<CombiningEvaluator.And>();
            eval.ToString().Should().Be("li :lt(10)");

            and = (CombiningEvaluator.And)eval;
            and.Evaluators.Should().HaveCount(2);

            and.Evaluators[0].Should().BeOfType<Evaluator.Tag>();
            and.Evaluators[1].Should().BeOfType<Evaluator.IndexLessThan>();

            // :lt(?)
            Action act = () => QueryParser.Parse(":lt(?)");
            act.ShouldThrow<SelectorParseException>();

            // :lt[#]
            //
            act = () => QueryParser.Parse(":lt[#]");
            act.ShouldThrow<SelectorParseException>();

            // :lt{@}
            //
            act = () => QueryParser.Parse(":lt[@]");
            act.ShouldThrow<SelectorParseException>();
        }

        [TestMethod]
        public void TestParseIndexGreaterThen()
        {
            Evaluator eval;
            CombiningEvaluator.And and;

            // :gt(1)
            //
            eval = QueryParser.Parse(":gt(1)");
            eval.Should().BeOfType<Evaluator.IndexGreaterThan>();
            eval.ToString().Should().Be(":gt(1)");

            // li:gt(10)
            // 
            eval = QueryParser.Parse("li:gt(10)");
            eval.Should().BeOfType<CombiningEvaluator.And>();
            eval.ToString().Should().Be("li :gt(10)");

            and = (CombiningEvaluator.And)eval;
            and.Evaluators.Should().HaveCount(2);

            and.Evaluators[0].Should().BeOfType<Evaluator.Tag>();
            and.Evaluators[1].Should().BeOfType<Evaluator.IndexGreaterThan>();

            // :gt(*)
            //
            Action act = () => QueryParser.Parse(":gt(*)");
            act.ShouldThrow<SelectorParseException>();

            // :gt[~]
            //
            act = () => QueryParser.Parse(":gt[~]");
            act.ShouldThrow<SelectorParseException>();

            // :gt{?}
            //
            act = () => QueryParser.Parse(":gt{?}");
            act.ShouldThrow<SelectorParseException>();
        }

        [TestMethod]
        public void TestParseIndexEquals()
        {
            Evaluator eval;
            CombiningEvaluator.And and;

            // :gt(1)
            //
            eval = QueryParser.Parse(":eq(1)");
            eval.Should().BeOfType<Evaluator.IndexEquals>();
            eval.ToString().Should().Be(":eq(1)");

            // li:eq(10)
            // 
            eval = QueryParser.Parse("li:eq(10)");
            eval.Should().BeOfType<CombiningEvaluator.And>();
            eval.ToString().Should().Be("li :eq(10)");

            and = (CombiningEvaluator.And)eval;
            and.Evaluators.Should().HaveCount(2);

            and.Evaluators[0].Should().BeOfType<Evaluator.Tag>();
            and.Evaluators[1].Should().BeOfType<Evaluator.IndexEquals>();

            // :eq(%)
            //
            Action act = () => QueryParser.Parse(":eq(%)");
            act.ShouldThrow<SelectorParseException>();

            // :eq[$]
            //
            act = () => QueryParser.Parse(":eq[$]");
            act.ShouldThrow<SelectorParseException>();

            // :eq{-}
            //
            act = () => QueryParser.Parse(":eq{-}");
            act.ShouldThrow<SelectorParseException>();
        }

        [TestMethod]
        public void TestParseContainText()
        {
            Evaluator eval;
            CombiningEvaluator.And and;

            // :contains(foo)
            //
            eval = QueryParser.Parse(":contains(foo)");
            eval.Should().BeOfType<Evaluator.ContainsText>();
            eval.ToString().Should().Be(":contains(foo)");

            // p:contains(foo)
            // 
            eval = QueryParser.Parse("p:contains(foo)");
            eval.Should().BeOfType<CombiningEvaluator.And>();
            eval.ToString().Should().Be("p :contains(foo)");

            and = (CombiningEvaluator.And)eval;
            and.Evaluators.Should().HaveCount(2);

            and.Evaluators[0].Should().BeOfType<Evaluator.Tag>();
            and.Evaluators[1].Should().BeOfType<Evaluator.ContainsText>();

            // :contains[foo]
            //
            Action act = () => QueryParser.Parse(":contains[foo]");
            act.ShouldThrow<SelectorParseException>();

            // :contains{foo}
            //
            act = () => QueryParser.Parse(":contains{foo}");
            act.ShouldThrow<SelectorParseException>();
        }

        [TestMethod]
        public void TestParseMatchesRegex()
        {
            Evaluator eval;
            CombiningEvaluator.And and;

            // :matches(foo)
            //
            eval = QueryParser.Parse(":matches(foo)");
            eval.Should().BeOfType<Evaluator.MatchesRegex>();
            eval.ToString().Should().Be(":matches(foo)");

            // p:matches(\\d+)
            // 
            eval = QueryParser.Parse("p:matches(\\d+)");
            eval.Should().BeOfType<CombiningEvaluator.And>();
            eval.ToString().Should().Be("p :matches(\\d+)");

            and = (CombiningEvaluator.And)eval;
            and.Evaluators.Should().HaveCount(2);

            and.Evaluators[0].Should().BeOfType<Evaluator.Tag>();
            and.Evaluators[1].Should().BeOfType<Evaluator.MatchesRegex>();

            // :matches[foo]
            //
            Action act = () => QueryParser.Parse(":matches[foo]");
            act.ShouldThrow<SelectorParseException>();

            // :matches{foo}
            //
            act = () => QueryParser.Parse(":matches{foo}");
            act.ShouldThrow<SelectorParseException>();
        }

        [TestMethod]
        public void TestParsesCombining()
        {
            var eval = QueryParser.Parse("a b, c d, e f");
            eval.Should().BeOfType<CombiningEvaluator.Or>();

            var or = (CombiningEvaluator.Or)eval;
            or.Evaluators.Should().HaveCount(3);

            foreach (var innerEval in or.Evaluators)
            {
                innerEval.Should().BeOfType<CombiningEvaluator.And>();

                var and = (CombiningEvaluator.And)innerEval;
                and.Evaluators.Should().HaveCount(2);
                and.Evaluators[0].Should().BeOfType<Evaluator.Tag>();
                and.Evaluators[1].Should().BeOfType<StructuralEvaluator.Ancestor>();
            }
        }

        [TestMethod]
        public void TestParsesMulti()
        {
            var eval = QueryParser.Parse(".foo > ol, ol li + li");
            eval.Should().BeOfType<CombiningEvaluator.Or>();

            var or = (CombiningEvaluator.Or)eval;
            or.Evaluators.Should().HaveCount(2);

            var andLeft = (CombiningEvaluator.And)or.Evaluators[0];
            andLeft.ToString().Should().Be("ol :parent(.foo)");
            andLeft.Evaluators.Should().HaveCount(2);

            var andRight = (CombiningEvaluator.And)or.Evaluators[1];
            andRight.ToString().Should().Be("li :prev(li :ancestor(ol))");
            andRight.Evaluators.Should().HaveCount(2);
        }

    }
}
