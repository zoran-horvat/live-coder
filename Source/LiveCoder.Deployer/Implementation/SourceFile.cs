using System;
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

        public static SourceFile From(FileInfo location) =>
            IsXmlSnippets(location) ? new XmlSnippetsFile(location)
            : IsSlidesFile(location) ? (SourceFile)new InternalSourceFile(location)
            : new CommonSourceFile(location);

        public void Deploy(Directories directories) =>
            this.Deploy(this.Location, directories);

        protected abstract void Deploy(FileInfo source, Directories to);

        private static bool IsXmlSnippets(FileInfo location) =>
            location.Extension.Equals(".snippet", StringComparison.OrdinalIgnoreCase);

        private static bool IsSlidesFile(FileInfo location) =>
            new[] {".ppt", ".pptx"}.Contains(location.Extension, StringComparer.OrdinalIgnoreCase);

        public override string ToString() =>
            $"{this.GetType().Name} {this.Location.FullName}";
    }
}
