using System.Collections.Generic;
using LiveCoder.Scripting;

namespace LiveCoder.Extension.Interfaces
{
    internal interface IProject
    {
        IEnumerable<ISource> SourceFiles { get; }
        IEnumerable<IDemoStep> GetDemoSteps(DemoScript script);
    }
}