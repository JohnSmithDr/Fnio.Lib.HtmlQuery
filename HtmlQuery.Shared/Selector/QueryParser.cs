using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Fnio.Lib.HtmlQuery.Parser;

namespace Fnio.Lib.HtmlQuery.Selector
{
    /// <summary>
    /// Parses a CSS selector into an Evaluator tree.
    /// </summary>
    public class QueryParser
    {
        private static readonly string[] Combinators = { ",", ">", "+", "~", " " };

        private readonly TokenQueue _tq;
        private readonly string _query;
        private readonly List<Evaluator> _evals = new List<Evaluator>();

        /// <summary>
        /// Create a new QueryParser.
        /// </summary>
        /// <param name="query">css query selector</param>
        private QueryParser(string query)
        {
            _query = query.Trim();
            _tq = new TokenQueue(_query);
        }

        /// <summary>
        /// Parse a css query selector into an Evaluator.
        /// </summary>
        /// <param name="query">css query selector</param>
        public static Evaluator Parse(string query)
        {
            var p = new QueryParser(query);
            return p.Parse();
        }

        /// <summary>
        /// Parse the query.
        /// </summary>
        /// <returns>Evaluator</returns>
        private Evaluator Parse()
        {
            _tq.ConsumeWhitespace();

            if (_tq.MatchesAny(Combinators))
            { 
                // if starts with a combinator, use root as elements
                //
                _evals.Add(new StructuralEvaluator.Root());
                Combinator(_tq.Consume());
            }
            else
            {
                FindElements();
            }

            while (!_tq.IsEmpty)
            {
                // hierarchy and extras
                //
                bool seenWhite = _tq.ConsumeWhitespace();

                if (_tq.MatchesAny(Combinators))
                {
                    // group or
                    //
                    Combinator(_tq.Consume());
                }
                else if (seenWhite)
                {
                    Combinator(' ');
                }
                else
                { 
                    // E.class, E#id, E[attr] etc. AND
                    //
                    FindElements(); // take next el, #. etc off queue
                }
            }

            if (_evals.Count == 1)
            {
                return _evals[0];
            }

            return new CombiningEvaluator.And(_evals);
        }

        private void Combinator(char combinator)
        {
            _tq.ConsumeWhitespace();

            var subQuery = ConsumeSubQuery(); // support multi > childs

            Evaluator rootEval; // the new topmost evaluator
            Evaluator currentEval; // the evaluator the new eval will be combined to. could be root, or rightmost or.
            Evaluator newEval = Parse(subQuery); // the evaluator to add into target evaluator
            var replaceRightMost = false;

            if (_evals.Count == 1)
            {
                rootEval = currentEval = _evals[0];

                // make sure OR (,) has precedence:
                //
                if (rootEval is CombiningEvaluator.Or && combinator != ',')
                {
                    currentEval = ((CombiningEvaluator.Or)currentEval).RightMostEvaluator();
                    replaceRightMost = true;
                }
            }
            else
            {
                rootEval = currentEval = new CombiningEvaluator.And(_evals);
            }
            _evals.Clear();

            var f = Parse(subQuery);

            // for most combinators: change the current eval into an AND of the current eval and the new eval
            //
            if (combinator == '>')
            {
                currentEval = new CombiningEvaluator.And(newEval, new StructuralEvaluator.Parent(currentEval));
            }
            else if (combinator == ' ')
            {
                currentEval = new CombiningEvaluator.And(newEval, new StructuralEvaluator.Ancestor(currentEval));
            }
            else if (combinator == '+')
            {
                currentEval = new CombiningEvaluator.And(newEval, new StructuralEvaluator.Previous(currentEval));
            }
            else if (combinator == '~')
            {
                currentEval = new CombiningEvaluator.And(newEval, new StructuralEvaluator.Before(currentEval));
            }
            else if (combinator == ',')
            { 
                // group or
                //
                CombiningEvaluator.Or or;
                if (currentEval is CombiningEvaluator.Or)
                {
                    or = (CombiningEvaluator.Or)currentEval;
                    or.Add(newEval);
                }
                else
                {
                    or = new CombiningEvaluator.Or();
                    or.Add(currentEval);
                    or.Add(newEval);
                }
                currentEval = or;
            }
            else
            {
                throw new SelectorParseException($"Unknown combinator: {combinator}");
            }

            if (replaceRightMost)
            {
                ((CombiningEvaluator.Or)rootEval).ReplaceRightMostEvaluator(currentEval);
            }
            else
            {
                rootEval = currentEval;
            }
            _evals.Add(rootEval);
        }

        private string ConsumeSubQuery()
        {
            var sq = new StringBuilder();
            while (!_tq.IsEmpty)
            {
                if (_tq.Matches("("))
                {
                    sq.Append("(").Append(_tq.ChompBalanced('(', ')')).Append(")");
                }
                else if (_tq.Matches("["))
                {
                    sq.Append("[").Append(_tq.ChompBalanced('[', ']')).Append("]");
                }
                else if (_tq.MatchesAny(Combinators))
                {
                    break;
                }
                else
                {
                    sq.Append(_tq.Consume());
                }
            }
            return sq.ToString();
        }

        private void FindElements()
        {
            if (_tq.MatchChomp("#"))
            {
                ById();
            }
            else if (_tq.MatchChomp("."))
            {
                ByClass();
            }
            else if (_tq.MatchesWord())
            {
                ByTag();
            }
            else if (_tq.Matches("["))
            {
                ByAttribute();
            }
            else if (_tq.MatchChomp("*"))
            {
                AllElements();
            }
            else if (_tq.MatchChomp(":lt("))
            {
                IndexLessThan();
            }
            else if (_tq.MatchChomp(":gt("))
            {
                IndexGreaterThan();
            }
            else if (_tq.MatchChomp(":eq("))
            {
                IndexEquals();
            }
            else if (_tq.Matches(":has("))
            {
                Has();
            }
            else if (_tq.Matches(":contains("))
            {
                Contains();
            }
            else if (_tq.Matches(":matches("))
            {
                Matches();
            }
            else if (_tq.Matches(":not("))
            {
                Not();
            }
            else // unhandled
            {
                var token = _tq.ConsumeRemained();
                throw new SelectorParseException($"Could not parse query \"{_query}\": unexpected token at \"{token}\"");
            }
        }

        private int ConsumeIndex()
        {
            var indexStr = _tq.ChompTo(")").Trim();

            int index;
            if (!int.TryParse(indexStr, out index))
            {
                throw new SelectorParseException("Index must be numeric");
            }

            return index;
        }

        /// <summary>
        /// Id selector #id
        /// </summary>
        private void ById()
        {
            string id = _tq.ConsumeCssIdentifier();

            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("id is empty.");
            }

            _evals.Add(new Evaluator.Id(id));
        }

        /// <summary>
        /// Class name selector .class
        /// </summary>
        private void ByClass()
        {
            string className = _tq.ConsumeCssIdentifier();

            if (string.IsNullOrEmpty(className))
            {
                throw new Exception("className is empty.");
            }

            _evals.Add(new Evaluator.Class(className.Trim().ToLowerInvariant()));
        }

        /// <summary>
        /// Tag name selector
        /// </summary>
        private void ByTag()
        {
            string tagName = _tq.ConsumeCssElementSelector();

            if (string.IsNullOrEmpty(tagName))
            {
                throw new Exception("tagName is empty.");
            }

            // namespaces: if element name is "abc:def", selector must be "abc|def", so flip:
            //
            if (tagName.Contains("|"))
            {
                tagName = tagName.Replace("|", ":");
            }

            _evals.Add(new Evaluator.Tag(tagName.Trim().ToLowerInvariant()));
        }

        /// <summary>
        /// Attribute selector
        /// </summary>
        private void ByAttribute()
        {
            var cq = new TokenQueue(_tq.ChompBalanced('[', ']')); // content queue
            var key = cq.ConsumeToAny("=", "!=", "^=", "$=", "*=", "~="); // eq, not, start, end, contain, match, (no val)

            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("attribute key is empty.");
            }

            cq.ConsumeWhitespace();

            if (cq.IsEmpty)
            {
                if (key.StartsWith("^"))
                {
                    _evals.Add(new Evaluator.AttributeStarting(key.Substring(1)));
                }
                else
                {
                    _evals.Add(new Evaluator.Attribute(key));
                }
            }
            else
            {
                if (cq.MatchChomp("="))
                {
                    _evals.Add(new Evaluator.AttributeWithValue(key, cq.ConsumeRemained()));
                }
                else if (cq.MatchChomp("!="))
                {
                    _evals.Add(new Evaluator.AttributeWithValueNot(key, cq.ConsumeRemained()));
                }
                else if (cq.MatchChomp("^="))
                {
                    _evals.Add(new Evaluator.AttributeWithValueStarting(key, cq.ConsumeRemained()));
                }
                else if (cq.MatchChomp("$="))
                {
                    _evals.Add(new Evaluator.AttributeWithValueEnding(key, cq.ConsumeRemained()));
                }
                else if (cq.MatchChomp("*="))
                {
                    _evals.Add(new Evaluator.AttributeWithValueContaining(key, cq.ConsumeRemained()));
                }
                else if (cq.MatchChomp("~="))
                {
                    _evals.Add(new Evaluator.AttributeWithValueMatching(key, new Regex(cq.ConsumeRemained())));
                }
                else
                {
                    var token = cq.ConsumeRemained();
                    throw new SelectorParseException($"Could not parse attribute query \"{_query}\": unexpected token at \"{token}\"");
                }
            }
        }

        /// <summary>
        /// All element selector *
        /// </summary>
        private void AllElements()
        {
            _evals.Add(new Evaluator.AllElements());
        }

        /// <summary>
        /// Pseudo selectors :lt(index)
        /// </summary>
        private void IndexLessThan()
        {
            _evals.Add(new Evaluator.IndexLessThan(ConsumeIndex()));
        }

        /// <summary>
        /// Pseudo selectors :gt(index)
        /// </summary>
        private void IndexGreaterThan()
        {
            _evals.Add(new Evaluator.IndexGreaterThan(ConsumeIndex()));
        }

        /// <summary>
        /// Pseudo selectors :eq(index)
        /// </summary>
        private void IndexEquals()
        {
            _evals.Add(new Evaluator.IndexEquals(ConsumeIndex()));
        }

        /// <summary>
        /// Pseudo selector :contains(text)
        /// </summary>
        private void Contains()
        {
            _tq.Consume(":contains");

            var searchText = TokenQueue.Unescape(_tq.ChompBalanced('(', ')'));

            if (string.IsNullOrEmpty(searchText))
            {
                throw new Exception(":contains(text) query must not be empty");
            }

            _evals.Add(new Evaluator.ContainsText(searchText));
        }

        /// <summary>
        /// Pseudo selector :matches(regex)
        /// </summary>
        private void Matches()
        {
            _tq.Consume(":matches");

            var regex = _tq.ChompBalanced('(', ')'); // don't unescape, as regex bits will be escaped

            if (string.IsNullOrEmpty(regex))
            {
                throw new Exception(":matches(regex) query must not be empty");
            }

            _evals.Add(new Evaluator.MatchesRegex(new Regex(regex)));
        }

        /// <summary>
        /// Pseudo selector :not(selector)
        /// </summary>
        private void Not()
        {
            _tq.Consume(":not");

            string subQuery = _tq.ChompBalanced('(', ')');

            if (string.IsNullOrEmpty(subQuery))
            {
                throw new Exception(":not(selector) subselect must not be empty");
            }

            _evals.Add(new StructuralEvaluator.Not(Parse(subQuery)));
        }

        /// <summary>
        /// Pseudo selector :has(element)
        /// </summary>
        private void Has()
        {
            _tq.Consume(":has");

            var subQuery = _tq.ChompBalanced('(', ')');

            if (string.IsNullOrEmpty(subQuery))
            {
                throw new Exception(":has(element) subselect must not be empty");
            }

            _evals.Add(new StructuralEvaluator.Has(Parse(subQuery)));
        }

    }
}