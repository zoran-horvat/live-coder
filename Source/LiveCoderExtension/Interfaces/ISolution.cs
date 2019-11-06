using System.Collections.Generic;

namespace LiveCoderExtension.Interfaces
{
    interface ISolution
    {
        IEnumerable<IProject> Projects { get; }
        IEnumerable<IDemoStep> DemoStepsOrdered { get; }
    }
}
