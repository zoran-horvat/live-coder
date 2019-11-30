using System;

namespace LiveCoder.Scripting.Lexing
{
    public abstract class Token : IEquatable<Token>
    {
        public string Value { get; }
     
        protected Token(string value)
        {
            this.Value = value;
        }

        public override string ToString() =>
            $"{this.GetType().Name}({this.Value})";

        public bool Equals(Token other) =>
            other is Token some && other.GetType() == this.GetType() && some.Value == this.Value;

        public override bool Equals(object obj) =>
            this.Equals(obj as Token);

        public override int GetHashCode() =>
            this.Value?.GetHashCode() ?? 0;

        public static bool operator == (Token a, Token b) =>
            a?.Equals(b) ?? b is null;

        public static bool operator !=(Token a, Token b) =>
            !(a == b);
    }
}
