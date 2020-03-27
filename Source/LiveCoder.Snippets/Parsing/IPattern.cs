using System.Text.RegularExpressions;
using LiveCoder.Common.Optional;
using LiveCoder.Common.Text.Documents;

namespace LiveCoder.Snippets.Parsing
{
    interface IPattern
    {
        Regex StartsWith { get; }
        Option<(IText remaining, CodeSnippets script)> Apply(NonEmptyText current, CodeSnippets script);
    }
}
