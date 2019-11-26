using System.Collections.Generic;
using LiveCoder.Scripting;

namespace LiveCoder.Extension.Interfaces
{
    interface ISolution : Scripting.Execution.ISolution
    {
        IEnumerable<IProject> Projects { get; }
        IEnumerable<IDemoStep> GetDemoStepsOrdered(DemoScript script);
    }
}
