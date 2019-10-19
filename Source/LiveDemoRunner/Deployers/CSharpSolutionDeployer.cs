using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using LiveDemoRunner.Interfaces;
using LiveDemoRunner.Models;

namespace LiveDemoRunner.Deployers
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

            Contract.Requires(sourceDirectory != null, "Source directory must be non-null.");
            Contract.Requires(sourceDirectory.Exists, "Source directory must exist.");
            Contract.Requires(destination != null, "Destination must be non-null.");
            Contract.Requires(logger != null, "Logger must be non-null.");

            this.SourceDirectory = sourceDirectory;
            this.Destination = destination;
            this.Logger = logger;

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
