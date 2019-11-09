using System.Collections.Immutable;
using System.IO;
using LiveCoder.Common.Optional;
using LiveCoder.Extension.Interfaces;
using LiveCoder.Extension.Scripting.Elements;
using LiveCoder.Extension.Scripting.Parsing;

namespace LiveCoder.Extension.Scripting
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
