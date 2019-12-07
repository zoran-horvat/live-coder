using LiveCoder.Scripting.Lexing.Lexemes;
using LiveCoder.Scripting.Parsing.Tree;

namespace LiveCoder.Scripting.Parsing
{
    class GrammarRules
    {
        public ScriptRoot Reduce1_ScriptNode(ParsingStack stack) =>
            new ScriptRoot(stack.Pop<ScriptNode>());

        public ScriptNode Reduce2_GlobalExpression(ParsingStack stack) =>
            ScriptNode.Empty.Append(stack.Pop<GlobalExpression>());

        public ScriptNode Reduce3_ScriptNode_GlobalExpression(ParsingStack stack) =>
            this.Reduce3_ScriptNode_GlobalExpression(stack.Pop<ScriptNode, GlobalExpression>());

        private ScriptNode Reduce3_ScriptNode_GlobalExpression((ScriptNode script, GlobalExpression globalExpression) tuple) =>
            tuple.script.Append(tuple.globalExpression);

        public GlobalExpression Reduce4_Reference(ParsingStack stack) => 
            GlobalExpression.Empty.Append(stack.Pop<Reference>());

        public GlobalExpression Reduce5_Reference_LocalExpression(ParsingStack stack) =>
            this.Reduce5_Reference_LocalExpression(stack.Pop<Reference, LocalExpression>());

        private GlobalExpression Reduce5_Reference_LocalExpression((Reference reference, LocalExpression localExpression) tuple) =>
            GlobalExpression.Empty.Append(tuple.reference).Append(tuple.localExpression);

        public LocalExpression Reduce6_Dot_Reference(ParsingStack stack) =>
            this.Reduce6_Dot_Reference(stack.Pop<Operator, Reference>());

        private LocalExpression Reduce6_Dot_Reference((Operator dot, Reference reference) tuple) =>
            LocalExpression.Empty.Append(tuple.reference);

        public LocalExpression Reduce7_LocalExpression_Dot_Reference(ParsingStack stack) =>
            this.Reduce7_LocalExpression_Dot_Reference(stack.Pop<LocalExpression, Operator, Reference>());

        private LocalExpression Reduce7_LocalExpression_Dot_Reference((LocalExpression localExpression, Operator dot, Reference reference) tuple) =>
            tuple.localExpression.Append(tuple.reference);

        public Reference Reduce8_Identifier(ParsingStack stack) => 
            new AttributeReference(stack.Pop<Identifier>());
    }
}
