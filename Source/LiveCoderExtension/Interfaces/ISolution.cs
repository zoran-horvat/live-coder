using System.Collections.Generic;
using System.IO;
using Common.Optional;

namespace LiveCoderExtension.Interfaces
{
    interface ISolution
    {
        IEnumerable<IProject> Projects { get; }
        IEnumerable<IDemoStep> DemoStepsOrdered { get; }
        Option<FileInfo> SolutionFile { get; }
    }
}
