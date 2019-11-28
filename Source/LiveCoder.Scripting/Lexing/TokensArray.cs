using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LiveCoder.Common;
using LiveCoder.Scripting.Lexing.Lexemes;

namespace LiveCoder.Scripting.Lexing
{
    public class TokensArray : IEnumerable<Token>
    {
        private IEnumerable<Token> Tokens { get; }

        public static TokensArray Empty() => 
            new TokensArray(Enumerable.Empty<Token>());

        public TokensArray(IEnumerable<Token> tokens)
        {
            this.Tokens = tokens.ToList();
        }

        public TokensArray StripSeparators() =>
            new TokensArray(this.NotSeparators);

        private IEnumerable<Token> NotSeparators =>
            this.Tokens.Where(token => !(token is WhiteSpace) && !(token is EndOfLine));

        public IEnumerator<Token> GetEnumerator() => 
            this.Tokens.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => 
            GetEnumerator();

        public override string ToString() => 
            this.Tokens.Select(token => token.ToString()).Join(Environment.NewLine);
    }
}
