using System.Text.RegularExpressions;

namespace LiveCoder.Scripting.Lexing.Lexemes
{
    public class StringLiteral : Token
    {
        private StringLiteral(string value) : base(value)
        {
        }

        public static Token Of(string value) => new StringLiteral(value);

        public string RawValue =>
            new Regex("\"(?<content>[^\"]*)\"").Replace(Value, @"${content}");
    }
}
