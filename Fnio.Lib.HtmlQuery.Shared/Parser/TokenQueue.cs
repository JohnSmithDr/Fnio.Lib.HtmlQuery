using System;
using System.Linq;
using System.Text;

namespace Fnio.Lib.HtmlQuery.Parser
{
    /// <summary>
    /// A character queue for parsing tokens.
    /// </summary>
    public class TokenQueue
    {
        private const char Esc = '\\'; // escape char for chomp balanced.

        private string _queue;
        private int _position;

        /// <summary>
        /// Get the remaining length of character sequence.
        /// </summary>
        public int RemainingLength => _queue.Length - _position;

        /// <summary>
        /// Determine whether the available character sequence is empty.
        /// </summary>
        public bool IsEmpty => RemainingLength == 0;

        /// <summary>
        /// Get the char at the current position.
        /// </summary>
        public char CurrentChar => IsEmpty ? (char)0 : _queue[_position];

        /// <summary>
        /// Get the chat next to the current position.
        /// </summary>
        public char NextChar => RemainingLength <= 1 ? (char)0 : _queue[_position + 1];

        /// <summary>
        /// Constructor.
        /// </summary>
        public TokenQueue(string data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            _queue = data;
        }

        /// <summary>
        /// Retrieves but does not remove the first character from the queue, returns 0 if empty.
        /// </summary>
        public char Peek()
        {
            return IsEmpty ? (char)0 : _queue[_position];
        }

        /// <summary>
        /// Add a character to the start of the queue (will be the next character retrieved).
        /// </summary>
        public void AddFirst(char c)
        {
            AddFirst(c.ToString());
        }

        /// <summary>
        /// Add a string to the start of the queue.
        /// </summary>
        public void AddFirst(string seq)
        {
            // not very performant, but an edge case
            _queue = seq + _queue.Substring(_position);
            _position = 0;
        }

        /// <summary>
        /// Tests whether the next characters on the queue match the sequence. Case insensitive.
        /// </summary>
        public bool Matches(string sequence)
        {
            if (_position + sequence.Length > _queue.Length)
            {
                return false;
            }

            return _queue
                .Substring(_position, sequence.Length)
                .Equals(sequence, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Tests whether the next characters on the queue match the sequence. Case sensitive.
        /// </summary>
        public bool MatchesCaseSensitive(string sequence)
        {
            if (_position + sequence.Length > _queue.Length)
            {
                return false;
            }

            return _queue.Substring(_position, sequence.Length) == sequence;
        }

        /// <summary>
        /// Tests whether the next characters match any of the sequences. Case insensitive.
        /// </summary>
        public bool MatchesAny(params string[] sequences)
        {
            return sequences.Any(Matches);
        }

        /// <summary>
        /// Tests whether the next characters match any of the character in sequences.
        /// </summary>
        public bool MatchesAny(params char[] sequence)
        {
            return !IsEmpty && sequence.Any(c => _queue[_position] == c);
        }

        /// <summary>
        /// Tests whether the next two characters match a start tag pattern.
        /// </summary>
        public bool MatchesStartTag()
        {
            return (RemainingLength >= 2 && CurrentChar == '<' && char.IsLetter(NextChar));
        }

        /// <summary>
        /// Tests whether the next character is a whitespace character.
        /// </summary>
        public bool MatchesWhitespace()
        {
            return !IsEmpty && string.IsNullOrWhiteSpace(CurrentChar.ToString());
        }

        /// <summary>
        /// Test whether the queue matches a word character (letter or digit).
        /// </summary>
        public bool MatchesWord()
        {
            return !IsEmpty && char.IsLetterOrDigit(CurrentChar);
        }

        /// <summary>
        /// Tests whether the queue matches the sequence, and if they do, removes the matched string from the queue.
        /// </summary>
        /// <param name="sequence">string to search for, and if found, remove from queue.</param>
        /// <returns>true if found and removed, false if not found.</returns>
        public bool MatchChomp(string sequence)
        {
            if (Matches(sequence))
            {
                _position += sequence.Length;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Drops the next character off the queue.
        /// </summary>
        public void Advance()
        {
            if (!IsEmpty)
            {
                _position++;
            }
        }

        /// <summary>
        /// Pulls a string off the queue (like consumeTo), and then pulls off the matched string (but does not return it).
        /// </summary>
        /// <remarks>
        /// If the queue runs out of characters before finding the sequence, will return as much as it can, and the queue will be empty.
        /// </remarks>
        /// <param name="sequence">Case sensitive string to match up to, and not include in return, and to pull off queue.</param>
        /// <returns>String matched from the queue.</returns>
        public string ChompTo(string sequence)
        {
            var str = ConsumeTo(sequence);
            MatchChomp(sequence);
            return str;
        }

        /// <summary>
        /// Pulls a string off the queue (like consumeTo), and then pulls off the matched string (but does not return it).
        /// </summary>
        /// <remarks>
        /// If the queue runs out of characters before finding the sequence, will return as much as it can, and the queue will be empty.
        /// </remarks>
        /// <param name="sequence">Case insensitive string to match up to, and not include in return, and to pull off queue.</param>
        /// <returns>String matched from the queue.</returns>
        public string ChompToIgnoreCase(string sequence)
        {
            var str = ConsumeToIgnoreCase(sequence);
            MatchChomp(sequence);
            return str;
        }

        /// <summary>
        /// Pulls a balanced string off the queue. E.g. if queue is "(one (two) three) four", (,) will return "one (two) three", 
        /// and leave " four" on the queue. Unbalanced openers and closers can be escaped (with \). Those escapes will be left 
        /// in the returned string, which is suitable for regexes (where we need to preserve the escape), but unsuitable for 
        /// contains text strings; use unescape for that. 
        /// </summary>
        /// <param name="open">opener</param>
        /// <param name="close">closer</param>
        /// <returns>String matched from the queue.</returns>
        public string ChompBalanced(char open, char close)
        {
            var accum = new StringBuilder();
            var depth = 0;
            var last = (char)0;

            do
            {
                if (IsEmpty) break;

                var c = Consume();
                if (last == 0 || last != Esc)
                {
                    if (c.Equals(open))
                    {
                        depth++;
                    }
                    else if (c.Equals(close))
                    {
                        depth--;
                    }
                }

                if (depth > 0 && last != 0)
                {
                    accum.Append(c); // don't include the outer match pair in the return
                }

                last = c;
            }
            while (depth > 0);

            return accum.ToString();
        }

        /// <summary>
        /// Consume the first character of the queue.
        /// </summary>
        public char Consume()
        {
            return _queue[_position++];
        }

        /// <summary>
        /// Consumes the supplied sequence of the queue.
        /// </summary>
        /// <remarks>Case insensitive.</remarks>
        /// <param name="sequence">sequence to remove from head of queue.</param>
        /// <exception cref="Exception">When the queue does not start with the supplied sequence.</exception>
        public void Consume(string sequence)
        {
            if (!Matches(sequence))
            {
                throw new Exception("Token queue did not match expected sequence");
            }

            var len = sequence.Length;

            if (len > RemainingLength)
            {
                throw new Exception("Token queue not long enough to consume sequence");
            }

            _position += len;
        }

        /// <summary>
        /// Pulls a string off the queue, up to but exclusive of the match sequence, or to the queue running out.
        /// </summary>
        /// <remarks>Case sensitive.</remarks>
        /// <param name="sequence">string to end on, and not include in return, but leave on queue.</param>
        /// <returns>The matched string consumed from queue.</returns>
        public string ConsumeTo(string sequence)
        {
            int offset = _queue.IndexOf(sequence, _position, StringComparison.Ordinal);

            if (offset != -1)
            {
                var consumed = _queue.Substring(_position, offset - _position);
                _position += consumed.Length;
                return consumed;
            }
            else
            {
                return ConsumeRemained();
            }
        }

        /// <summary>
        /// Pulls a string off the queue, up to but exclusive of the match sequence, or to the queue running out.
        /// </summary>
        /// <remarks>Case insensitive.</remarks>
        /// <param name="sequence">string to end on, and not include in return, but leave on queue.</param>
        /// <returns>The matched string consumed from queue.</returns>
        public string ConsumeToIgnoreCase(string sequence)
        {
            var start = _position;
            var first = sequence.Substring(0, 1);
            var canScan = first.ToLowerInvariant().Equals(first.ToUpperInvariant()); // if first is not cased, use index of

            while (!IsEmpty)
            {
                if (Matches(sequence))
                {
                    break;
                }

                if (canScan)
                {
                    var skip = _queue.IndexOf(first, _position, StringComparison.Ordinal) - _position;
                    if (skip == 0) // this char is the skip char, but not match, so force advance of position
                    {
                        _position++;
                    }
                    else if (skip < 0) // no chance of finding, grab to end
                    {
                        _position = _queue.Length;
                    }
                    else
                    {
                        _position += skip;
                    }
                }
                else
                {
                    _position++;
                }
            }

            return _queue.Substring(start, _position - start);
        }

        /// <summary>
        /// Consumes to the first sequence provided, or to the end of the queue. Leaves the terminator on the queue.
        /// </summary>
        /// <param name="sequences">any number of case insensitive terminators to consume to.</param>
        /// <returns>Consumed string.</returns>
        public string ConsumeToAny(params string[] sequences)
        {
            var start = _position;

            while (!IsEmpty && !MatchesAny(sequences))
            {
                _position++;
            }

            return _queue.Substring(start, _position - start);
        }

        /// <summary>
        /// Pulls the next run of whitespace characters of the queue.
        /// </summary>
        public bool ConsumeWhitespace()
        {
            var seen = false;

            while (MatchesWhitespace())
            {
                _position++;
                seen = true;
            }

            return seen;
        }

        /// <summary>
        /// Retrieves the next run of word type (letter or digit) off the queue.
        /// </summary>
        /// <returns>String of word characters from queue, or empty string if none.</returns>
        public string ConsumeWord()
        {
            var start = _position;

            while (MatchesWord())
            {
                _position++;
            }

            return _queue.Substring(start, _position - start);
        }

        /// <summary>
        /// Consume an tag name off the queue.
        /// </summary>
        /// <returns>Consumed tag name.</returns>
        public string ConsumeTagName()
        {
            var start = _position;

            while (!IsEmpty && (MatchesWord() || MatchesAny(':', '_', '-')))
            {
                _position++;
            }

            return _queue.Substring(start, _position - start);
        }

        /// <summary>
        /// Consume a CSS element selector off the queue.
        /// </summary>
        /// <returns>Consumed CSS element selector.</returns>
        public string ConsumeCssElementSelector()
        {
            var start = _position;

            while (!IsEmpty && (MatchesWord() || MatchesAny('|', '_', '-')))
            {
                _position++;
            }

            return _queue.Substring(start, _position - start);
        }

        /// <summary>
        /// Consume a CSS identifier (ID or class) off the queue.
        /// </summary>
        /// <see cref="http://www.w3.org/TR/CSS2/syndata.html#value-def-identifier"/>
        /// <returns>Consumed CSS identifier.</returns>
        public string ConsumeCssIdentifier()
        {
            var start = _position;

            while (!IsEmpty && (MatchesWord() || MatchesAny('-', '_')))
            {
                _position++;
            }

            return _queue.Substring(start, _position - start);
        }

        /// <summary>
        /// Consume an attribute key off the queue.
        /// </summary>
        /// <returns>Consumed attribute key.</returns>
        public string ConsumeAttributeKey()
        {
            var start = _position;

            while (!IsEmpty && (MatchesWord() || MatchesAny('-', '_', ':')))
            {
                _position++;
            }

            return _queue.Substring(start, _position - start);
        }

        /// <summary>
        /// Consume and return whatever is left on the queue.
        /// </summary>
        /// <returns>Remained string of queue.</returns>
        public string ConsumeRemained()
        {
            var accum = new StringBuilder();

            while (!IsEmpty)
            {
                accum.Append(Consume());
            }

            return accum.ToString();
        }

        /// <summary>
        /// Unescaped a \ escaped string.
        /// </summary>
        /// <param name="input">backslash escaped string</param>
        /// <returns>Unescaped string.</returns>
        public static string Unescape(string input)
        {
            var output = new StringBuilder();
            var last = (char)0;

            foreach (char c in input)
            {
                if (c == Esc)
                {
                    if (last != 0 && last == Esc)
                    {
                        output.Append(c);
                    }
                }
                else
                {
                    output.Append(c);
                }
                last = c;
            }

            return output.ToString();
        }

        public override string ToString()
        {
            return _queue.Substring(_position);
        }

    }
}