using System.IO;
using LiveCoder.Scripting.Execution;
using LiveCoder.Scripting.Snippets;

namespace LiveCoder.Scripting
{
    class ScriptingEngine
    {
        private CodeSnippets Script { get; }

        private ScriptingEngine(CodeSnippets script)
        {
            this.Script = script;
        }

        public static ScriptingEngine For(DirectoryInfo liveCoderDirectory, IScriptingAuditor auditor) =>
            new ScriptingEngine(new ScriptLoader().For(liveCoderDirectory, auditor));
    }
}
