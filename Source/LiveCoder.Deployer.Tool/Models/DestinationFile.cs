using System;
using System.Diagnostics;
using System.IO;
using LiveCoder.Deployer.Tool.Interfaces;

namespace LiveCoder.Deployer.Tool.Models
{
    public class DestinationFile: IDeployedComponent
    {

        public string Name => this.File.Name;

        private FileInfo File { get; }

        public DestinationFile(FileInfo file)
        {
            this.File = file ?? throw new ArgumentNullException(nameof(file));
        }

        public void Open()
        {
            Process.Start(this.File.FullName);
        }
    }
}
