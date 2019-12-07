using LiveCoder.Scripting.Lexing.Lexemes;
using LiveCoder.Scripting.Parsing.Tree;

namespace LiveCoder.Scripting.Parsing
{
    class GrammarRules
    {
        private ParsingStack Stack { get; }
     
        public GrammarRules(ParsingStack stack)
        {
            this.Stack = stack;
        }

        public ScriptRoot Reduce1_ScriptNode() =>
            new ScriptRoot(this.Stack.Pop<ScriptNode>());

        public ScriptNode Reduce2_GlobalExpression() =>
            ScriptNode.Empty.Append(this.Stack.Pop<GlobalExpression>());

        public ScriptNode Reduce3_ScriptNode_GlobalExpression() =>
            this.Reduce3_ScriptNode_GlobalExpression(this.Stack.Pop<ScriptNode, GlobalExpression>());

        private ScriptNode Reduce3_ScriptNode_GlobalExpression((ScriptNode script, GlobalExpression globalExpression) tuple) =>
            tuple.script.Append(tuple.globalExpression);

        public GlobalExpression Reduce4_Reference() => 
            GlobalExpression.Empty.Append(this.Stack.Pop<Reference>());

        public GlobalExpression Reduce5_Reference_LocalExpression() =>
            this.Reduce5_Reference_LocalExpression(this.Stack.Pop<Reference, LocalExpression>());

        private GlobalExpression Reduce5_Reference_LocalExpression((Reference reference, LocalExpression localExpression) tuple) =>
            GlobalExpression.Empty.Append(tuple.reference).Append(tuple.localExpression);

        public LocalExpression Reduce6_Dot_Reference() =>
            this.Reduce6_Dot_Reference(this.Stack.Pop<Operator, Reference>());

        private LocalExpression Reduce6_Dot_Reference((Operator dot, Reference reference) tuple) =>
            LocalExpression.Empty.Append(tuple.reference);

        public LocalExpression Reduce7_LocalExpression_Dot_Reference() =>
            this.Reduce7_LocalExpression_Dot_Reference(this.Stack.Pop<LocalExpression, Operator, Reference>());

        private LocalExpression Reduce7_LocalExpression_Dot_Reference((LocalExpression localExpression, Operator dot, Reference reference) tuple) =>
            tuple.localExpression.Append(tuple.reference);

        public Reference Reduce8_Identifier() => 
            new AttributeReference(this.Stack.Pop<Identifier>());
    }
}
