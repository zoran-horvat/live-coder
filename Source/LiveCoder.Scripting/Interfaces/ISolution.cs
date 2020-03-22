using System.Collections.Generic;
using System.IO;

namespace LiveCoder.Scripting.Interfaces
{
    public interface ISolution
    {
        IEnumerable<IProject> Projects { get; }
    }
}
