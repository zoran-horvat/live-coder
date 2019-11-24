namespace LiveCoder.Scripting.Lexing.Lexemes
{
    public class Operator : Token
    {
        private Operator(string value) : base(value)
        {
        }

        public static Token Of(string value) => new Operator(value);
    }
}
