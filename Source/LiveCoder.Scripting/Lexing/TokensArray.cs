using System;
using System.Collections.Generic;
using System.Linq;
using LiveCoder.Common;
using LiveCoder.Common.Optional;
using LiveCoder.Scripting.Lexing.Lexemes;

namespace LiveCoder.Scripting.Lexing
{
    public class TokensArray
    {
        private Token[] Tokens { get; }
        private int Index { get; }

        public static TokensArray Empty() => 
            new TokensArray(Enumerable.Empty<Token>());

        public TokensArray(IEnumerable<Token> tokens)
        {
            this.Tokens = tokens.ToArray();
            this.Index = 0;
        }


        private TokensArray(TokensArray array, int indexOffset)
        {
            this.Tokens = array.Tokens;
            this.Index = Math.Max(Math.Min(array.Index + indexOffset, this.Tokens.Length), 0);
        }

        public TokensArray StripSeparators() =>
            new TokensArray(this.NotSeparators);

        private IEnumerable<Token> NotSeparators =>
            this.Tokens.Where(token => !(token is WhiteSpace) && !(token is EndOfLine));

        public Option<Token> FirstOrNone() =>
            this.Index >= this.Tokens.Length ? Option.None<Token>()
            : this.Tokens[this.Index];

        public bool Any() =>
            this.Index < this.Tokens.Length;

        public TokensArray Next() =>
            new TokensArray(this, 1);

        public IEnumerable<Token> GetAll() =>
            Enumerable.Range(this.Index, this.Tokens.Length - this.Index)
                .Select(index => this.Tokens[index]);

        public override string ToString() => 
            this.Tokens.Select(token => token.ToString()).Join(Environment.NewLine);
    }
}
