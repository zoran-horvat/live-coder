using System.Collections.Generic;

namespace LiveCoder.Extension.Interfaces
{
    internal interface IProject
    {
        IEnumerable<ISource> SourceFiles { get; }
        IEnumerable<IDemoStep> DemoSteps { get; }
    }
}