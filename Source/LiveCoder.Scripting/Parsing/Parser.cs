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
                            processInput = stack.Reduce(Reduce1_ScriptNode);
                            break;

                        case Identifier _ when stack.StateIndex == 2:
                        case EndOfInput _ when stack.StateIndex == 2:
                            processInput = stack.Reduce(Reduce2_GlobalExpression);
                            break;

                        case Identifier _ when stack.StateIndex == 3:
                        case EndOfInput _ when stack.StateIndex == 3:
                            processInput = stack.Reduce(Reduce4_Reference);
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
                            processInput = stack.Reduce(Reduce8_Identifier);
                            break;

                        case Identifier _ when stack.StateIndex == 11:
                        case EndOfInput _ when stack.StateIndex == 11:
                            processInput = stack.Reduce(Reduce3_ScriptNode_GlobalExpression);
                            break;

                        case Identifier _ when stack.StateIndex == 12:
                        case EndOfInput _ when stack.StateIndex == 12:
                            processInput = stack.Reduce(Reduce5_Reference_LocalExpression);
                            break;
                        case Operator op when op.Value == "." && stack.StateIndex == 12:
                            processInput = stack.Shift(input, 16);
                            break;

                        case Identifier _ when stack.StateIndex == 13:
                        case Operator op when op.Value == "." && stack.StateIndex == 13:
                        case EndOfInput _ when stack.StateIndex == 13:
                            processInput = stack.Reduce(Reduce6_Dot_Reference);
                            break;

                        case Identifier _ when stack.StateIndex == 16:
                            processInput = stack.Shift(input, 6);
                            break;

                        case Identifier _ when stack.StateIndex == 20:
                        case Operator op when op.Value == "." && stack.StateIndex == 20:
                        case EndOfInput _ when stack.StateIndex == 20:
                            processInput = stack.Reduce(Reduce7_LocalExpression_Dot_Reference);
                            break;
                        default:
                            processInput = false;
                            break;
                    }
                }
            }

            return ScriptNode.Empty;
        }

        private bool Reduce(Func<Stack<object>, Node> reduction, Stack<object> parsingStack)
        {
            Node nonTerminal = reduction(parsingStack);
            int stateIndex = (int) parsingStack.Peek();
            parsingStack.Push(nonTerminal);

            int nextStateIndex = this.Goto(stateIndex, nonTerminal);
            if (nextStateIndex < 0)
                return false;

            parsingStack.Push(nextStateIndex);
            return true;
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

        private ScriptRoot Reduce1_ScriptNode(ParsingStack stack) =>
            new ScriptRoot(stack.Pop<ScriptNode>());

        private ScriptNode Reduce2_GlobalExpression(ParsingStack stack) =>
            ScriptNode.Empty.Append(stack.Pop<GlobalExpression>());

        private ScriptNode Reduce3_ScriptNode_GlobalExpression(ParsingStack stack) =>
            this.Reduce3_ScriptNode_GlobalExpression(stack.Pop<ScriptNode, GlobalExpression>());

        private ScriptNode Reduce3_ScriptNode_GlobalExpression((ScriptNode script, GlobalExpression globalExpression) tuple) =>
            tuple.script.Append(tuple.globalExpression);

        private GlobalExpression Reduce4_Reference(ParsingStack stack) => 
            GlobalExpression.Empty.Append(stack.Pop<Reference>());

        private GlobalExpression Reduce5_Reference_LocalExpression(ParsingStack stack) =>
            this.Reduce5_Reference_LocalExpression(stack.Pop<Reference, LocalExpression>());

        private GlobalExpression Reduce5_Reference_LocalExpression((Reference reference, LocalExpression localExpression) tuple) =>
            GlobalExpression.Empty.Append(tuple.reference).Append(tuple.localExpression);

        private LocalExpression Reduce6_Dot_Reference(ParsingStack stack) =>
            this.Reduce6_Dot_Reference(stack.Pop<Operator, Reference>());

        private LocalExpression Reduce6_Dot_Reference((Operator dot, Reference reference) tuple) =>
            LocalExpression.Empty.Append(tuple.reference);

        private LocalExpression Reduce7_LocalExpression_Dot_Reference(ParsingStack stack) =>
            this.Reduce7_LocalExpression_Dot_Reference(stack.Pop<LocalExpression, Operator, Reference>());

        private LocalExpression Reduce7_LocalExpression_Dot_Reference((LocalExpression localExpression, Operator dot, Reference reference) tuple) =>
            tuple.localExpression.Append(tuple.reference);

        private Reference Reduce8_Identifier(ParsingStack stack) => 
            new AttributeReference(stack.Pop<Identifier>());
    }
}
