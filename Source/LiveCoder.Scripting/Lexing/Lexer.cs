using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LiveCoder.Scripting.Lexing.Lexemes;
using LiveCoder.Scripting.Text;

namespace LiveCoder.Scripting.Lexing
{
    public class Lexer
    {
        private Regex WhiteSpacePattern { get; } = new Regex(@"[\s]+");

        public IEnumerable<Token> Tokenize(IText text) =>
            text is NonEmptyText nonEmpty ? this.Tokenize(nonEmpty) 
            : Enumerable.Empty<Token>();

        private IEnumerable<Token> Tokenize(NonEmptyText text)
        {
            yield return this.Tokenize(text.CurrentLine);
            while (text.ConsumeLine() is NonEmptyText moved)
            {
                text = moved;
                yield return this.Tokenize(text.CurrentLine);
            }
        }

        private Token Tokenize(string line) =>
            this.WhiteSpacePattern.Match(line) is Match whiteSpaceMatch && whiteSpaceMatch.Success ? (Token)new WhiteSpace(whiteSpaceMatch.Value)
            : new Identifier(line);
    }
}
