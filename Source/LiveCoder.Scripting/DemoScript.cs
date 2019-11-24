using System.Collections.Immutable;
using System.IO;
using System.Text.RegularExpressions;
using LiveCoder.Common.Optional;
using LiveCoder.Scripting.Elements;
using LiveCoder.Scripting.Parsing;

namespace LiveCoder.Scripting
{
    public class DemoScript
    {
        private ImmutableList<Snippet> Snippets { get; }

        public static Option<DemoScript> TryParse(FileInfo file, ILogger logger) =>
            NonEmptyText.Load(file).MapOptional(new ScriptTextParser(logger).TryParse);

        public DemoScript() : this(ImmutableList<Snippet>.Empty) { }

        private DemoScript(ImmutableList<Snippet> snippets)
        {
            this.Snippets = snippets;
        }

        public DemoScript Append(Snippet snippet) =>
            new DemoScript(this.Snippets.Add(snippet));

        public Option<Snippet> TryGetSnippet(string shortcut) =>
            Regex.Match(shortcut, @"^snp(?<number>\d+)$") is Match match &&
            match.Success &&
            match.Groups["number"] is Group group &&
            group.Value is string number
                ? this.TryGetSnippet(int.Parse(number))
                : None.Value;

        private Option<Snippet> TryGetSnippet(int number) =>
            this.Snippets.FirstOrNone(snippet => snippet.Number == number);

        public override string ToString() =>
            $"Script: {this.Snippets.Count} snippet(s)";
    }
}
