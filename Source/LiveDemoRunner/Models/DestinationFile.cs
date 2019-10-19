using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using LiveDemoRunner.Interfaces;

namespace LiveDemoRunner.Models
{
    public class DestinationFile: IDeployedComponent
    {

        public string Name => this.File.Name;

        private FileInfo File { get; }

        public DestinationFile(FileInfo file)
        {
            Contract.Requires(file != null, "File must be non-null.");
            Contract.Requires(file.Exists, "File must exist.");
            this.File = file;
        }

        public void Open()
        {
            Process.Start(this.File.FullName);
        }
    }
}
