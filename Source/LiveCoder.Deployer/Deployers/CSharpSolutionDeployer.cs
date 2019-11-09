using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiveCoder.Deployer.Interfaces;
using LiveCoder.Deployer.Models;

namespace LiveCoder.Deployer.Deployers
{
    internal class CSharpSolutionDeployer: IDeployer
    {

        public IEnumerable<IDeployedComponent> DeployedComponents { get; private set; }
        public bool IsDeployed { get; private set; }

        private DirectoryInfo SourceDirectory { get; }
        private IDestination Destination { get; }

        private ILogger Logger { get; }

        public CSharpSolutionDeployer(DirectoryInfo sourceDirectory, IDestination destination, ILogger logger)
        {
            this.SourceDirectory = sourceDirectory ?? throw new ArgumentNullException(nameof(sourceDirectory));
            this.Destination = destination ?? throw new ArgumentNullException(nameof(destination));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }

        public void Deploy()
        {
            this.DeployedComponents = this.GetDeployedComponents().ToList();
            this.IsDeployed = true;
        }

        public IEnumerable<IDeployedComponent> GetDeployedComponents()
        {
            foreach (IDemoComponent component in this.GetComponents())
            {
                component.DeployTo(this.Destination);

                foreach (IDeployedComponent deployed in component.GetDeploymentRoot())
                    yield return deployed;

            }
        }

        private IEnumerable<IDemoComponent> GetComponents()
        {
            return
                this.SourceDirectory
                    .EnumerateFiles("*.sln", SearchOption.AllDirectories)
                    .Take(1)
                    .Select(file => file.Directory)
                    .Select(dir => new CSharpSolutionDirectory(dir, this.Logger));
        }

        private void Deploy(IEnumerable<IDemoComponent> components)
        {
            foreach (IDemoComponent component in components)
                component.DeployTo(this.Destination);
        }

    }
}
