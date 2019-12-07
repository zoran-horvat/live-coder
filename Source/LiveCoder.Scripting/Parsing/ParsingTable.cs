using System;
using LiveCoder.Common.Optional;
using LiveCoder.Scripting.Lexing;
using LiveCoder.Scripting.Lexing.Lexemes;
using LiveCoder.Scripting.Parsing.Tree;

namespace LiveCoder.Scripting.Parsing
{
    class ParsingTable
    {
        private GrammarRules Grammar { get; }
        private (Type nodeType, int currentState, int nextState)[] GotoTable { get; }
        private (int currentState, Type tokenType, string value, Func<ParsingStack, Node> reduction)[] Reductions { get; }

        public ParsingTable(GrammarRules grammar)
        {
            this.Grammar = grammar;
            this.GotoTable = CreateGotoTable();
            this.Reductions = CreateReductions(grammar);
        }

        private static (Type nodeType, int currentState, int nextState)[] CreateGotoTable() =>
            new (Type nodeType, int currentState, int nextState)[]
            {
                (typeof(ScriptNode), 0, 1),
                (typeof(GlobalExpression), 0, 2),
                (typeof(LocalExpression), 0, 5),
                (typeof(Reference), 0, 3),
                (typeof(ScriptRoot), 0, 0),
                (typeof(GlobalExpression), 1, 11),
                (typeof(Reference), 1, 3),
                (typeof(LocalExpression), 3, 12),
                (typeof(Reference), 4, 13),
                (typeof(Reference), 16, 20),
            };

        private static (int currentState, Type tokenType, string value, Func<ParsingStack, Node> reduction)[] CreateReductions(GrammarRules grammar) =>
            new (int currentState, Type tokenType, string value, Func<ParsingStack, Node> reduction)[]
            {
                (1, typeof(EndOfInput), string.Empty, grammar.Reduce1_ScriptNode),
                (2, typeof(Identifier), string.Empty, grammar.Reduce2_GlobalExpression),
                (2, typeof(EndOfInput), string.Empty, grammar.Reduce2_GlobalExpression),
                (3, typeof(Identifier), string.Empty, grammar.Reduce4_Reference),
                (3, typeof(EndOfInput), string.Empty, grammar.Reduce4_Reference),
                (6, typeof(Identifier), string.Empty, grammar.Reduce8_Identifier),
                (6, typeof(Operator), ".", grammar.Reduce8_Identifier),
                (6, typeof(EndOfInput), string.Empty, grammar.Reduce8_Identifier),
                (11, typeof(Identifier), string.Empty, grammar.Reduce3_ScriptNode_GlobalExpression),
                (11, typeof(EndOfInput), string.Empty, grammar.Reduce3_ScriptNode_GlobalExpression),
                (12, typeof(Identifier), string.Empty, grammar.Reduce5_Reference_LocalExpression),
                (12, typeof(EndOfInput), string.Empty, grammar.Reduce5_Reference_LocalExpression),
                (13, typeof(Identifier), string.Empty, grammar.Reduce6_Dot_Reference),
                (13, typeof(Operator), ".", grammar.Reduce6_Dot_Reference),
                (13, typeof(EndOfInput), string.Empty, grammar.Reduce6_Dot_Reference),
                (20, typeof(Identifier), string.Empty, grammar.Reduce7_LocalExpression_Dot_Reference),
                (20, typeof(Operator), ".", grammar.Reduce7_LocalExpression_Dot_Reference),
                (20, typeof(EndOfInput), string.Empty, grammar.Reduce7_LocalExpression_Dot_Reference),
            };

        public Option<int> Goto(Node nonTerminal, int currentState) =>
            nonTerminal.ObjectOfType<Node>()
                .Map(node => node.GetType())
                .MapOptional(nodeType => this.Goto(nodeType, currentState));

        private Option<int> Goto(Type nodeType, int currentState) =>
            this.GotoTable
                .FirstOrNone(record => record.nodeType.IsAssignableFrom(nodeType) && record.currentState == currentState)
                .Map(record => record.nextState);

        public Option<Func<ParsingStack, Node>> Reduction(int currentState, Token input) =>
            input.ObjectOfType<Token>()
                .MapOptional(token => this.Reduction(currentState, token.GetType(), token.Value));

        private Option<Func<ParsingStack, Node>> Reduction(int currentState, Type tokenType, string value) =>
            this.Reductions
                .FirstOrNone(reduction =>
                    reduction.currentState == currentState &&
                    reduction.tokenType.IsAssignableFrom(tokenType) &&
                    (string.IsNullOrWhiteSpace(reduction.value) || reduction.value == value))
                .Map(tuple => tuple.reduction);
    }
}
