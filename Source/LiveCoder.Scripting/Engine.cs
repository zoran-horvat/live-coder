using System.IO;
using LiveCoder.Api;
using LiveCoder.Snippets;

namespace LiveCoder.Scripting
{
    public static class Engine
    {
        public static IEngine Create(ISolution solution, DirectoryInfo liveCoderDirectory, ILogger logger) =>
            CodeSnippetsEngine
                .TryCreate(solution, liveCoderDirectory, logger)
                .Reduce(() => new NoScriptEngine(logger));

    }
}