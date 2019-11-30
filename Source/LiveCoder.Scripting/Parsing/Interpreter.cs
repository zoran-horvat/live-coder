using System.Collections;
using System.Linq;
using LiveCoder.Scripting.Lexing;

namespace LiveCoder.Scripting.Parsing
{
    public class Interpreter
    {
        public IEnumerable Parse(TokensArray tokens) =>
            Enumerable.Empty<object>();
    }
}
