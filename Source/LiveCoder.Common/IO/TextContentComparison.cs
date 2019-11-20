using System.IO;
using System.Text;
using System.Linq;

namespace LiveCoder.Common.IO
{
    public static class TextContentComparison
    {
        public static bool IsContentModified(this FileInfo file, string[] lines, Encoding encoding) =>
            file is null || !File.Exists(file.FullName) || IsContentModifiedExisting(file, lines, encoding);

        public static bool RewriteIfModified(this FileInfo file, string[] lines, Encoding encoding)
        {
            if (file is null || !file.IsContentModified(lines, encoding)) return false;

            file.Directory?.Create();
            File.WriteAllLines(file.FullName, lines, encoding);
            return true;
        }


        private static bool IsContentModifiedExisting(FileInfo file, string[] lines, Encoding encoding) =>
            file.TryReadAllLines(encoding)
                .Map(fileLines => !lines.SequenceEqual(fileLines))
                .Reduce(false);
    }
}
