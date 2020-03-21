using System.Collections.Generic;

namespace LiveCoder.Scripting.Interfaces
{
    public interface IProject
    {
        IEnumerable<ISource> SourceFiles { get; }
        IEnumerable<IDemoStep> GetDemoSteps(DemoScript script);
    }
}