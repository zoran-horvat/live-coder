using System;
using System.Collections.Generic;

namespace LiveCoder.Deployer.Implementation.Snippets
{
    class XmlSnippet
    {
        public int Number { get; }

        public string Code { get; }

        public IEnumerable<string> CodeLines =>
            this.Code.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None);

        public XmlSnippet(int number, string code)
        {
            this.Number = number;
            this.Code = code;
        }

        public override string ToString() =>
            $"Snippet: {this.Number} -> {this.Code}";
    }
}
