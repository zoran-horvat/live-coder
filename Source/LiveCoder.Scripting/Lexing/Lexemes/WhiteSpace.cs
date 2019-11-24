namespace LiveCoder.Scripting.Lexing.Lexemes
{
    public class WhiteSpace : Token
    {
        private WhiteSpace(string value) : base(value)
        {
        }

        public static Token Of(string value) => new WhiteSpace(value);
    }
}
