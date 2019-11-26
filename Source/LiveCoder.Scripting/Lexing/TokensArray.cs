using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LiveCoder.Scripting.Lexing
{
    public class TokensArray : IEnumerable<Token>
    {
        private IEnumerable<Token> Tokens { get; }

        public TokensArray(IEnumerable<Token> tokens)
        {
            this.Tokens = tokens.ToList();
        }

        public IEnumerator<Token> GetEnumerator() => 
            this.Tokens.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => 
            GetEnumerator();
    }
}
