namespace LiveCoder.Scripting.Lexing.Lexemes
{
    public class Number : Token
    {
        private Number(string value) : base(value)
        {
        }

        public static Token Of(string value) => new Number(value);
    }
}
