using LiveCoder.Scripting.Execution;

namespace LiveCoder.Scripting
{
    public class DemoEngine
    {
        private DemoScript Script { get; }

        private DemoEngine(DemoScript script)
        {
            this.Script = script;
        }

        public static DemoEngine For(IContext context, IScriptingAuditor auditor) =>
            new DemoEngine(new ScriptLoader().For(context.Solution, auditor));
    }
}
