using System.Collections.Generic;
using System.IO;

namespace LiveCoder.Scripting.Interfaces
{
    public interface ISolution
    {
        FileInfo File { get; }
        IEnumerable<IProject> Projects { get; }
        IEnumerable<IDemoStep> GetDemoStepsOrdered(DemoScript script);
    }
}
