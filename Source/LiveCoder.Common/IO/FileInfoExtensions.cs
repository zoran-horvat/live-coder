using System.IO;
using LiveCoder.Common.Optional;

namespace LiveCoder.Common.IO
{
    public static class FileInfoExtensions
    {
        public static Option<FileInfo> WhenExists(this FileInfo file) =>
            Option.Of(file).When(f=> f.Exists);
    }
}
