using System.Collections.Generic;
using System.IO;
using LiveCoder.Scripting.Snippets;

namespace LiveCoder.Scripting.Interfaces
{
    public interface ISolution
    {
        FileInfo File { get; }
        IEnumerable<IProject> Projects { get; }
        IEnumerable<IDemoStep> GetDemoStepsOrdered(CodeSnippets script);
    }
}
