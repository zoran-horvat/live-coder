using System.IO;
using System.Text;
using System.Linq;

namespace LiveCoder.Common.IO
{
    public static class TextContentComparison
    {
        public static bool IsContentModified(this FileInfo file, string[] lines, Encoding encoding) =>
            !File.Exists(file.FullName) || IsContentModifiedExisting(file, lines, encoding);

        private static bool IsContentModifiedExisting(FileInfo file, string[] lines, Encoding encoding) =>
            file.TryReadAllLines(encoding)
                .Map(fileLines => !lines.SequenceEqual(fileLines))
                .Reduce(false);
    }
}
