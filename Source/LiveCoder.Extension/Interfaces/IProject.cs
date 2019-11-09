using System.Collections.Generic;
using LiveCoder.Extension.Scripting;

namespace LiveCoder.Extension.Interfaces
{
    internal interface IProject
    {
        IEnumerable<ISource> SourceFiles { get; }
        IEnumerable<IDemoStep> GetDemoSteps(DemoScript script);
    }
}