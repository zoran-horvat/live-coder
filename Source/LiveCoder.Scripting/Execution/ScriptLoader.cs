using System.IO;
using LiveCoder.Common.Optional;
using LiveCoder.Scripting.Snippets;

namespace LiveCoder.Scripting.Execution
{
    class ScriptLoader
    {
        public CodeSnippets For(DirectoryInfo liveCoderDirectory, IScriptingAuditor auditor) =>
            this.InSolutionDirectory(liveCoderDirectory, auditor).Reduce(CodeSnippets.Empty);

        private Option<CodeSnippets> InSolutionDirectory(DirectoryInfo directory, IScriptingAuditor auditor) =>
            directory.GetFiles("*.lcs")
                .FirstOrNone()
                .MapOptional(file => this.From(file, auditor));

        private Option<CodeSnippets> From(FileInfo file, IScriptingAuditor auditor) =>
            CodeSnippets.TryParse(file, auditor);
    }
}
