using System.Collections.Generic;
using System.Linq;
using LiveCoder.Common;
using LiveCoder.Common.Optional;
using LiveCoder.Scripting.Lexing;
using LiveCoder.Scripting.Lexing.Lexemes;
using LiveCoder.Scripting.Parsing.Tree;

namespace LiveCoder.Scripting.Parsing
{
    public class Parser
    {
        private GrammarRules Grammar { get; }
        private ParsingTable Table { get; }

        public Parser()
        {
            this.Grammar = new GrammarRules();
            this.Table = new ParsingTable(this.Grammar);
        }

        public ScriptNode Parse(TokensArray tokens) =>
            tokens.Any() ? this.Parse(tokens.GetAll().Concat(new[] {new EndOfInput()}))
            : Tree.ScriptNode.Empty;

        private ScriptNode Parse(IEnumerable<Token> tokens) =>
            this.Parse(new ParsingStack(this.Table.Goto), tokens);

        private ScriptNode Parse(ParsingStack stack, IEnumerable<Token> tokens) =>
            Disposable.Using(tokens.GetEnumerator).Map(input => this.Parse(stack, input));

        private ScriptNode Parse(ParsingStack stack, IEnumerator<Token> input)
        {
            input.MoveNext();
            while (stack.AcceptedResult is None<ScriptNode> && this.Advance(stack, input)) { }
            return stack.AcceptedResult.Reduce(() => ScriptNode.Empty);
        }

        private bool Advance(ParsingStack stack, IEnumerator<Token> input) =>
            this.Shift(stack, input).Reduce(() => this.Reduce(stack, input));

        private Option<bool> Shift(ParsingStack stack, IEnumerator<Token> input) =>
            this.Table.Shift(stack.StateIndex, input.Current).Map(nextState => stack.Shift(input, nextState));

        private bool Reduce(ParsingStack stack, IEnumerator<Token> input) =>
            this.Table.Reduction(stack.StateIndex, input.Current).Map(stack.Reduce).Reduce(false);
    }
}
