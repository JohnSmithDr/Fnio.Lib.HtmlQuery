using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Fnio.Lib.HtmlQuery.Node;

namespace Fnio.Lib.HtmlQuery.Selector
{
    /// <summary>
    /// Evaluates that an element matches the selector.
    /// </summary>
    public abstract class Evaluator
    {
        /// <summary>
        /// Test wheter the element meets the evaluator's requirements.
        /// </summary>
        /// <param name="root">Root of the matching subtree</param>
        /// <param name="element">tested element</param>
        /// <returns></returns>
        public abstract bool Matches(HtmlElement root, HtmlElement element);

        /// <summary>
        /// Evaluator for tag name.
        /// </summary>
        public sealed class Tag : Evaluator
        {
            private string TagName { get; }

            public Tag(string tagName)
            {
                TagName = tagName;
            }

            public override bool Matches(HtmlElement root, HtmlElement element) => TagName.Equals(element.TagName);

            public override string ToString() => TagName;
        }

        /// <summary>
        /// Evaluator for element id.
        /// </summary>
        public sealed class Id : Evaluator
        {
            private string ElementId { get; }

            public Id(string id)
            {
                ElementId = id;
            }

            public override bool Matches(HtmlElement root, HtmlElement element) => ElementId.Equals(element.Id);

            public override string ToString() => ElementId;

        }

        /// <summary>
        /// Evaluator for element class.
        /// </summary>
        public sealed class Class : Evaluator
        {
            private string ClassName { get; }

            public Class(string className)
            {
                ClassName = className;
            }

            public override bool Matches(HtmlElement root, HtmlElement element) => element.HasClassName(ClassName);

            public override string ToString() => $".{ClassName}";

        }

        /// <summary>
        /// Evaluator for attribute name matching.
        /// </summary>
        public sealed class Attribute : Evaluator
        {
            private string AttributeName { get; }

            public Attribute(string key)
            {
                AttributeName = key;
            }

            public override bool Matches(HtmlElement root, HtmlElement element) 
                => element.HasAttribute(AttributeName);

            public override string ToString() => $"[{AttributeName}]";

        }

        /// <summary>
        /// Evaluator for attribute name prefix matching.
        /// </summary>
        public sealed class AttributeStarting : Evaluator
        {
            private string AttributePrefix { get; }

            public AttributeStarting(string attrPrefix)
            {
                AttributePrefix = attrPrefix;
            }

            public override bool Matches(HtmlElement root, HtmlElement element) 
                => element.Attributes.Any(a => a.Name.StartsWith(AttributePrefix));

            public override string ToString() => $"[^{AttributePrefix}]";

        }

        /// <summary>
        /// Evaluator for attribute name value matching.
        /// </summary>
        public sealed class AttributeWithValue : AttributeKeyPair
        {
            public AttributeWithValue(string key, string value) : base(key, value) { }

            public override bool Matches(HtmlElement root, HtmlElement element)
                => element.HasAttribute(Key) && Value.Equals(element[Key], StringComparison.CurrentCultureIgnoreCase);

            public override string ToString() => $"[{Key}={Value}]";

        }

        /// <summary>
        /// Evaluator for attribute name != value matching.
        /// </summary>
        public sealed class AttributeWithValueNot : AttributeKeyPair
        {
            public AttributeWithValueNot(string key, string value) : base(key, value) { }

            public override bool Matches(HtmlElement root, HtmlElement element)
                => !Value.Equals(element[Key], StringComparison.CurrentCultureIgnoreCase);

            public override string ToString() => $"[{Key}!={Value}]";

        }

        /// <summary>
        /// Evaluator for attribute name value matching (value prefix).
        /// </summary>
        public sealed class AttributeWithValueStarting : AttributeKeyPair
        {
            public AttributeWithValueStarting(string key, string value) : base(key, value) { }

            public override bool Matches(HtmlElement root, HtmlElement element)
                => element.HasAttribute(Key) && element[Key].ToLowerInvariant().StartsWith(Value);

            public override string ToString() => $"[{Key}^={Value}]";

        }

        /// <summary>
        /// Evaluator for attribute name value matching (value ending).
        /// </summary>
        public sealed class AttributeWithValueEnding : AttributeKeyPair
        {
            public AttributeWithValueEnding(string key, string value) : base(key, value) { }

            public override bool Matches(HtmlElement root, HtmlElement element)
                => element.HasAttribute(Key) && element[Key].ToLowerInvariant().EndsWith(Value);

            public override string ToString() => $"[{Key}$={Value}]";

        }

        /// <summary>
        /// Evaluator for attribute name value matching (value containing).
        /// </summary>
        public sealed class AttributeWithValueContaining : AttributeKeyPair
        {
            public AttributeWithValueContaining(string key, string value) : base(key, value) { }

            public override bool Matches(HtmlElement root, HtmlElement element)
                => element.HasAttribute(Key) && element[Key].ToLowerInvariant().Contains(Value);

            public override string ToString() => $"[{Key}*={Value}]";

        }

        /// <summary>
        /// Evaluator for attribute name value matching (value regex matching).
        /// </summary>
        public sealed class AttributeWithValueMatching : Evaluator
        {
            private string Key { get; }
            private Regex Pattern { get; }

            public AttributeWithValueMatching(string key, Regex pattern)
            {
                Key = key.Trim().ToLowerInvariant();
                Pattern = pattern;
            }

            public override bool Matches(HtmlElement root, HtmlElement element)
                => element.HasAttribute(Key) && Pattern.IsMatch(element[Key]);

            public override string ToString() => $"[{Key}~={Pattern}]";
        }

        /// <summary>
        /// Abstract evaluator for attribute name value matching.
        /// </summary>
        public abstract class AttributeKeyPair : Evaluator
        {
            protected string Key { get; } 
            protected string Value { get; }

            protected AttributeKeyPair(string key, string value)
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException("Key cannot be empty.", nameof(key));
                }

                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Value cannot be empty.", nameof(value));
                }

                Key = key.Trim().ToLowerInvariant();
                Value = value.Trim().ToLowerInvariant();
            }
        }

        /// <summary>
        /// Evaluator for any/all element matching.
        /// </summary>
        public sealed class AllElements : Evaluator
        {
            public override bool Matches(HtmlElement root, HtmlElement element) => true;

            public override string ToString() => "*";
        }

        /// <summary>
        /// Evaluator for matching by index number (e &lt; index).
        /// </summary>
        public sealed class IndexLessThan : IndexEvaluator
        {
            public IndexLessThan(int index) : base(index) { }

            public override bool Matches(HtmlElement root, HtmlElement element)
            {
                var siblingIndex = (element.Parent as HtmlElement)?.ChildNodes.IndexOf(element) ?? -1;
                return siblingIndex >= 0 && siblingIndex < Index;
            }

            public override string ToString() => $":lt({Index})";

        }

        /// <summary>
        /// Evaluator for matching by index number (e &gt; index).
        /// </summary>
        public sealed class IndexGreaterThan : IndexEvaluator
        {
            public IndexGreaterThan(int index) : base(index) { }

            public override bool Matches(HtmlElement root, HtmlElement element)
            {
                var siblingIndex = (element.Parent as HtmlElement)?.ChildNodes.IndexOf(element) ?? -1;
                return siblingIndex > Index;
            }

            public override string ToString() => $":gt({Index})";

        }

        /// <summary>
        /// Evaluator for matching by index number (e = idx)
        /// </summary>
        public sealed class IndexEquals : IndexEvaluator
        {
            public IndexEquals(int index) : base(index) { }

            public override bool Matches(HtmlElement root, HtmlElement element)
            {
                var siblingIndex = (element.Parent as HtmlElement)?.ChildNodes.IndexOf(element) ?? -1;
                return siblingIndex == Index;
            }

            public override string ToString() => $":eq({Index})";
        }

        /// <summary>
        /// Abstract evaluator for index matching.
        /// </summary>
        public abstract class IndexEvaluator : Evaluator
        {
            protected int Index { get; }

            protected IndexEvaluator(int index)
            {
                Index = index;
            }
        }

        /// <summary>
        /// Evaluator for matching Element (and its descendants) text.
        /// </summary>
        public sealed class ContainsText : Evaluator
        {
            private string SearchText { get; }

            public ContainsText(string searchText)
            {
                SearchText = searchText.ToLowerInvariant();
            }

            public override bool Matches(HtmlElement root, HtmlElement element)
                => element.Text().ToLowerInvariant().Contains(SearchText);

            public override string ToString() => $":contains({SearchText})";

        }

        /// <summary>
        /// Evaluator for matching Element (and its descendants) text with regex.
        /// </summary>
        public sealed class MatchesRegex : Evaluator
        {
            private Regex Pattern { get; }

            public MatchesRegex(Regex pattern)
            {
                Pattern = pattern;
            }

            public override bool Matches(HtmlElement root, HtmlElement element) => Pattern.IsMatch(element.Text());

            public override string ToString() => $":matches({Pattern})";

        }
        
    }

    /// <summary>
    /// Base combining (and, or) evaluator.
    /// </summary>
    public abstract class CombiningEvaluator : Evaluator
    {
        public List<Evaluator> Evaluators { get; }

        private CombiningEvaluator()
        {
            Evaluators = new List<Evaluator>();
        }

        private CombiningEvaluator(IEnumerable<Evaluator> evaluators) : this()
        {
            Evaluators.AddRange(evaluators);
        }

        public Evaluator RightMostEvaluator() => Evaluators.LastOrDefault();

        public void ReplaceRightMostEvaluator(Evaluator replacement)
        {
            Evaluators[Evaluators.Count - 1] = replacement;
        }

        public sealed class And : CombiningEvaluator
        {
            public And(IEnumerable<Evaluator> evaluators) : base(evaluators) { }

            public And(params Evaluator[] evaluators) : base(evaluators) { }

            public override bool Matches(HtmlElement root, HtmlElement node) => Evaluators.All(eval => eval.Matches(root, node));

            public override string ToString() => string.Join(" ", Evaluators.Select(s => s.ToString()));

        }

        public sealed class Or : CombiningEvaluator
        {
            public Or(IEnumerable<Evaluator> evaluators)
            {
                var evaluatorArr = evaluators as Evaluator[] ?? evaluators.ToArray();
                if (evaluatorArr.Length > 1)
                {
                    Evaluators.Add(new And(evaluatorArr));
                }
                else
                {
                    Evaluators.AddRange(evaluatorArr);
                }
            }

            public Or() { }

            public void Add(Evaluator e)
            {
                Evaluators.Add(e);
            }

            public override bool Matches(HtmlElement root, HtmlElement node) => Evaluators.Any(e => e.Matches(root, node));

            public override string ToString() => $":or{Evaluators}";
        }
    }

    /// <summary>
    /// Base structural evaluator.
    /// </summary>
    public abstract class StructuralEvaluator : Evaluator
    {
        protected Evaluator InnerEvaluator { get; }

        protected StructuralEvaluator(Evaluator evaluator)
        {
            InnerEvaluator = evaluator;
        }

        public class Root : Evaluator
        {
            public override bool Matches(HtmlElement root, HtmlElement element) => root == element;
        }

        public class Has : StructuralEvaluator
        {
            public Has(Evaluator evaluator) : base(evaluator) { }

            public override bool Matches(HtmlElement root, HtmlElement element) 
                => element.Descendants(e => e != element && InnerEvaluator.Matches(root, e)).Any();

            public override string ToString() => $":has({InnerEvaluator})";

        }

        public class Not : StructuralEvaluator
        {
            public Not(Evaluator evaluator) : base(evaluator) { }

            public override bool Matches(HtmlElement root, HtmlElement node) => !InnerEvaluator.Matches(root, node);

            public override string ToString() => $":not({InnerEvaluator})";

        }

        public class Parent : StructuralEvaluator
        {
            public Parent(Evaluator evaluator) : base(evaluator) { }

            public override bool Matches(HtmlElement root, HtmlElement element)
            {
                if (root == element)
                {
                    return false;
                }

                var parent = element.Parent as HtmlElement;
                return parent != null && InnerEvaluator.Matches(root, parent);
            }

            public override string ToString() => $":parent{InnerEvaluator}";
        }

        public class Ancestor : StructuralEvaluator
        {
            public Ancestor(Evaluator evaluator) : base(evaluator) { }

            public override bool Matches(HtmlElement root, HtmlElement element)
                => root != element && element.Ancestors().Reverse().Any(e => InnerEvaluator.Matches(root, e));

            public override string ToString() => $":ancestor{InnerEvaluator}";
        }

        public class Previous : StructuralEvaluator
        {
            public Previous(Evaluator evaluator) : base(evaluator) { }

            public override bool Matches(HtmlElement root, HtmlElement element)
            {
                if (root == element)
                {
                    return false;
                }

                var prev = element.ElementsBeforeSelf().LastOrDefault();

                return prev != null && InnerEvaluator.Matches(root, prev);
            }

            public override string ToString() => $":prev{InnerEvaluator}";

        }

        public class Before : StructuralEvaluator
        {
            public Before(Evaluator evaluator) : base(evaluator) { }

            public override bool Matches(HtmlElement root, HtmlElement element)
                => root != element && element.ElementsBeforeSelf().Reverse().Any(e => InnerEvaluator.Matches(root, e));

            public override string ToString() => $":prev*{InnerEvaluator}";

        }

    }

}
