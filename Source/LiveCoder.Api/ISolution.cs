using System.Collections.Generic;

namespace LiveCoder.Api
{
    public interface ISolution
    {
        IEnumerable<IProject> Projects { get; }
    }
}
