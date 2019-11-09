using System;
using System.Collections.Generic;
using System.IO;
using LiveCoder.Deployer.Interfaces;

namespace LiveCoder.Deployer.Models
{
    public abstract class File: IDemoComponent
    {

        private FileInfo SourceFile { get; }
        private ILogger Logger { get; }
        private string FileName => this.SourceFile.Name;
        public bool IsDeployed { get; private set; }
        private IList<IDeployedComponent> DeployedComponents { get; } = new List<IDeployedComponent>();

        private Action<FileInfo> BeforeDeploy { get; } = info => { };

        public File(FileInfo file, ILogger logger)
        {
            this.SourceFile = file ?? throw new ArgumentNullException(nameof(file));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }

        public File(FileInfo file, ILogger logger, Action<FileInfo> beforeDeploy) : this(file, logger)
        {
            this.BeforeDeploy = beforeDeploy ?? throw new ArgumentNullException(nameof(beforeDeploy));
        }

        public void DeployTo(IDestination destination)
        {

            this.BeforeDeploy(this.SourceFile);

            using (FileStream stream = this.SourceFile.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                foreach (IDeployedComponent deployed in this.Deploy(stream, destination))
                {
                    this.DeployedComponents.Add(deployed);
                    this.Logger.Log($"Deployed {deployed.Name}");
                }
                this.IsDeployed = true;
            }

        }

        protected abstract IEnumerable<IDeployedComponent> Deploy(Stream stream, IDestination toDestination);

        public IEnumerable<IDeployedComponent> GetDeploymentRoot() => this.DeployedComponents;

    }
}
