using System;

namespace LiveCoder.Scripting.Lexing.Lexemes
{
    public class EndOfLine : Token
    {
        public EndOfLine() : base(Environment.NewLine)
        {
        }
    }
}
