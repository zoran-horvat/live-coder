using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using LiveCoder.Deployer.Interfaces;

namespace LiveCoder.Deployer.Models
{
    public class CSharpSolutionDirectory: IDemoComponent
    {

        private DirectoryInfo SourceDirectory { get; }
        private ILogger Logger { get; }
        private FileInfo SourceSolutionFile { get; }
        public Action<FileInfo> BeforeDeployFile { get; } = info => { };

        private DirectoryInfo SourceSolutionDirectory => this.SourceSolutionFile.Directory;
        private string SourceSolutionFileName => this.SourceSolutionFile.Name;

        private readonly string[] IgnoreDirectories = new[] {".vs", "bin", "obj"};

        private IList<IDeployedComponent> DeployedComponents { get; } = new List<IDeployedComponent>();

        public CSharpSolutionDirectory(DirectoryInfo sourceDirectory, ILogger logger)
        {

            Contract.Requires(sourceDirectory != null, "Solution directory must be non-null.");
            Contract.Requires(sourceDirectory.Exists, "Solution directory must exist.");
            Contract.Requires(Contract.Exists(sourceDirectory.GetFiles("*.sln", SearchOption.AllDirectories), file => true));
            Contract.Requires(logger != null, "Logger must be non-null.");

            this.SourceDirectory = sourceDirectory;
            this.SourceSolutionFile = sourceDirectory.GetFiles("*.sln", SearchOption.AllDirectories).First();
            this.Logger = logger;

        }

        public CSharpSolutionDirectory(DirectoryInfo sourceDirectory, ILogger logger, Action<FileInfo> beforeDeployFile)
            : this(sourceDirectory, logger)
        {
            Contract.Requires(beforeDeployFile != null, "Action before file deployment must be non-null.");
            this.BeforeDeployFile = beforeDeployFile;
        }

        public void DeployTo(IDestination destination)
        {
            this.DeployContentOf(this.SourceDirectory, destination);
            IDestination destinationDirectory = this.GetDestinationDirectoryFor(SourceSolutionDirectory, destination);
            this.DeployedComponents.Add(destinationDirectory.GetFile(this.SourceSolutionFileName));
            this.IsDeployed = true;
        }

        public IEnumerable<IDeployedComponent> GetDeploymentRoot() => this.DeployedComponents;

        public bool IsDeployed { get; private set; }

        private IDestination GetDestinationDirectoryFor(DirectoryInfo directory, IDestination rootDestination)
        {
            if (directory.FullName.Equals(this.SourceDirectory.FullName))
                return rootDestination;
            return this.GetDestinationDirectoryFor(directory.Parent, rootDestination).GetDirectory(directory.Name);
        }

        private void DeployContentOf(DirectoryInfo directory, IDestination toDestination)
        {
            this.Deploy(directory.EnumerateFiles(), toDestination);
            this.Deploy(directory.EnumerateDirectories(), toDestination);
        }

        private void Deploy(IEnumerable<DirectoryInfo> subdirectories, IDestination toDestination)
        {
            foreach (DirectoryInfo dir in subdirectories)
            {
                this.Deploy(dir, toDestination);
            }
        }

        private void Deploy(DirectoryInfo dir, IDestination toDestination)
        {

            if (this.IgnoreDirectories.Contains(dir.Name.ToLower()))
                return;

            toDestination.DeployDirectory(dir.Name);
            this.DeployContentOf(dir, toDestination.GetDirectory(dir.Name));

        }

        private void Deploy(IEnumerable<FileInfo> files, IDestination toDestination)
        {
            foreach (FileInfo file in files)
            {
                Deploy(file, toDestination);
            }
        }

        private void Deploy(FileInfo file, IDestination toDestination)
        {
            new IntegralFile(file, this.Logger, this.BeforeDeployFile).DeployTo(toDestination);
        }
    }
}
