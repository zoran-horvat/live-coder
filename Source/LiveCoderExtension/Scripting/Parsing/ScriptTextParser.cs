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
            this.Patterns = new[]
            {
                new Snippet(), 
            };
        }

        public Option<DemoScript> TryParse(Text content)
        {
            Option<DemoScript> script = new Some<DemoScript>(new DemoScript());
            Option<Text> rest = new Some<Text>(content);

            while (script is Some<DemoScript> currentScript &&
                   rest is Some<Text> remaining && 
                   this.TryMatch(remaining) is Some<IPattern> somePattern &&
                   somePattern.Content is IPattern pattern)
            {
                (rest, script) = pattern.Apply(remaining, currentScript);
            }

            rest.Do(remaining => this.Logger.Write(new ErrorParsingLine(remaining)));

            return rest.Reverse(script).Reduce(None.Value);
        }

        private Option<IPattern> TryMatch(Text content) =>
            this.Patterns.FirstOrNone(pattern => pattern.StartsWith.IsMatch(content.CurrentLine));
    }
}
