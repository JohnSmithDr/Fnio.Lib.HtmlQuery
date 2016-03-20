using FluentAssertions;
using Fnio.Lib.HtmlQuery.Selector;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fnio.Lib.HtmlQuery.Windows.UnitTest
{
    [TestClass]
    public class QueryParserTest
    {
        [TestMethod]
        public void TestParseIdSelector()
        {
            var eval = QueryParser.Parse("#foo");
            eval.Should().BeOfType<Evaluator.Id>();
            eval.ToString().Should().Be("#foo");
        }

        [TestMethod]
        public void TestParseClassNameSelector()
        {
            var eval = QueryParser.Parse(".foo");
            eval.Should().BeOfType<Evaluator.Class>();
            eval.ToString().Should().Be(".foo");
        }

        [TestMethod]
        public void TestParseTagNameSelector()
        {
            var eval = QueryParser.Parse("foo");
            eval.Should().BeOfType<Evaluator.Tag>();
            eval.ToString().Should().Be("foo");
        }

        [TestMethod]
        public void TestParseAttributeSelector()
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

            and = (CombiningEvaluator.And)eval;
            and.Evaluators.Should().HaveCount(2);

            and.Evaluators[0].Should().BeOfType<Evaluator.Id>();
            and.Evaluators[1].Should().BeOfType<Evaluator.Attribute>();

            // .foo[bar]
            eval = QueryParser.Parse(".foo[bar]");
            eval.Should().BeOfType<CombiningEvaluator.And>();
            eval.ToString().Should().Be(".foo [bar]");

            and = (CombiningEvaluator.And)eval;
            and.Evaluators.Should().HaveCount(2);

            and.Evaluators[0].Should().BeOfType<Evaluator.Class>();
            and.Evaluators[1].Should().BeOfType<Evaluator.Attribute>();
        }

        [TestMethod]
        public void TestParseAttributeStartingSelector()
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

            and = (CombiningEvaluator.And)eval;
            and.Evaluators.Should().HaveCount(2);

            and.Evaluators[0].Should().BeOfType<Evaluator.Tag>();
            and.Evaluators[1].Should().BeOfType<Evaluator.AttributeStarting>();

            // #foo[bar]
            eval = QueryParser.Parse("#foo[^bar]");
            eval.Should().BeOfType<CombiningEvaluator.And>();
            eval.ToString().Should().Be("#foo [^bar]");

            and = (CombiningEvaluator.And)eval;
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
        public void TestParseAttributeWithValueSelector()
        {
            var eval = QueryParser.Parse("a[href=http]");
            eval.Should().BeOfType<CombiningEvaluator.And>();
            eval.ToString().Should().Be("a [href=http]");

            var and = (CombiningEvaluator.And) eval;
            and.Evaluators.Should().HaveCount(2);

            and.Evaluators[0].Should().BeOfType<Evaluator.Tag>();
            and.Evaluators[1].Should().BeOfType<Evaluator.AttributeWithValue>();
        }

        [TestMethod]
        public void TestParsesOr()
        {
            var eval = QueryParser.Parse("a b, c d, e f");
            eval.Should().BeOfType<CombiningEvaluator.Or>();

            var or = (CombiningEvaluator.Or) eval;
            or.Evaluators.Should().HaveCount(3);

            foreach (var innerEval in or.Evaluators)
            {
                innerEval.Should().BeOfType<CombiningEvaluator.And>();

                var and = (CombiningEvaluator.And) innerEval;
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

            var or = (CombiningEvaluator.Or) eval;
            or.Evaluators.Should().HaveCount(2);

            var andLeft = (CombiningEvaluator.And) or.Evaluators[0];
            andLeft.ToString().Should().Be("ol :parent(.foo)");
            andLeft.Evaluators.Should().HaveCount(2);

            var andRight = (CombiningEvaluator.And) or.Evaluators[1];
            andRight.ToString().Should().Be("li :prev(li :ancestor(ol))");
            andRight.Evaluators.Should().HaveCount(2);
        }

        
    }
}
