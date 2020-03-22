using System.Collections.Generic;

namespace LiveCoder.Api
{
    public interface IProject
    {
        IEnumerable<ISource> SourceFiles { get; }
    }
}