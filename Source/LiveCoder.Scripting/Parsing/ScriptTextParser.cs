using System.Collections.Generic;
using LiveCoder.Common.Optional;
using LiveCoder.Scripting.Lexing;
using LiveCoder.Scripting.Parsing.Patterns;
using LiveCoder.Scripting.Text;

namespace LiveCoder.Scripting.Parsing
{
    class ScriptTextParser
    {
        private IScriptingAuditor Auditor { get; }
        private IEnumerable<IPattern> Patterns { get; }

        public ScriptTextParser(IScriptingAuditor auditor)
        {
            this.Auditor = auditor;
            this.Patterns = new IPattern[]
            {
                new Snippet(), 
                new BlankLine(), 
            };
        }

        public Option<DemoScript> TryParse(NonEmptyText content)
        {
            Option<DemoScript> script = Option.Of(DemoScript.Empty);
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

            TokensArray tokens = rest
                .ObjectOfType<NonEmptyText>()
                .Map(new Lexer().Tokenize)
                .Reduce(TokensArray.Empty);

            return script;
        }

        private Option<IPattern> TryMatch(NonEmptyText content) =>
            this.Patterns.FirstOrNone(pattern => pattern.StartsWith.IsMatch(content.CurrentLine));
    }
}
