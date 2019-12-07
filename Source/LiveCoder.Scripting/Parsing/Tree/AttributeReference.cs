using LiveCoder.Scripting.Lexing.Lexemes;

namespace LiveCoder.Scripting.Parsing.Tree
{
    public class AttributeReference : Reference
    {
        public Identifier Identifier { get; }

        public AttributeReference(Identifier identifier)
        {
            this.Identifier = identifier;
        }

        public override string ToString() =>
            this.Identifier.Value;
    }
}