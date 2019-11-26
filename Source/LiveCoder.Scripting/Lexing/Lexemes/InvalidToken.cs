namespace LiveCoder.Scripting.Lexing.Lexemes
{
    public class InvalidToken : Token
    {
        private InvalidToken(string value) : base(value)
        {
        }

        public static Token Of(string value) => new InvalidToken(value);
    }
}
