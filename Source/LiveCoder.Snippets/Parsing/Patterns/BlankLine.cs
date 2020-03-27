using System.Text.RegularExpressions;
using LiveCoder.Common.Optional;
using LiveCoder.Common.Text.Documents;

namespace LiveCoder.Snippets.Parsing.Patterns
{
    class BlankLine : IPattern
    {
        public Regex StartsWith => new Regex(@"^\s*$");

        public Option<(IText remaining, CodeSnippets script)> Apply(NonEmptyText current, CodeSnippets script) =>
            Option.Of((current.ConsumeLine(), script));
    }
}
