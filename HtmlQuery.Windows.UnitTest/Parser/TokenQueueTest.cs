using FluentAssertions;
using Fnio.Lib.HtmlQuery.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fnio.Lib.HtmlQuery.Windows.UnitTest
{
    [TestClass]
    public class TokenQueueTest
    {
        [TestMethod]
        public void TestIsEmpty()
        {
            var tq = new TokenQueue("");
            tq.IsEmpty.Should().BeTrue();

            tq = new TokenQueue("foo");
            tq.IsEmpty.Should().BeFalse();
            tq.ConsumeRemained();
            tq.IsEmpty.Should().BeTrue();
        }

        [TestMethod]
        public void TestRemainingLength()
        {
            var tq = new TokenQueue("");
            tq.RemainingLength.Should().Be(0);

            tq = new TokenQueue("foo");
            tq.RemainingLength.Should().Be(3);
            tq.ConsumeRemained();
            tq.RemainingLength.Should().Be(0);
        }

        [TestMethod]
        public void TestCurrentChar()
        {
            var tq = new TokenQueue("");
            tq.CurrentChar.Should().Be((char) 0);
            tq.NextChar.Should().Be((char) 0);

            tq = new TokenQueue("foo");
            tq.CurrentChar.Should().Be('f');
            tq.NextChar.Should().Be('o');
            tq.Consume().Should().Be('f');

            tq.CurrentChar.Should().Be('o');
            tq.NextChar.Should().Be('o');
            tq.Consume().Should().Be('o');

            tq.CurrentChar.Should().Be('o');
            tq.NextChar.Should().Be((char) 0);
            tq.Consume().Should().Be('o');

            tq.CurrentChar.Should().Be((char) 0);
            tq.NextChar.Should().Be((char) 0);
        }

        [TestMethod]
        public void TestAddFirst()
        {
            var tq = new TokenQueue("One Two");
            tq.ConsumeWord();
            tq.AddFirst("Three");
            tq.ConsumeRemained().Should().Be("Three Two");
        }

        [TestMethod]
        public void TestChompBalanced()
        {
            var tq = new TokenQueue(":contains(one (two) three) four");
            var pre = tq.ConsumeTo("(");
            var guts = tq.ChompBalanced('(', ')');
            var remained = tq.ConsumeRemained();

            pre.Should().Be(":contains");
            guts.Should().Be("one (two) three");
            remained.Should().Be(" four");
        }

        [TestMethod]
        public void TestChompEscapedBalanced()
        {
            var tq = new TokenQueue(":contains(one (two) \\( \\) \\) three) four");
            var pre = tq.ConsumeTo("(");
            var guts = tq.ChompBalanced('(', ')');
            var unescaped = TokenQueue.Unescape(guts);
            var remained = tq.ConsumeRemained();

            pre.Should().Be(":contains");
            guts.Should().Be("one (two) \\( \\) \\) three");
            unescaped.Should().Be("one (two) ( ) ) three");
            remained.Should().Be(" four");
        }

        [TestMethod]
        public void TestChompToIgnoreCase()
        {
            var tq = new TokenQueue("<textarea>one < two </TEXTarea>");
            tq.ChompToIgnoreCase("</textarea").Should().Be("<textarea>one < two ");

            tq = new TokenQueue("<textarea> one two < three </oops>");
            tq.ChompToIgnoreCase("</textarea").Should().Be("<textarea> one two < three </oops>");
        }

        [TestMethod]
        public void TestChompBalancedMatchesAsMuchAsPossible()
        {
            var tq = new TokenQueue("unbalanced(something(or another");
            tq.ConsumeTo("(");
            tq.ChompBalanced('(', ')').Should().Be("something(or another");
        }

        [TestMethod]
        public void TestUnescape()
        {
            TokenQueue.Unescape("one \\( \\) \\\\").Should().Be("one ( ) \\");
        }
        
    }
}
