using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace LiveCoder.Deployer.Implementation
{
    class DirectoryBrowser
    {
        private DirectoryInfo Directory { get; }
     
        private DirectoryBrowser(DirectoryInfo directory)
        {
            this.Directory = directory;
        }

        public static DirectoryBrowser For(DirectoryInfo directory) =>
            new DirectoryBrowser(directory);

        public IEnumerable<FileInfo> GetAllFiles() =>
            this.GetAllDirectories().SelectMany(dir => dir.GetFiles());

        private IEnumerable<DirectoryInfo> GetAllDirectories() =>
            this.SubdirectoriesOf(this.Directory);

        private IEnumerable<DirectoryInfo> SubdirectoriesOf(DirectoryInfo directory) =>
            new[] {directory}
                .Concat(directory.GetDirectories().SelectMany(SubdirectoriesOf));
    }
}
