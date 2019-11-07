using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using LiveDemoRunner.Interfaces;

namespace LiveDemoRunner.Models
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

            Contract.Requires(file != null, "File must be non-null.");
            Contract.Requires(file.Exists, "Source file must exist.");
            Contract.Requires(logger != null, "Logger must be non-null.");

            this.SourceFile = file;
            this.Logger = logger;

        }

        public File(FileInfo file, ILogger logger, Action<FileInfo> beforeDeploy) : this(file, logger)
        {
            Contract.Requires(beforeDeploy != null, "Action before file deployment must be non-null.");
            this.BeforeDeploy = beforeDeploy;
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
