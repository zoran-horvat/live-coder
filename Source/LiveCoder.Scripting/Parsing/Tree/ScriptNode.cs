using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using LiveCoder.Common;

namespace LiveCoder.Scripting.Parsing.Tree
{
    public class ScriptNode : Node, IEnumerable<GlobalExpression>
    {
        private ImmutableList<GlobalExpression> Expressions { get; }

        public static ScriptNode Empty => 
            new ScriptNode(ImmutableList<GlobalExpression>.Empty);

        private ScriptNode(ImmutableList<GlobalExpression> expressions)
        {
            this.Expressions = expressions;
        }

        public ScriptNode Append(GlobalExpression expression) =>
            new ScriptNode(this.Expressions.Add(expression));

        public IEnumerator<GlobalExpression> GetEnumerator() =>
            this.Expressions.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => 
            GetEnumerator();

        public override string ToString() =>
            this.Expressions.Select(expr => expr.ToString()).Join(Environment.NewLine);
    }
}
