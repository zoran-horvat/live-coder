using System.Collections.Generic;
using System.IO;
using LiveCoder.Common.Optional;

namespace LiveCoder.Scripting.Interfaces
{
    public interface ISolution
    {
        Option<FileInfo> File { get; }
        IEnumerable<IProject> Projects { get; }
        IEnumerable<IDemoStep> GetDemoStepsOrdered(DemoScript script);
    }
}
