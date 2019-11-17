using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using LiveCoder.Deployer.Implementation.Files;

namespace LiveCoder.Deployer.Implementation
{
    abstract class SourceFile
    {
        private FileInfo Location { get; }
     
        protected SourceFile(FileInfo location)
        {
            this.Location = location;
        }

        public static SourceFile From(FileInfo location)
        {
            SourceFile file = 
                IsSlidesFile(location) ? (SourceFile)new InternalSourceFile(location)
                : new CommonSourceFile(location);

            Debug.WriteLine($"Preparing to deploy {file}");
            return file;
        }

        private static bool IsSlidesFile(FileInfo location) =>
            new[] {".ppt", ".pptx"}.Contains(location.Extension, StringComparer.OrdinalIgnoreCase);

        public override string ToString() =>
            $"{this.GetType().Name} {this.Location.FullName}";
    }
}
