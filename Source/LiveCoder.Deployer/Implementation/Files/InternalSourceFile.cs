using System.Diagnostics;
using System.IO;

namespace LiveCoder.Deployer.Implementation.Files
{
    class InternalSourceFile : SourceFile
    {
        public InternalSourceFile(FileInfo location) : base(location)
        {
        }

        protected override void Deploy(FileInfo source, Directories directories) =>
            this.Deploy(source, this.GetDestination(source, directories));

        private void Deploy(FileInfo source, FileInfo destination)
        {
            source.CopyTo(destination.FullName);
            Debug.WriteLine($"Deployed {this} to {destination.FullName}");
        }

        private FileInfo GetDestination(FileInfo source, Directories directories) =>
            new FileInfo(Path.Combine(directories.InternalDestination.FullName, source.Name));
    }
}