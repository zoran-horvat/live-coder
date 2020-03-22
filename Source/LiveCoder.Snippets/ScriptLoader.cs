using System.IO;
using LiveCoder.Api;
using LiveCoder.Common.Optional;

namespace LiveCoder.Snippets
{
    class ScriptLoader
    {
        public CodeSnippets For(DirectoryInfo liveCoderDirectory, ILogger logger) =>
            this.InSolutionDirectory(liveCoderDirectory, logger).Reduce(CodeSnippets.Empty);

        private Option<CodeSnippets> InSolutionDirectory(DirectoryInfo directory, ILogger logger) =>
            directory.GetFiles("*.lcs")
                .FirstOrNone()
                .MapOptional(file => this.From(file, logger));

        private Option<CodeSnippets> From(FileInfo file, ILogger logger) =>
            CodeSnippets.TryParse(file, logger);
    }
}
