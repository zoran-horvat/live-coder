using System.Collections.Generic;
using System.Linq;
using LiveCoder.Scripting.Lexing.Lexemes;
using LiveCoder.Scripting.Text;

namespace LiveCoder.Scripting.Lexing
{
    public class Lexer
    {
        public IEnumerable<Token> Tokenize(IText text) =>
            text is NonEmptyText nonEmpty ? this.Tokenize(nonEmpty) 
            : Enumerable.Empty<Token>();

        private IEnumerable<Token> Tokenize(NonEmptyText text)
        {
            yield return new Identifier(text.CurrentLine);
        }
    }
}
