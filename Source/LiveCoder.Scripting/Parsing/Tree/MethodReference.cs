using LiveCoder.Scripting.Lexing.Lexemes;

namespace LiveCoder.Scripting.Parsing.Tree
{
    public class MethodReference : Reference
    {
        private Identifier Method { get; }
     
        public MethodReference(Identifier method)
        {
            this.Method = method;
        }
    }
}
