using System.IO;
using LiveCoder.Common.Optional;

namespace LiveCoder.Scripting.Execution
{
    public interface ISolution
    {
        Option<FileInfo> File { get; }
    }
}
