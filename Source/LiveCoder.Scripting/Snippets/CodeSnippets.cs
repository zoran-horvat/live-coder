using System.Collections.Immutable;
using System.IO;
using System.Text.RegularExpressions;
using LiveCoder.Common.Optional;
using LiveCoder.Scripting.Elements;
using LiveCoder.Scripting.Parsing;
using LiveCoder.Scripting.Text;

namespace LiveCoder.Scripting.Snippets
{
    class CodeSnippets
    {
        private ImmutableList<Snippet> Snippets { get; }

        public static CodeSnippets Empty => new CodeSnippets(ImmutableList<Snippet>.Empty);
        public static Option<CodeSnippets> TryParse(FileInfo file, IScriptingAuditor auditor) =>
            NonEmptyText.Load(file).MapOptional(new ScriptTextParser(auditor).TryParse);

        private CodeSnippets(ImmutableList<Snippet> snippets)
        {
            this.Snippets = snippets;
        }

        public CodeSnippets Add(Snippet snippet) =>
            new CodeSnippets(this.Snippets.Remove(snippet, new SnippetNumberComparer()).Add(snippet));

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
