using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiveCoder.Deployer.Implementation.Files;

namespace LiveCoder.Deployer.Implementation
{
    abstract class SourceFile
    {
        private IAuditor Auditor { get; }
        private FileInfo Location { get; }
     
        protected SourceFile(IAuditor auditor, FileInfo location)
        {
            this.Auditor = auditor;
            this.Location = location;
        }

        public static SourceFile From(IAuditor auditor, FileInfo location) =>
            IsXmlSnippets(location) ? new XmlSnippetsFile(auditor, location)
            : IsSlidesFile(location) ? (SourceFile)new InternalSourceFile(auditor, location)
            : new CommonSourceFile(auditor, location);

        public IEnumerable<Artifact> Deploy(Directories directories)
        {
            foreach (Artifact artifact in this.Deploy(this.Auditor, this.Location, directories))
            {
                this.Auditor.ComponentDeployed(artifact);
                yield return artifact;
            }
        }

        protected abstract IEnumerable<Artifact> Deploy(IAuditor auditor, FileInfo source, Directories to);

        private static bool IsXmlSnippets(FileInfo location) =>
            location.Extension.Equals(".snippet", StringComparison.OrdinalIgnoreCase);

        private static bool IsSlidesFile(FileInfo location) =>
            new[] {".ppt", ".pptx"}.Contains(location.Extension, StringComparer.OrdinalIgnoreCase);

        public override string ToString() =>
            $"{this.GetType().Name} {this.Location.FullName}";
    }
}
