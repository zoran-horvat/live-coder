using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LiveCoder.Common.Optional;
using LiveCoder.Scripting.Lexing.Lexemes;
using LiveCoder.Scripting.Text;

namespace LiveCoder.Scripting.Lexing
{
    public class Lexer
    {
        private IEnumerable<(Regex pattern, Func<string, Token> tokenFactory)> Patterns { get; }

        public Lexer()
        {
            Regex numberPattern = new Regex(@"\d+");
            Regex operatorPattern = new Regex(@"[\.\(\),]");
            Regex whiteSpacePattern = new Regex(@"\s+");
            Regex identifierPattern = new Regex(@"[a-zA-Z_][a-zA-Z0-9_]*");
            Regex stringPattern = new Regex(@"""(\\[""\\n]|[^\\])*""");

            this.Patterns = new (Regex, Func<string, Token>)[]
            {
                (numberPattern, Number.Of),
                (operatorPattern, Operator.Of),
                (whiteSpacePattern, WhiteSpace.Of),
                (identifierPattern, Identifier.Of),
                (stringPattern, StringLiteral.Of)
            };
        }

        public TokensArray Tokenize(IText text) =>
            new TokensArray(this.GetTokensSequence(text));

        private IEnumerable<Token> GetTokensSequence(IText text)
        {
            while (text is NonEmptyText remaining)
            {
                foreach (Token partial in this.Tokenize(remaining.CurrentLine))
                    yield return partial;
                yield return new EndOfLine();
                text = remaining.ConsumeLine();
            }
        }

        public IEnumerable<Token> Tokenize(string line)
        {
            int pos = 0;
            while (pos < line.Length)
            {
                (string value, Func<string, Token> tokenFactory) = this.Match(line, pos);
                yield return tokenFactory(value);
                pos += value.Length;
            }
        }

        private (string value, Func<string, Token> tokenFactory) Match(string target, int pos) =>
            this.Matches(target, pos)
                .FirstOrNone(tuple => tuple.match.Index == pos)
                .Map(tuple => (tuple.match.Value, tuple.tokenFactory))
                .Reduce(() => (target.Substring(pos), InvalidToken.Of));

        private IEnumerable<(Match match, Func<string, Token> tokenFactory)> Matches(string target, int pos) =>
            this.Patterns
                .Select(tuple => (match: tuple.pattern.Match(target, pos), tuple.tokenFactory))
                .Where(tuple => tuple.match.Success);
    }
}
