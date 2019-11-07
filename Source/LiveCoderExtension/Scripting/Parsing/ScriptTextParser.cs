using System.Collections.Generic;
using Common.Optional;
using LiveCoderExtension.Interfaces;
using LiveCoderExtension.Scripting.Parsing.Events;
using LiveCoderExtension.Scripting.Parsing.Patterns;

namespace LiveCoderExtension.Scripting.Parsing
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

        public Option<DemoScript> TryParse(NonEmptyText content)
        {
            Option<DemoScript> script = Option.Of(new DemoScript());
            IText rest = content;

            while (script is Some<DemoScript> someScript &&
                   rest is NonEmptyText remaining && 
                   this.TryMatch(remaining) is Some<IPattern> somePattern &&
                   somePattern.Content is IPattern pattern)
            {
                Option<(IText newRest, DemoScript newScript)> newState = pattern.Apply(remaining, someScript);
                script = newState.Map(state => state.newScript);
                rest = newState.Map(state => state.newRest).Reduce(rest);
            }

            rest.OfType<NonEmptyText>().Do(remaining => this.Logger.Write(new ErrorParsingLine(remaining)));

            return script;
        }

        private Option<IPattern> TryMatch(NonEmptyText content) =>
            this.Patterns.FirstOrNone(pattern => pattern.StartsWith.IsMatch(content.CurrentLine));
    }
}
