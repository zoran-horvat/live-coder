namespace LiveCoder.Scripting.Parsing.Tree
{
    class ScriptRoot : Node
    {
        public ScriptNode Script { get; }
        
        public static ScriptRoot Empty => 
            new ScriptRoot(ScriptNode.Empty);

        public ScriptRoot(ScriptNode script)
        {
            this.Script = script;
        }

        public override string ToString() =>
            this.Script.ToString();
    }
}
