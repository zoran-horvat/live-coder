using System.Collections.Generic;
using System.IO;

namespace LiveCoder.Deployer.Implementation.Files
{
    class InternalSourceFile : SourceFile
    {
        public InternalSourceFile(FileInfo location) : base(location)
        {
        }

        protected override IEnumerable<Artifact> Deploy(FileInfo source, Directories directories) =>
            this.Deploy(source, this.GetDestination(source, directories));

        private IEnumerable<Artifact> Deploy(FileInfo source, FileInfo destination)
        {
            source.CopyTo(destination.FullName);
            return Artifact.ManyFor(source, destination);
        }

        private FileInfo GetDestination(FileInfo source, Directories directories) =>
            new FileInfo(Path.Combine(directories.InternalDestination.FullName, source.Name));
    }
}