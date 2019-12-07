using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LiveCoder.Common.Optional;
using LiveCoder.Scripting.Lexing;
using LiveCoder.Scripting.Lexing.Lexemes;
using LiveCoder.Scripting.Parsing.Instructions;
using LiveCoder.Scripting.Parsing.Tree;

namespace LiveCoder.Scripting.Parsing
{
    public class Parser
    {
        public IEnumerable<Instruction> Parse(TokensArray tokens)
        {
            ScriptNode script = this.ParseTree(tokens);

            bool produced = false;
            do
            {
                produced = false;
                foreach ((Instruction instruction, TokensArray rest) tuple in this.ParseNext(tokens))
                {
                    yield return tuple.instruction;
                    tokens = tuple.rest;
                    produced = true;
                }
            } while (produced);
        }
            
        private IEnumerable<(Instruction instruction, TokensArray rest)> ParseNext(TokensArray tokens)
        {
            if (tokens.FirstOrNone() is Some<Token> some &&
                some.Content is Token firstToken &&
                firstToken != Operator.Of("."))
            {
                yield return (new SelectGlobalScope(), tokens);
            }

            if (tokens.FirstOrNone() is Some<Token> someIdentifier &&
                someIdentifier.Content is Identifier identifier)
            {
                yield return (new ResolveAttribute(), tokens.Next());
            }
        }

        public ScriptNode ParseTree(TokensArray tokens) =>
            tokens.Any() ? this.ParseNonEmptyTree(tokens)
            : Tree.ScriptNode.Empty;

        private ScriptNode ParseNonEmptyTree(TokensArray tokens)
        {
            Stack<object> parsingStack = new Stack<object>();
            parsingStack.Push(0);

            using (IEnumerator<Token> next = tokens.GetAll().Concat(new[] {new EndOfInput()}).GetEnumerator())
            {
                bool processInput = next.MoveNext();
                while (processInput)
                {
                    int stateIndex = (int) parsingStack.Peek();

                    if (stateIndex == 0 && parsingStack.Count > 1 && parsingStack.ElementAt(1) is ScriptRoot root)
                        return root.Script;

                    switch (next.Current)
                    {
                        case Identifier _ when stateIndex == 0:
                            processInput = this.Shift(parsingStack, next, 6);
                            break;
                        case Operator op when op.Value == "." && stateIndex == 0:
                            processInput = this.Shift(parsingStack, next, 4);
                            break;

                        case Identifier _ when stateIndex == 1:
                            processInput = this.Shift(parsingStack, next, 6);
                            break;
                        case EndOfInput _ when stateIndex == 1:
                            processInput = this.Reduce(Reduce1_ScriptNode, parsingStack);
                            break;

                        case Identifier _ when stateIndex == 2:
                        case EndOfInput _ when stateIndex == 2:
                            processInput = this.Reduce(Reduce2_GlobalExpression, parsingStack);
                            break;

                        case Identifier _ when stateIndex == 3:
                        case EndOfInput _ when stateIndex == 3:
                            processInput = this.Reduce(Reduce4_Reference, parsingStack);
                            break;
                        case Operator op when op.Value == "." && stateIndex == 3:
                            processInput = this.Shift(parsingStack, next, 4);
                            break;

                        case Identifier _ when stateIndex == 4:
                            processInput = this.Shift(parsingStack, next, 6);
                            break;

                        case Operator op when op.Value == "." && stateIndex == 5:
                            processInput = this.Shift(parsingStack, next, 16);
                            break;

                        case Identifier _ when stateIndex == 6:
                        case Operator op when op.Value == "." && stateIndex == 6:
                        case EndOfInput _ when stateIndex == 6:
                            processInput = this.Reduce(Reduce8_Identifier, parsingStack);
                            break;

                        case Identifier _ when stateIndex == 11:
                        case EndOfInput _ when stateIndex == 11:
                            processInput = this.Reduce(Reduce3_ScriptNode_GlobalExpression, parsingStack);
                            break;

                        case Identifier _ when stateIndex == 12:
                        case EndOfInput _ when stateIndex == 12:
                            processInput = this.Reduce(Reduce5_Reference_LocalExpression, parsingStack);
                            break;
                        case Operator op when op.Value == "." && stateIndex == 12:
                            processInput = this.Shift(parsingStack, next, 16);
                            break;

                        case Identifier _ when stateIndex == 13:
                        case Operator op when op.Value == "." && stateIndex == 13:
                        case EndOfInput _ when stateIndex == 13:
                            processInput = this.Reduce(Reduce6_Dot_Reference, parsingStack);
                            break;

                        case Identifier _ when stateIndex == 16:
                            processInput = this.Shift(parsingStack, next, 6);
                            break;

                        case Identifier _ when stateIndex == 20:
                        case Operator op when op.Value == "." && stateIndex == 20:
                        case EndOfInput _ when stateIndex == 20:
                            processInput = this.Reduce(Reduce7_LocalExpression_Dot_Reference, parsingStack);
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

        private ScriptRoot Reduce1_ScriptNode(Stack<object> parsingStack) =>
            new ScriptRoot(this.Pop<ScriptNode>(parsingStack));

        private ScriptNode Reduce2_GlobalExpression(Stack<object> parsingStack) =>
            ScriptNode.Empty.Append(this.Pop<GlobalExpression>(parsingStack));

        private ScriptNode Reduce3_ScriptNode_GlobalExpression(Stack<object> parsingStack) =>
            this.Reduce3_ScriptNode_GlobalExpression(this.Pop<ScriptNode, GlobalExpression>(parsingStack));

        private ScriptNode Reduce3_ScriptNode_GlobalExpression((ScriptNode script, GlobalExpression globalExpression) tuple) =>
            tuple.script.Append(tuple.globalExpression);

        private GlobalExpression Reduce4_Reference(Stack<object> parsingStack) => 
            GlobalExpression.Empty.Append(this.Pop<Reference>(parsingStack));

        private GlobalExpression Reduce5_Reference_LocalExpression(Stack<object> parsingStack) =>
            this.Reduce5_Reference_LocalExpression(this.Pop<Reference, LocalExpression>(parsingStack));

        private GlobalExpression Reduce5_Reference_LocalExpression((Reference reference, LocalExpression localExpression) tuple) =>
            GlobalExpression.Empty.Append(tuple.reference).Append(tuple.localExpression);

        private LocalExpression Reduce6_Dot_Reference(Stack<object> parsingStack) =>
            this.Reduce6_Dot_Reference(this.Pop<Operator, Reference>(parsingStack));

        private LocalExpression Reduce6_Dot_Reference((Operator dot, Reference reference) tuple) =>
            LocalExpression.Empty.Append(tuple.reference);

        private LocalExpression Reduce7_LocalExpression_Dot_Reference(Stack<object> parsingStack) =>
            this.Reduce7_LocalExpression_Dot_Reference(this.Pop<LocalExpression, Operator, Reference>(parsingStack));

        private LocalExpression Reduce7_LocalExpression_Dot_Reference((LocalExpression localExpression, Operator dot, Reference reference) tuple) =>
            tuple.localExpression.Append(tuple.reference);

        private Reference Reduce8_Identifier(Stack<object> parsingStack) => 
            new AttributeReference(this.Pop<Identifier>(parsingStack));

        private TNode Pop<TNode>(Stack<object> parsingStack) where TNode : class
        {
            parsingStack.Pop();
            return parsingStack.Pop() as TNode;
        }

        private (TNode1, TNode2) Pop<TNode1, TNode2>(Stack<object> parsingStack) 
            where TNode1 : class 
            where TNode2 : class
        {
            TNode2 node2 = this.Pop<TNode2>(parsingStack);
            TNode1 node1 = this.Pop<TNode1>(parsingStack);
            return (node1, node2);
        }

        private (TNode1, TNode2, TNode3) Pop<TNode1, TNode2, TNode3>(Stack<object> parsingStack)
            where TNode1 : class
            where TNode2 : class
            where TNode3 : class
        {
            TNode3 node3 = this.Pop<TNode3>(parsingStack);
            TNode2 node2 = this.Pop<TNode2>(parsingStack);
            TNode1 node1 = this.Pop<TNode1>(parsingStack);
            return (node1, node2, node3);
        }

        private bool Shift(Stack<object> parsingStack, IEnumerator<Token> symbol, int nextState)
        {
            parsingStack.Push(symbol.Current);
            parsingStack.Push(nextState);
            return symbol.MoveNext();
        }
    }
}
