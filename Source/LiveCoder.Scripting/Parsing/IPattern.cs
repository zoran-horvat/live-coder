using System.Text.RegularExpressions;
using LiveCoder.Common.Optional;
using LiveCoder.Scripting.Snippets;
using LiveCoder.Scripting.Text;

namespace LiveCoder.Scripting.Parsing
{
    interface IPattern
    {
        Regex StartsWith { get; }
        Option<(IText remaining, CodeSnippets script)> Apply(NonEmptyText current, CodeSnippets script);
    }
}
