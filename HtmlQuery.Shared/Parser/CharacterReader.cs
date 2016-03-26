using System;
using System.Text.RegularExpressions;

namespace Fnio.Lib.HtmlQuery.Parser
{
    /// <summary>
    /// CharacterReader consumes tokens off a string.
    /// </summary>
    internal class CharacterReader
    {
        public const char EOF = (char) 254;

        /// <summary>
        /// Get all characters of reader.
        /// </summary>
        public char[] Data { get; }

        /// <summary>
        /// Get current position.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Get the length of input character sequence.
        /// </summary>
        public int Length => Data.Length;

        /// <summary>
        /// Determins wheter there is any unconsumed character.
        /// </summary>
        public bool IsEmpty => Position >= Length;

        /// <summary>
        /// Get the current character in reader.
        /// </summary>
        public char Current => IsEmpty ? EOF : Data[Position];

        /// <summary>
        /// Get mark position.
        /// </summary>
        public int MarkPosition { get; private set; }

        public CharacterReader(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            input = Regex.Replace(input, "\r\n?", "\n"); // normalise carriage returns to newlines
            Data = input.ToCharArray();
        }

        /// <summary>
        /// Returns current char and move advance.
        /// </summary>
        public char Consume() => IsEmpty ? EOF : Data[Position++];

        /// <summary>
        /// Consume current char as string.
        /// </summary>
        public string ConsumeAsString() => new string(Data, Position++, 1);

        /// <summary>
        /// Move position backward.
        /// </summary>
        public void Unconsume() => Position--;

        /// <summary>
        /// Move position advance.
        /// </summary>
        public void Advance() => Position++;

        /// <summary>
        /// Mark current position.
        /// </summary>
        public void Mark() => MarkPosition = Position;

        public void RewindToMark()
        {
            Position = MarkPosition;
        }


        /// <summary>
        /// Returns the number of characters between the current position and the next instance of the input char.
        /// </summary>
        /// <param name="c">Scan target char</param>
        /// <returns>Offset between current position and next instance of target. -1 if not found.</returns>
        public int NextIndexOf(char c)
        {
            for (var i = Position; i < Length; i++)
            {
                if (c == Data[i])
                {
                    return i - Position;
                }
            }
            return -1;
        }

        /// <summary>
        /// Returns the number of characters between the current position and the next instance of the input sequence.
        /// </summary>
        /// <param name="sequence">Scan target char sequence</param>
        /// <returns>Offset between current position and next instance of target. -1 if not found.</returns>
        public int NextIndexOf(string sequence)
        {
            // TODO: refine
            // doesn't handle scanning for surrogates
            var startChar = sequence[0];
            for (var offset = Position; offset < Length; offset++)
            {
                // scan to first instance of startchar:
                if (startChar != Data[offset])
                {
                    while (++offset < Length && startChar != Data[offset]) ;
                }
                if (offset < Length)
                {
                    var i = offset + 1;
                    var last = i + sequence.Length - 1;

                    for (var j = 1; i < last && sequence[j] == Data[i]; i++, j++) ;

                    if (i == last) // found full sequence
                    {
                        return offset - Position;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Consume to specific char.
        /// </summary>
        public string ConsumeTo(char c)
        {
            var offset = NextIndexOf(c);
            if (offset == -1) return ConsumeToEnd();
            var consumed = new string(Data, Position, offset);
            Position += offset;
            return consumed;
        }

        /// <summary>
        /// Consume to specific char sequence.
        /// </summary>
        public string ConsumeTo(string sequence)
        {
            var offset = NextIndexOf(sequence);
            if (offset == -1) return ConsumeToEnd();
            var consumed = new string(Data, Position, offset);
            Position += offset;
            return consumed;
        }

        /// <summary>
        /// Consume to any matched char sequence.
        /// </summary>
        public string ConsumeToAny(params char[] sequences)
        {
            var start = Position;

            var flag = false;
            while (Position < Length && !flag)
            {
                for (var i = 0; i < sequences.Length; i++)
                {
                    if (Data[Position] == sequences[i])
                    {
                        flag = true; // Break outer loop.
                        Position--; // Nullify next pos++ operation.
                        break;
                    }
                }
                Position++;
            }

            return Position > start ? new string(Data, start, Position - start) : string.Empty;
        }

        /// <summary>
        /// Consume the reset characters in reader.
        /// </summary>
        public string ConsumeToEnd()
        {
            var data = new string(Data, Position, Length - Position);
            Position = Length;
            return data;
        }

        public string ConsumeLetterSequence()
        {
            var start = Position;
            while (Position < Length)
            {
                var c = Data[Position];
                if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    Position++;
                }
                else
                {
                    break;
                }
            }

            return new string(Data, start, Position - start);
        }

        public string ConsumeLetterThenDigitSequence()
        {
            var start = Position;
            while (Position < Length)
            {
                var c = Data[Position];
                if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    Position++;
                }
                else
                {
                    break;
                }
            }
            while (!IsEmpty)
            {
                var c = Data[Position];
                if (c >= '0' && c <= '9')
                {
                    Position++;
                }
                else
                {
                    break;
                }
            }

            return new string(Data, start, Position - start);
        }

        public string ConsumeHexSequence()
        {
            var start = Position;
            while (Position < Length)
            {
                var c = Data[Position];
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f'))
                {
                    Position++;
                }
                else
                {
                    break;
                }
            }
            return new string(Data, start, Position - start);
        }

        public string ConsumeDigitSequence()
        {
            var start = Position;
            while (Position < Length)
            {
                var c = Data[Position];
                if (c >= '0' && c <= '9')
                {
                    Position++;
                }
                else
                {
                    break;
                }
            }
            return new string(Data, start, Position - start);
        }

        public bool Matches(char c)
        {
            return !IsEmpty && Data[Position] == c;
        }

        public bool Matches(string seq)
        {
            var scanLength = seq.Length;
            if (scanLength > Length - Position)
            {
                return false;
            }

            for (var offset = 0; offset < scanLength; offset++)
            {
                if (seq[offset] != Data[Position + offset])
                {
                    return false;
                }
            }

            return true;
        }

        public bool MatchesIgnoreCase(string seq)
        {
            var scanLength = seq.Length;
            if (scanLength > Length - Position)
            {
                return false;
            }

            for (var offset = 0; offset < scanLength; offset++)
            {
                var upScan = char.ToUpperInvariant(seq[offset]);
                var upTarget = char.ToUpperInvariant(Data[Position + offset]);
                if (upScan != upTarget)
                {
                    return false;
                }
            }
            return true;
        }

        public bool MatchesAny(params char[] seq)
        {
            if (IsEmpty)
            {
                return false;
            }

            var c = Data[Position];
            foreach (var seek in seq)
            {
                if (seek == c)
                {
                    return true;
                }
            }
            return false;
        }

        public bool MatchesLetter()
        {
            if (IsEmpty)
            {
                return false;
            }
            var c = Data[Position];
            return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
        }

        public bool MatchesDigit()
        {
            if (IsEmpty)
            {
                return false;
            }
            var c = Data[Position];
            return c >= '0' && c <= '9';
        }

        public bool MatchConsume(string seq)
        {
            if (Matches(seq))
            {
                Position += seq.Length;
                return true;
            }
            return false;
        }

        public bool MatchConsumeIgnoreCase(string seq)
        {
            if (MatchesIgnoreCase(seq))
            {
                Position += seq.Length;
                return true;
            }
            return false;
        }

        public bool ContainsIgnoreCase(string seq)
        {
            // used to check presence of </title>, </style>. only finds consistent case.
            var loScan = seq.ToLower( /*CultureInfo.CreateSpecificCulture("en-US")*/);
            var hiScan = seq.ToUpper( /*CultureInfo.CreateSpecificCulture("en-US")*/);
            return (NextIndexOf(loScan) > -1) || (NextIndexOf(hiScan) > -1);
        }


        public override string ToString()
        {
            return new string(Data, Position, Length - Position);
        }
    }
}