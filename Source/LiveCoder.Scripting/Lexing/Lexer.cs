using System.Collections.Generic;
using System.Linq;

namespace LiveCoder.Scripting.Lexing
{
    public class Lexer
    {
        public IEnumerable<object> Tokenize(IText emptyText)
        {
            return Enumerable.Empty<object>();
        }
    }
}
