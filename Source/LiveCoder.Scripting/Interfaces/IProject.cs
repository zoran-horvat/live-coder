using System.Collections.Generic;
using LiveCoder.Scripting.Snippets;

namespace LiveCoder.Scripting.Interfaces
{
    public interface IProject
    {
        IEnumerable<ISource> SourceFiles { get; }
        IEnumerable<IDemoStep> GetDemoSteps(CodeSnippets script);
    }
}