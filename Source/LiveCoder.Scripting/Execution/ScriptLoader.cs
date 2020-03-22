using System.IO;
using LiveCoder.Common.Optional;
using LiveCoder.Scripting.Interfaces;
using LiveCoder.Scripting.Snippets;

namespace LiveCoder.Scripting.Execution
{
    class ScriptLoader
    {
        public CodeSnippets For(ISolution solution, IScriptingAuditor auditor) =>
            this.InSolutionDirectory(solution.File.Directory, auditor).Reduce(CodeSnippets.Empty);

        private Option<CodeSnippets> InSolutionDirectory(DirectoryInfo directory, IScriptingAuditor auditor) =>
            directory.GetDirectories(".livecoder")
                .FirstOrNone()
                .MapOptional(dir => this.In(dir, auditor));

        private Option<CodeSnippets> In(DirectoryInfo directory, IScriptingAuditor auditor) =>
            directory.GetFiles("*.lcs")
                .FirstOrNone()
                .MapOptional(file => this.From(file, auditor));

        private Option<CodeSnippets> From(FileInfo file, IScriptingAuditor auditor) =>
            CodeSnippets.TryParse(file, auditor);
    }
}
