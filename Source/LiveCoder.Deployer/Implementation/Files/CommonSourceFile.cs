using System;
using System.Collections.Generic;
using System.IO;

namespace LiveCoder.Deployer.Implementation.Files
{
    class CommonSourceFile : SourceFile
    {
        public CommonSourceFile(IAuditor auditor, FileInfo location) : base(auditor, location)
        {
        }

        protected override IEnumerable<Artifact> Deploy(IAuditor auditor, FileInfo source, Directories to) => 
            this.Deploy(source, this.GetDestinationFile(source, to));

        private IEnumerable<Artifact> Deploy(FileInfo source, FileInfo destination)
        {
            source.CopyTo(destination?.FullName ?? throw new ArgumentNullException(nameof(destination)));
            return Artifact.ManyFor(source, destination);
        }

        private FileInfo GetDestinationFile(FileInfo source, Directories to) =>
            new FileInfo(Path.Combine(to.DestinationFor(source.Directory).FullName, source.Name));
    }
}