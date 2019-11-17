using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiveCoder.Deployer.Implementation.Artifacts;

namespace LiveCoder.Deployer
{
    public abstract class Artifact
    {
        public static Artifact For(FileInfo source, FileInfo destination) => 
            IsVisualStudioSolution(destination) ? VisualStudioSolution.At(destination)
            : IsSlidesFile(destination) ? Slides.At(destination)
            : new CommonFile(destination);

        public static IEnumerable<Artifact> ManyFor(FileInfo source, FileInfo destination) =>
            new[] {For(source, destination)};

        private static bool IsVisualStudioSolution(FileInfo file) =>
            IsExtension(file, ".sln");

        private static bool IsSlidesFile(FileInfo file) =>
            IsExtension(file, ".pptx", ".ppt");

        private static bool IsExtension(FileInfo file, params string[] extensions) =>
            extensions.Any(extension => extension.Equals(file.Extension, StringComparison.OrdinalIgnoreCase));
    }
}