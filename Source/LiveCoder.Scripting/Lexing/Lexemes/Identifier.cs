namespace LiveCoder.Scripting.Lexing.Lexemes
{
    public class Identifier : Token
    {
        private Identifier(string value) : base(value)
        {
        }

        public static Token Of(string value) => new Identifier(value);
    }
}
