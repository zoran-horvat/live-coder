using System.Text.RegularExpressions;
using Common.Optional;

namespace LiveCoderExtension.Scripting.Parsing
{
    interface IPattern
    {
        Regex StartsWith { get; }
        Option<(IText remaining, DemoScript script)> Apply(NonEmptyText current, DemoScript script);
    }
}
