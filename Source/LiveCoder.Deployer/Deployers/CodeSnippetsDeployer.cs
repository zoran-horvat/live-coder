using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiveCoder.Deployer.Interfaces;
using LiveCoder.Deployer.Models;

namespace LiveCoder.Deployer.Deployers
{
    internal class CodeSnippetsDeployer: IDeployer
    {

        public IEnumerable<IDeployedComponent> DeployedComponents { get; private set; }
        public bool IsDeployed { get; private set; }

        private DirectoryInfo SourceDirectory { get; }

        private IDestination Script { get; }
        private string MyDocumentsPath => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private DirectoryInfo MyDocumentsDirectory => new DirectoryInfo(MyDocumentsPath);
        private IEnumerable<DirectoryInfo> VisualStudioDirectories => MyDocumentsDirectory.EnumerateDirectories(@"Visual Studio *", SearchOption.TopDirectoryOnly);

        private Action<FileInfo> BeforeDeployFile { get; } = info => { };

        private ILogger Logger { get; }

        private IEnumerable<DirectoryInfo> CodeSnippetsDirectories =>
            VisualStudioDirectories
                .SelectMany(dir => dir.GetDirectories("Code Snippets"))
                .SelectMany(dir => dir.GetDirectories("Visual C#"))
                .SelectMany(dir => dir.GetDirectories("My Code Snippets"))
                .ToList();

        public CodeSnippetsDeployer(DirectoryInfo sourceDirectory, IDestination script, ILogger logger)
        {
            this.SourceDirectory = sourceDirectory ?? throw new ArgumentNullException(nameof(sourceDirectory));
            this.Script = script ?? throw new ArgumentNullException(nameof(script));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }

        public CodeSnippetsDeployer(DirectoryInfo sourceDirectory, IDestination script, ILogger logger, Action<FileInfo> beforeDeployFile)
            : this(sourceDirectory, script, logger)
        {
            this.BeforeDeployFile = beforeDeployFile ?? throw new ArgumentNullException(nameof(beforeDeployFile));
        }

        public void Deploy()
        {
            this.DeployedComponents = this.GetDeployedComponents().ToList();
            this.IsDeployed = true;
        }

        private IEnumerable<IDeployedComponent> GetDeployedComponents()
        {
            foreach (Tuple<IDemoComponent, IDestination> candidate in this.GetDeploymentCandidates())
            {
                IDemoComponent file = candidate.Item1;
                IDestination destination = candidate.Item2;
                file.DeployTo(destination);

                foreach (IDeployedComponent deployed in file.GetDeploymentRoot())
                    yield return deployed;

            }
        }

        private IEnumerable<Tuple<IDemoComponent, IDestination>> GetDeploymentCandidates() =>
            SourceDirectory
                .EnumerateFiles("*.snippet", SearchOption.AllDirectories)
                .SelectMany(this.PrepareSnippetsDeployment);

        private void DeleteAllFiles(DirectoryInfo dir)
        {
            foreach (FileInfo file in dir.EnumerateFiles())
            {
                try
                {
                    file.Delete();
                }
                catch (Exception)
                {
                }
            }
        }

        private IDestination PrepareSnippetsDestinations(DirectoryInfo dir)
        {
            DeleteAllFiles(dir);
            return new FileSystemDirectory(dir);
        }

        private IEnumerable<IDestination> PrepareSnippetsDestinations() => 
            this.CodeSnippetDestinations.Concat(this.ScriptFileDestination).ToList();

        private IEnumerable<IDestination> CodeSnippetDestinations =>
            this.CodeSnippetsDirectories.Select(this.PrepareSnippetsDestinations);

        private IEnumerable<IDestination> ScriptFileDestination =>
            new[] {this.Script};

        private IEnumerable<Tuple<IDemoComponent, IDestination>> PrepareSnippetsDeployment(FileInfo snippetsFile)
        {
            return PrepareSnippetsDeployment(new Models.SplittableSnippetsFile(snippetsFile, this.Logger, this.BeforeDeployFile));
        }

        private IEnumerable<Tuple<IDemoComponent, IDestination>> PrepareSnippetsDeployment(IDemoComponent snippetsFile)
        {
            return
                PrepareSnippetsDestinations()
                    .Select(destination => Tuple.Create(snippetsFile, destination))
                    .ToList();
        }

    }
}