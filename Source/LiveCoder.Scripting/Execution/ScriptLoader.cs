using System.IO;
using LiveCoder.Common.Optional;
using LiveCoder.Scripting.Interfaces;

namespace LiveCoder.Scripting.Execution
{
    class ScriptLoader
    {
        public DemoScript For(ISolution solution, IScriptingAuditor auditor) =>
            this.InSolutionDirectory(solution.File.Directory, auditor).Reduce(DemoScript.Empty);

        private Option<DemoScript> InSolutionDirectory(DirectoryInfo directory, IScriptingAuditor auditor) =>
            directory.GetDirectories(".livecoder")
                .FirstOrNone()
                .MapOptional(dir => this.In(dir, auditor));

        private Option<DemoScript> In(DirectoryInfo directory, IScriptingAuditor auditor) =>
            directory.GetFiles("*.lcs")
                .FirstOrNone()
                .MapOptional(file => this.From(file, auditor));

        private Option<DemoScript> From(FileInfo file, IScriptingAuditor auditor) =>
            DemoScript.TryParse(file, auditor);
    }
}
