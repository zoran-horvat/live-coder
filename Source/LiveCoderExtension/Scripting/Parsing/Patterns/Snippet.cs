using System.Text.RegularExpressions;
using Common.Optional;

namespace LiveCoderExtension.Scripting.Parsing.Patterns
{
    class Snippet : IPattern
    {
        public Regex StartsWith => new Regex("^snippet ");
        private const string PreamblePattern = @"^snippet\s+(?<number>[^\s]+)\s+until\s+(?<terminator>.+)$";

        public Option<(IText remaining, DemoScript script)> Apply(NonEmptyText current, DemoScript script) => 
            this.ApplyBeforePreamble(current, script);

        private Option<(IText remaining, DemoScript script)> ApplyBeforePreamble(NonEmptyText current, DemoScript script) =>
            from preamble in new Regex(PreamblePattern).TryExtract(current.CurrentLine)
            from text in current.ConsumeLine().OfType<NonEmptyText>()
            from record in this.ApplyAfterPreamble(text, preamble["number"].Value, preamble["terminator"].Value, script)
            select record;

        private Option<(IText remaining, DemoScript script)> ApplyAfterPreamble(IText current, string snippetNumber, string terminator, DemoScript script) =>
            None.Value;
    }
}
