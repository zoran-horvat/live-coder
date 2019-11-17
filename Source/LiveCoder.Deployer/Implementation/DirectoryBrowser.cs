using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiveCoder.Common.Optional;

namespace LiveCoder.Deployer.Implementation
{
    class DirectoryBrowser
    {
        private string[] SkipNames { get; } = {"bin", "obj", "Release", "Debug"};
        private string[] SkipPrefixes { get; } = {"."};
        private string[] SkipFileExtensions { get; } = {".suo", ".bak"};

        private DirectoryInfo Directory { get; }
     
        private DirectoryBrowser(DirectoryInfo directory)
        {
            this.Directory = directory;
        }

        public static DirectoryBrowser For(DirectoryInfo directory) =>
            new DirectoryBrowser(directory);

        public IEnumerable<FileInfo> GetAllFiles() =>
            this.GetAllDirectories()
                .SelectMany(dir => dir.GetFiles())
                .Where(IsAcceptableFileExtension);

        private bool IsAcceptableFileExtension(FileInfo file) =>
            !this.SkipFileExtensions.Contains(file.Extension, StringComparer.OrdinalIgnoreCase);

        private IEnumerable<DirectoryInfo> SubdirectoriesOf(DirectoryInfo directory) =>
            this.Filter(directory)
                .Map(SubdirectoriesOfValid)
                .Reduce(Enumerable.Empty<DirectoryInfo>());

        private IEnumerable<DirectoryInfo> SubdirectoriesOfValid(DirectoryInfo directory) =>
            new[] {directory}
                .Concat(directory.GetDirectories().SelectMany(SubdirectoriesOf));

        private IEnumerable<DirectoryInfo> GetAllDirectories() =>
            this.SubdirectoriesOf(this.Directory);

        private Option<DirectoryInfo> Filter(DirectoryInfo directory) =>
            this.IsInSkipNames(directory) ? Option.None<DirectoryInfo>()
            : this.BeginsWithSkipPrefix(directory) ? Option.None<DirectoryInfo>()
            : directory;

        private bool IsInSkipNames(DirectoryInfo directory) =>
            this.SkipNames.Contains(directory.Name, StringComparer.OrdinalIgnoreCase);

        private bool BeginsWithSkipPrefix(DirectoryInfo directory) =>
            this.SkipPrefixes.Any(prefix => directory.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
    }
}
