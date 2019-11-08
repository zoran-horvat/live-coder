using System.Collections.Generic;
using System.Linq;
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
            from preamble in this.TryExtractPreamble(current)
            from text in current.ConsumeLine().OfType<NonEmptyText>()
            from record in this.ApplyAfterPreamble(text, preamble.number, preamble.terminator, script)
            select record;

        private Option<(string number, string terminator)> TryExtractPreamble(NonEmptyText text) =>
            new Regex(PreamblePattern)
                .TryExtract(text.CurrentLine)
                .Map(groups => (number: groups["number"].Value, terminator: groups["terminator"].Value));

        private Option<(IText remaining, DemoScript script)> ApplyAfterPreamble(NonEmptyText current, string snippetNumber, string terminator, DemoScript script) =>
            current.ConsumeUntilMatch(this.EndingIn(terminator)).Map(pair => (
                remaining: pair.remaining, 
                script: this.Apply(snippetNumber, this.RemoveTerminator(pair.lines, terminator), script)));

        private DemoScript Apply(string snippetNumber, string[] content, DemoScript script) => 
            script;

        private Regex EndingIn(string terminator) =>
            new Regex($"{Regex.Escape(terminator)}$");

        private string[] RemoveTerminator(IEnumerable<string> content, string terminator)
        {
            string[] array = content.ToArray();
            string lastLine = array[array.Length - 1];
            string trimmed = lastLine.Substring(0, lastLine.Length - terminator.Length);
            array[array.Length - 1] = trimmed;
            return array;
        }
    }
}
