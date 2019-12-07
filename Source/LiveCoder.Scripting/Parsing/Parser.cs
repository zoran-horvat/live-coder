using System.Collections.Generic;
using System.Linq;
using LiveCoder.Common.Optional;
using LiveCoder.Scripting.Lexing;
using LiveCoder.Scripting.Lexing.Lexemes;
using LiveCoder.Scripting.Parsing.Tree;

namespace LiveCoder.Scripting.Parsing
{
    public class Parser
    {
        public ScriptNode Parse(TokensArray tokens) =>
            tokens.Any() ? this.ParseNonEmptyTree(tokens)
            : Tree.ScriptNode.Empty;

        private ScriptNode ParseNonEmptyTree(TokensArray tokens)
        {
            GrammarRules grammar = new GrammarRules();
            ParsingTable table = new ParsingTable(grammar);
            ParsingStack stack = new ParsingStack(table.Goto);

            using (IEnumerator<Token> input = tokens.GetAll().Concat(new[] {new EndOfInput()}).GetEnumerator())
            {
                bool processInput = input.MoveNext();
                while (processInput)
                {
                    if (stack.AcceptedResult is Some<ScriptNode> script)
                        return script.Content;

                    processInput = table
                        .Shift(stack.StateIndex, input.Current)
                        .Map(nextState => stack.Shift(input, nextState))
                        .Reduce(() => table.Reduction(stack.StateIndex, input.Current).Map(stack.Reduce).Reduce(false));
                }
            }

            return ScriptNode.Empty;
        }
    }
}
