using System;
using System.Diagnostics.Contracts;
using System.IO;
using LiveDemoRunner.Interfaces;

namespace LiveDemoRunner.Models
{
    public class FileSystemDestination: IFutureDestination
    {

        private DirectoryInfo DeploymentDirectory { get; }

        public FileSystemDestination(DirectoryInfo rootDirectory)
        {

            Contract.Requires(rootDirectory != null, "Root directory must be non-null.");
        
            DateTime timestamp = DateTime.UtcNow;
            string deploymentPath = $"{rootDirectory.FullName}\\{timestamp:yyyyMMddHHmmss}";
            this.DeploymentDirectory = new DirectoryInfo(deploymentPath);

        }

        public void PrepareForDeployment() => this.CreateDeploymentDirectory();

        public IDestination GetDeploymentDestination() => new FileSystemDirectory(this.DeploymentDirectory);

        private void CreateDeploymentDirectory() => Directory.CreateDirectory(this.DeploymentDirectory.FullName);

    }
}
