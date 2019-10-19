using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using LiveDemoRunner.Interfaces;
using LiveDemoRunner.Models;

namespace LiveDemoRunner.Deployers
{
    internal class FileTypeDeployer: IDeployer
    {

        public IEnumerable<IDeployedComponent> DeployedComponents { get; private set; }
        public bool IsDeployed { get; private set; }

        private DirectoryInfo SourceDirectory { get; }
        private string FileExtension { get; }
        private IDestination DeploymentDestination { get; }

        private Action<FileInfo> BeforeDeployFile { get; } = info => { };

        private string FileNamesFilter => $"*.{this.FileExtension}";

        private ILogger Logger { get; }

        public FileTypeDeployer(DirectoryInfo sourceDirectory, string fileExtension, IDestination deploymentDestination, ILogger logger)
        {

            Contract.Requires(sourceDirectory != null, "Source directory must be non-null.");
            Contract.Requires(sourceDirectory.Exists, "Source directory must exist.");
            Contract.Requires(!string.IsNullOrEmpty(fileExtension), "File extension must be non-empty.");
            Contract.Requires(Regex.IsMatch(fileExtension, @"^\w+$"), "Must be a valid file extension.");
            Contract.Requires(deploymentDestination != null, "Deployment destination must be non-null.");
            Contract.Requires(logger != null, "Logger must be non-null.");

            this.SourceDirectory = sourceDirectory;
            this.FileExtension = fileExtension;
            this.DeploymentDestination = deploymentDestination;
            this.Logger = logger;

        }

        public FileTypeDeployer(DirectoryInfo sourceDirectory, string fileExtension, IDestination deploymentDestination,
            ILogger logger, Action<FileInfo> beforeDeployFile)
            : this(sourceDirectory, fileExtension, deploymentDestination, logger)
        {
            Contract.Requires(beforeDeployFile != null, "Action before file deployment must be non-null.");
            this.BeforeDeployFile = beforeDeployFile;
        }

        public void Deploy()
        {
            this.DeployedComponents = this.GetDeployedComponents().ToList();
            this.IsDeployed = true;
        }

        public IEnumerable<IDeployedComponent> GetDeployedComponents()
        {
            foreach (IDemoComponent component in this.GetDeploymentComponents())
            {
                component.DeployTo(this.DeploymentDestination);

                foreach (IDeployedComponent deployed in component.GetDeploymentRoot())
                    yield return deployed;
            }
        }

        private IEnumerable<IDemoComponent> GetDeploymentComponents()
        {
            return
                this.SourceDirectory
                    .EnumerateFiles(this.FileNamesFilter, SearchOption.TopDirectoryOnly)
                    .Select(file => new Models.IntegralFile(file, this.Logger, this.BeforeDeployFile));
        }

    }
}
