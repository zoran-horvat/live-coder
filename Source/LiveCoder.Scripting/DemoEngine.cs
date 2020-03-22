using System.IO;
using LiveCoder.Scripting.Execution;
using LiveCoder.Scripting.Snippets;

namespace LiveCoder.Scripting
{
    public class DemoEngine
    {
        private CodeSnippets Script { get; }

        private DemoEngine(CodeSnippets script)
        {
            this.Script = script;
        }

        public static DemoEngine For(DirectoryInfo liveCoderDirectory, IScriptingAuditor auditor) =>
            new DemoEngine(new ScriptLoader().For(liveCoderDirectory, auditor));
    }
}
