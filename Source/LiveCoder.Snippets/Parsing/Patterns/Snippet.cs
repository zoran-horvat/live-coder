using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LiveCoder.Common.Optional;
using LiveCoder.Snippets.Interfaces;
using LiveCoder.Snippets.Text;

namespace LiveCoder.Snippets.Parsing.Patterns
{
    class Snippet : IPattern
    {
        public Regex StartsWith => new Regex("^snippet ");
        private const string PreamblePattern = @"^snippet\s+(?<number>[^\s]+)\s+until\s+(?<terminator>.+)$";

        public Option<(IText remaining, CodeSnippets script)> Apply(NonEmptyText current, CodeSnippets script) => 
            this.ApplyBeforePreamble(current, script);

        private Option<(IText remaining, CodeSnippets script)> ApplyBeforePreamble(NonEmptyText current, CodeSnippets script) =>
            from preamble in this.TryExtractPreamble(current)
            from text in current.ConsumeLine().ObjectOfType<NonEmptyText>()
            from record in this.ApplyAfterPreamble(text, preamble.number, preamble.terminator, script)
            select record;

        private Option<(int number, string terminator)> TryExtractPreamble(NonEmptyText text) =>
            new Regex(PreamblePattern)
                .TryExtract(text.CurrentLine)
                .Map(groups => (number: int.Parse(groups["number"].Value), terminator: groups["terminator"].Value));

        private Option<(IText remaining, CodeSnippets script)> ApplyAfterPreamble(NonEmptyText current, int snippetNumber, string terminator, CodeSnippets script) =>
            current.ConsumeUntilMatch(this.EndingIn(terminator)).Map(pair => (
                remaining: pair.remaining, 
                script: this.Apply(snippetNumber, this.RemoveTerminator(pair.lines, terminator), script)));

        private CodeSnippets Apply(int snippetNumber, string[] content, CodeSnippets script) =>
            script.Add(new Snippets.Elements.Snippet(snippetNumber, string.Join(Environment.NewLine, content)));

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
