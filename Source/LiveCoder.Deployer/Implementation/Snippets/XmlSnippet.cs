namespace LiveCoder.Deployer.Implementation.Snippets
{
    class XmlSnippet
    {
        public int Number { get; }

        public string Code { get; }

        public XmlSnippet(int number, string code)
        {
            this.Number = number;
            this.Code = code;
        }

        public override string ToString() =>
            $"Snippet: {this.Number} -> {this.Code}";
    }
}
