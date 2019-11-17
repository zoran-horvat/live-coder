using System;
using System.Diagnostics;
using System.IO;

namespace LiveCoder.Deployer.Implementation.Files
{
    class CommonSourceFile : SourceFile
    {
        public CommonSourceFile(FileInfo location) : base(location)
        {
        }

        protected override void Deploy(FileInfo source, Directories to) => 
            this.Deploy(source, this.GetDestinationFile(source, to));

        private void Deploy(FileInfo source, FileInfo destination)
        {
            destination?.Directory?.Create();
            source.CopyTo(destination?.FullName ?? throw new ArgumentNullException(nameof(destination)));
            Debug.WriteLine($"Deployed {this} to {destination.FullName}");
        }

        private FileInfo GetDestinationFile(FileInfo source, Directories to) =>
            new FileInfo(Path.Combine(to.DestinationFor(source.Directory).FullName, source.Name));
    }
}