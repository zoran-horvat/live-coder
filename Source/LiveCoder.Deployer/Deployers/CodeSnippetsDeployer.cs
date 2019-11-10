using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiveCoder.Deployer.Interfaces;

namespace LiveCoder.Deployer.Deployers
{
    internal class CodeSnippetsDeployer: IDeployer
    {

        public IEnumerable<IDeployedComponent> DeployedComponents { get; private set; }
        public bool IsDeployed { get; private set; }

        private DirectoryInfo SourceDirectory { get; }

        private IDestination Script { get; }

        private Action<FileInfo> BeforeDeployFile { get; }

        private ILogger Logger { get; }

        public CodeSnippetsDeployer(DirectoryInfo sourceDirectory, IDestination script, ILogger logger, Action<FileInfo> beforeDeployFile)
            : this(sourceDirectory, script, logger)
        {
            this.BeforeDeployFile = beforeDeployFile ?? throw new ArgumentNullException(nameof(beforeDeployFile));
        }

        private CodeSnippetsDeployer(DirectoryInfo sourceDirectory, IDestination script, ILogger logger)
        {
            this.SourceDirectory = sourceDirectory ?? throw new ArgumentNullException(nameof(sourceDirectory));
            this.Script = script ?? throw new ArgumentNullException(nameof(script));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }

        public void Deploy()
        {
            this.DeployedComponents = this.GetDeployedComponents().ToList();
            this.IsDeployed = true;
        }

        private IEnumerable<IDeployedComponent> GetDeployedComponents()
        {
            foreach (IDemoComponent file in this.GetDeploymentCandidates())
            {
                file.DeployTo(this.Script);

                foreach (IDeployedComponent deployed in file.GetDeploymentRoot())
                    yield return deployed;
            }
        }

        private IEnumerable<IDemoComponent> GetDeploymentCandidates() =>
            SourceDirectory
                .EnumerateFiles("*.snippet", SearchOption.AllDirectories)
                .Select(file => new Models.SplittableSnippetsFile(file, this.Logger, this.BeforeDeployFile));
    }
}