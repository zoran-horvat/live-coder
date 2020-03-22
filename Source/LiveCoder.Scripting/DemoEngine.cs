using LiveCoder.Scripting.Execution;
using LiveCoder.Scripting.Interfaces;
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

        public static DemoEngine For(ISolution solution, IScriptingAuditor auditor) =>
            new DemoEngine(new ScriptLoader().For(solution, auditor));
    }
}
