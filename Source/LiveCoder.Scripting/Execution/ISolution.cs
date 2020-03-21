using System.IO;
using LiveCoder.Common.Optional;

namespace LiveCoder.Scripting.Execution
{
    public interface ISolution1
    {
        Option<FileInfo> File { get; }
    }
}
