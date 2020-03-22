using System.Text.RegularExpressions;
using LiveCoder.Common.Optional;
using LiveCoder.Snippets.Interfaces;
using LiveCoder.Snippets.Text;

namespace LiveCoder.Snippets.Parsing
{
    interface IPattern
    {
        Regex StartsWith { get; }
        Option<(IText remaining, CodeSnippets script)> Apply(NonEmptyText current, CodeSnippets script);
    }
}
