using System;
using System.Collections.Generic;
using System.Linq;
using LiveCoder.Scripting.Lexing;
using LiveCoder.Scripting.Text;

namespace LiveCoder.Scripting.Tests.Lexing
{
    public abstract class TokenizationTestsBase
    {
        protected IEnumerable<Token> TokensAtOddIndices(string[] lines) =>
            this.TokensAtIndices(lines, index => index % 2 == 1);

        protected IEnumerable<Token> TokensAtEvenIndices(string[] lines) =>
            this.TokensAtIndices(lines, index => index % 2 == 0);

        private IEnumerable<Token> TokensAtIndices(string[] lines, Func<int, bool> predicate) =>
            this.Tokenize(lines)
                .Select((token, index) => (token, index))
                .Where(tuple => predicate(tuple.index))
                .Select(tuple => tuple.token);

        protected IEnumerable<Token> Tokenize(params string[] lines) =>
            this.Tokenize(new NonEmptyText(lines));

        private IEnumerable<Token> Tokenize(IText text) =>
            new Lexer().Tokenize(text);

    }
}
