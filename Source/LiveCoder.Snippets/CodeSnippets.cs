using System.Collections.Immutable;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using LiveCoder.Api;
using LiveCoder.Common.Optional;
using LiveCoder.Common.Text.Documents;
using LiveCoder.Snippets.Elements;

namespace LiveCoder.Snippets
{
    class CodeSnippets
    {
        private ImmutableList<Snippet> Snippets { get; }
        public int Count => this.Snippets.Count;
        public static CodeSnippets Empty => new CodeSnippets(ImmutableList<Snippet>.Empty);
        public static Option<CodeSnippets> TryParse(FileInfo file, ILogger logger) =>
            Load(file).MapOptional(new ScriptTextParser(logger).TryParse);

        private static Option<NonEmptyText> Load(FileInfo source) =>
            LoadConcurrently(source) is string[] array && array.Length > 0
                ? Option.Of(new NonEmptyText(array))
                : None.Value;

        private static string[] LoadConcurrently(FileInfo source)
        {
            int repeats = 10;
            int waitMsec = 100;
            for (int repeat = 0; repeat < repeats; repeat++)
            {
                try
                {
                    return File.ReadAllLines(source.FullName, Encoding.UTF8);
                }
                catch 
                {
                    Thread.Sleep(waitMsec);
                }
            }

            return new string[0];
        }

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
