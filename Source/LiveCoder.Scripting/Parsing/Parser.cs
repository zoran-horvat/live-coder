using System;
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
            ParsingStack stack = new ParsingStack(this.Goto);
            GrammarRules grammar = new GrammarRules(stack);

            using (IEnumerator<Token> input = tokens.GetAll().Concat(new[] {new EndOfInput()}).GetEnumerator())
            {
                bool processInput = input.MoveNext();
                while (processInput)
                {
                    if (stack.AcceptedResult is Some<ScriptNode> script)
                        return script.Content;

                    switch (input.Current)
                    {
                        case Identifier _ when stack.StateIndex == 0:
                            processInput = stack.Shift(input, 6);
                            break;
                        case Operator op when op.Value == "." && stack.StateIndex == 0:
                            processInput = stack.Shift(input, 4);
                            break;

                        case Identifier _ when stack.StateIndex == 1:
                            processInput = stack.Shift(input, 6);
                            break;
                        case EndOfInput _ when stack.StateIndex == 1:
                            processInput = stack.Reduce(grammar.Reduce1_ScriptNode);
                            break;

                        case Identifier _ when stack.StateIndex == 2:
                        case EndOfInput _ when stack.StateIndex == 2:
                            processInput = stack.Reduce(grammar.Reduce2_GlobalExpression);
                            break;

                        case Identifier _ when stack.StateIndex == 3:
                        case EndOfInput _ when stack.StateIndex == 3:
                            processInput = stack.Reduce(grammar.Reduce4_Reference);
                            break;
                        case Operator op when op.Value == "." && stack.StateIndex == 3:
                            processInput = stack.Shift(input, 4);
                            break;

                        case Identifier _ when stack.StateIndex == 4:
                            processInput = stack.Shift(input, 6);
                            break;

                        case Operator op when op.Value == "." && stack.StateIndex == 5:
                            processInput = stack.Shift(input, 16);
                            break;

                        case Identifier _ when stack.StateIndex == 6:
                        case Operator op when op.Value == "." && stack.StateIndex == 6:
                        case EndOfInput _ when stack.StateIndex == 6:
                            processInput = stack.Reduce(grammar.Reduce8_Identifier);
                            break;

                        case Identifier _ when stack.StateIndex == 11:
                        case EndOfInput _ when stack.StateIndex == 11:
                            processInput = stack.Reduce(grammar.Reduce3_ScriptNode_GlobalExpression);
                            break;

                        case Identifier _ when stack.StateIndex == 12:
                        case EndOfInput _ when stack.StateIndex == 12:
                            processInput = stack.Reduce(grammar.Reduce5_Reference_LocalExpression);
                            break;
                        case Operator op when op.Value == "." && stack.StateIndex == 12:
                            processInput = stack.Shift(input, 16);
                            break;

                        case Identifier _ when stack.StateIndex == 13:
                        case Operator op when op.Value == "." && stack.StateIndex == 13:
                        case EndOfInput _ when stack.StateIndex == 13:
                            processInput = stack.Reduce(grammar.Reduce6_Dot_Reference);
                            break;

                        case Identifier _ when stack.StateIndex == 16:
                            processInput = stack.Shift(input, 6);
                            break;

                        case Identifier _ when stack.StateIndex == 20:
                        case Operator op when op.Value == "." && stack.StateIndex == 20:
                        case EndOfInput _ when stack.StateIndex == 20:
                            processInput = stack.Reduce(grammar.Reduce7_LocalExpression_Dot_Reference);
                            break;
                        default:
                            processInput = false;
                            break;
                    }
                }
            }

            return ScriptNode.Empty;
        }

        private int Goto(int stateIndex, Node nonTerminal)
        {
            switch (nonTerminal)
            {
                case ScriptNode _ when stateIndex == 0: return 1;
                case GlobalExpression _ when stateIndex == 0: return 2;
                case LocalExpression _ when stateIndex == 0: return 5;
                case Reference _ when stateIndex == 0: return 3;
                case ScriptRoot _ when stateIndex == 0: return 0;
                case GlobalExpression _ when stateIndex == 1: return 11;
                case Reference _ when stateIndex == 1: return 3;
                case LocalExpression _ when stateIndex == 3: return 12;
                case Reference _ when stateIndex == 4: return 13;
                case Reference _ when stateIndex == 16: return 20;
            }

            return -1;
        }
    }
}
