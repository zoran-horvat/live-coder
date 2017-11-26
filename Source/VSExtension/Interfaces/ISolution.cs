using System.Collections.Generic;

namespace VSExtension.Interfaces
{
    interface ISolution
    {
        IEnumerable<IProject> Projects { get; }
        IEnumerable<IDemoStep> DemoStepsOrdered { get; }
    }
}
