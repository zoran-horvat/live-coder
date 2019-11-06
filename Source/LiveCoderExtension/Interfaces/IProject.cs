using System.Collections.Generic;

namespace LiveCoderExtension.Interfaces
{
    internal interface IProject
    {
        IEnumerable<ISource> SourceFiles { get; }
        IEnumerable<IDemoStep> DemoSteps { get; }
    }
}