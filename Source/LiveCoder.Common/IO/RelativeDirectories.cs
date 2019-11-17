using System;
using System.IO;

namespace LiveCoder.Common.IO
{
    public static class RelativeDirectories
    {
        public static string RelativeTo(this DirectoryInfo directory, DirectoryInfo parent) =>
            directory.RelativeTo(parent, directory, string.Empty);

        private static string RelativeTo(this DirectoryInfo directory, DirectoryInfo parent, DirectoryInfo originalChild, string currentRelativePath) =>
            directory is null ? throw new ArgumentException($"[{originalChild.FullName}] is not contained in [{parent.FullName}]")
            : directory.FullName.Equals(parent.FullName, StringComparison.OrdinalIgnoreCase) ? currentRelativePath
            : directory.Parent.RelativeTo(parent, originalChild, Path.Combine(directory.Name, currentRelativePath));
    }
}
