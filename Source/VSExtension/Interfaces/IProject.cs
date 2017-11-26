using System.Collections.Generic;

namespace VSExtension.Interfaces
{
    internal interface IProject
    {
        IEnumerable<ISource> SourceFiles { get; }
        IEnumerable<IDemoStep> DemoSteps { get; }
    }
}