using System.Collections.Generic;
using LiveCoder.Api;
using LiveCoder.Common.Optional;
using LiveCoder.Common.Text.Documents;
using LiveCoder.Snippets.Parsing;
using LiveCoder.Snippets.Parsing.Patterns;

namespace LiveCoder.Snippets
{
    class ScriptTextParser
    {
        private ILogger Logger { get; }
        private IEnumerable<IPattern> Patterns { get; }

        public ScriptTextParser(ILogger logger)
        {
            this.Logger = logger;
            this.Patterns = new IPattern[]
            {
                new Snippet(), 
                new BlankLine(), 
            };
        }

        public Option<CodeSnippets> TryParse(NonEmptyText content)
        {
            Option<CodeSnippets> script = Option.Of(CodeSnippets.Empty);
            IText rest = content;

            while (script is Some<CodeSnippets> someScript &&
                   rest is NonEmptyText remaining && 
                   this.TryMatch(remaining) is Some<IPattern> somePattern &&
                   somePattern.Content is IPattern pattern)
            {
                Option<(IText newRest, CodeSnippets newScript)> newState = pattern.Apply(remaining, someScript);
                script = newState.Map(state => state.newScript);
                rest = newState.Map(state => state.newRest).Reduce(rest);
            }

            return script;
        }

        private Option<IPattern> TryMatch(NonEmptyText content) =>
            this.Patterns.FirstOrNone(pattern => pattern.StartsWith.IsMatch(content.CurrentLine));
    }
}
