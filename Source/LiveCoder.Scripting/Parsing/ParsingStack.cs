using System;
using System.Collections.Generic;
using System.Linq;
using LiveCoder.Common.Optional;
using LiveCoder.Scripting.Lexing;
using LiveCoder.Scripting.Parsing.Tree;

namespace LiveCoder.Scripting.Parsing
{
    class ParsingStack
    {
        private Stack<object> Content { get; }
        private Func<int, Node, int> GotoTable { get; }

        public ParsingStack(Func<int, Node, int> gotoTable)
        {
            this.Content = new Stack<object>();
            this.Content.Push(0);
            this.GotoTable = gotoTable;
        }

        public int StateIndex =>
            (int) this.Content.Peek();

        public Option<ScriptNode> AcceptedResult =>
            this.StateIndex == 0 && this.Content.Count > 1 && this.Content.ElementAt(1) is ScriptRoot root ? Option.Of(root.Script)
            : None.Value;

        public bool Shift(IEnumerator<Token> input, int nextState)
        {
            this.Content.Push(input.Current);
            this.Content.Push(nextState);
            return input.MoveNext();
        }

        public bool Reduce(Func<Node> reduction)
        {
            Node nonTerminal = reduction();
            int stateIndex = this.StateIndex;
            this.Content.Push(nonTerminal);

            int nextStateIndex = this.GotoTable(stateIndex, nonTerminal);
            if (nextStateIndex < 0)
                return false;

            this.Content.Push(nextStateIndex);
            return true;
        }

        public TNode Pop<TNode>() where TNode : class
        {
            this.Content.Pop();
            return this.Content.Pop() as TNode;
        }

        public (TNode1, TNode2) Pop<TNode1, TNode2>() 
            where TNode1 : class 
            where TNode2 : class
        {
            TNode2 node2 = this.Pop<TNode2>();
            TNode1 node1 = this.Pop<TNode1>();
            return (node1, node2);
        }

        public (TNode1, TNode2, TNode3) Pop<TNode1, TNode2, TNode3>()
            where TNode1 : class
            where TNode2 : class
            where TNode3 : class
        {
            TNode3 node3 = this.Pop<TNode3>();
            TNode2 node2 = this.Pop<TNode2>();
            TNode1 node1 = this.Pop<TNode1>();
            return (node1, node2, node3);
        }
    }
}
