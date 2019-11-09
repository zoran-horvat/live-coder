using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using LiveCoder.Deployer.Interfaces;

namespace LiveCoder.Deployer.Deployers
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
            this.SourceDirectory = sourceDirectory ?? throw new ArgumentNullException(nameof(sourceDirectory));
            this.FileExtension = fileExtension ?? throw new ArgumentNullException(nameof(fileExtension));
            this.DeploymentDestination = deploymentDestination ?? throw new ArgumentNullException(nameof(deploymentDestination));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }

        public FileTypeDeployer(DirectoryInfo sourceDirectory, string fileExtension, IDestination deploymentDestination,
            ILogger logger, Action<FileInfo> beforeDeployFile)
            : this(sourceDirectory, fileExtension, deploymentDestination, logger)
        {
            this.BeforeDeployFile = beforeDeployFile ?? throw new ArgumentNullException(nameof(beforeDeployFile));
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
