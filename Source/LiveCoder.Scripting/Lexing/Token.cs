namespace LiveCoder.Scripting.Lexing
{
    public abstract class Token
    {
        public string Value { get; }
     
        protected Token(string value)
        {
            this.Value = value;
        }

        public override string ToString() =>
            $"{this.GetType().Name}({this.Value})";
    }
}
