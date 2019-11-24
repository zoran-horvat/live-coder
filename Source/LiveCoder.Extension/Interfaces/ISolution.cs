using System.Collections.Generic;
using System.IO;
using LiveCoder.Common.Optional;
using LiveCoder.Scripting;

namespace LiveCoder.Extension.Interfaces
{
    interface ISolution
    {
        IEnumerable<IProject> Projects { get; }
        IEnumerable<IDemoStep> GetDemoStepsOrdered(DemoScript script);
        Option<FileInfo> SolutionFile { get; }
    }
}
