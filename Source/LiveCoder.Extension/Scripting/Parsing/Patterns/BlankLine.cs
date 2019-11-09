using System.Text.RegularExpressions;
using LiveCoder.Common.Optional;

namespace LiveCoder.Extension.Scripting.Parsing.Patterns
{
    class BlankLine : IPattern
    {
        public Regex StartsWith => new Regex(@"^\s*$");

        public Option<(IText remaining, DemoScript script)> Apply(NonEmptyText current, DemoScript script) =>
            Option.Of((current.ConsumeLine(), script));
    }
}
