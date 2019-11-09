using System.Collections.Immutable;
using System.IO;
using Common.Optional;
using LiveCoderExtension.Interfaces;
using LiveCoderExtension.Scripting.Elements;
using LiveCoderExtension.Scripting.Parsing;

namespace LiveCoderExtension.Scripting
{
    class DemoScript
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

        public override string ToString() =>
            $"Script: {this.Snippets.Count} snippet(s)";
    }
}
