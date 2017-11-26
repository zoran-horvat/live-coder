using System.Collections.Generic;

namespace VSExtension.Interfaces
{
    internal interface ISource
    {
        string Name { get; }
        IEnumerable<IDemoStep> DemoSteps { get; }
    }
}