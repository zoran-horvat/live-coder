using System.Collections.Generic;
using System.IO;
using LiveCoder.Common.Optional;

namespace LiveCoder.Extension.Interfaces
{
    interface ISolution
    {
        IEnumerable<IProject> Projects { get; }
        IEnumerable<IDemoStep> DemoStepsOrdered { get; }
        Option<FileInfo> SolutionFile { get; }
    }
}
