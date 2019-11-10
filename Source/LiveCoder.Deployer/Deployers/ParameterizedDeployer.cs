using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiveCoder.Common.Optional;
using LiveCoder.Deployer.Infrastructure;
using LiveCoder.Deployer.Interfaces;
using LiveCoder.Deployer.Models;

namespace LiveCoder.Deployer.Deployers
{
    internal class ParameterizedDeployer: IDeployer
    {

        private Arguments Arguments { get; }
        
        private DateTime CreationTime { get; }

        private ILogger Logger { get; }

        public bool IsDeployed { get; private set; }

        public IEnumerable<IDeployedComponent> DeployedComponents { get; private set; }

        private IDestination FilesDestination { get; set; }

        public ParameterizedDeployer(Arguments arguments, ILogger logger)
        {
            this.Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
            this.CreationTime = DateTime.UtcNow;
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }

        private IFutureDestination GetFilesDestination() => 
            new FileSystemDestination(this.DestinationDirectory);

        private DirectoryInfo DestinationDirectory =>
            new DirectoryInfo(Path.Combine(@"C:\", "Demo", this.DestinationSubdirectoryName));

        private string DestinationSubdirectoryName =>
            $"{this.CreationTime:yyyyMMddHHmmss}";

        private DirectoryInfo ScriptDirectory =>
            new DirectoryInfo(Path.Combine(this.DestinationDirectory.FullName, ".livecoder"));

        private FileInfo ScriptFile => 
            new FileInfo(Path.Combine(this.ScriptDirectory.FullName, "script.lcs"));

        private IDestination ScriptAppender =>
            new XmlSnippetsPublisher(this.ScriptFile);

        public void Deploy()
        {
            if (this.Arguments.CopyFiles)
                PrepareFilesDestination();

            IEnumerable<IDeployer> fileDeployers = GetFilesDeployers(this.Arguments).ToList();

            IEnumerable<IDeployer> snippetsDeployer = GetSnippetsDeployer(this.Arguments).ToList();

            if (this.Arguments.NormalizeSnippets && !this.Arguments.LiveTrackSnippets)
                NormalizeSnippets(this.Arguments.SourceDirectory);

            IDeployer[] deployers = fileDeployers.Concat(snippetsDeployer).ToArray();
            this.Deploy(deployers);
            this.DeployedComponents = fileDeployers.SelectMany(deployer => deployer.DeployedComponents).ToList();

            if (this.Arguments.OpenFiles)
                this.OpenDeployedFiles();

            if (this.Arguments.LiveTrackSnippets && 
                snippetsDeployer.FirstOrNone() is Some<IDeployer> someDeployer &&
                someDeployer.Content is IDeployer snippetsLiveDeployer)
                TrackSnippets(this.Arguments.SourceDirectory, snippetsLiveDeployer);
        }

        private void Deploy(params IDeployer[] deployers)
        {
            foreach (IDeployer deployer in deployers)
                deployer.Deploy();
            this.IsDeployed = true;
        }

        private void NormalizeSnippets(DirectoryInfo sourceDirectory)
        {
            SnippetsNormalizer normalizer = new SnippetsNormalizer(this.Logger);
            foreach (FileInfo file in sourceDirectory.EnumerateFiles("*.snippet", SearchOption.AllDirectories))
                normalizer.Normalize(file);
        }

        private void PrepareFilesDestination()
        {
            IFutureDestination futureDestination = GetFilesDestination();
            futureDestination.PrepareForDeployment();
            this.FilesDestination = futureDestination.GetDeploymentDestination();
        }

        private void TrackSnippets(DirectoryInfo sourceDirectory, IDeployer deployer)
        {

            SnippetsLiveTracker tracker = new SnippetsLiveTracker(sourceDirectory, deployer);

            tracker.StartTracking();

            Console.WriteLine();
            Console.WriteLine("Type 'exit' to stop tracking snippet files.");
            Console.WriteLine();

            while (true)
            {
                if (Console.ReadLine()?.ToLower() == "exit")
                    break;
            }

            tracker.StopTracking();

        }

        private IEnumerable<IDeployer> GetFilesDeployers(Arguments arguments)
        {

            if (arguments.CopyVisualStudioFiles)
                yield return new CSharpSolutionDeployer(arguments.SourceDirectory, this.FilesDestination, this.Logger);

            if (arguments.CopyPowerPointFiles)
                yield return new FileTypeDeployer(arguments.SourceDirectory, "pptx", this.FilesDestination, this.Logger);

            if (arguments.CopySql)
                yield return new FileTypeDeployer(arguments.SourceDirectory, "sql", this.FilesDestination, this.Logger);

        }

        private IEnumerable<IDeployer> GetSnippetsDeployer(Arguments arguments)
        {
            if (!arguments.CopySnippets)
                return new IDeployer[0];

            Action<FileInfo> beforeDeploySnippets = info => { };
            if (arguments.NormalizeSnippets)
                beforeDeploySnippets = info => new SnippetsNormalizer(this.Logger).Normalize(info);

            return new[] { new CodeSnippetsDeployer(arguments.SourceDirectory, this.ScriptAppender, Logger, beforeDeploySnippets) };

        }

        private void OpenDeployedFiles()
        {
            foreach (IDeployedComponent component in this.DeployedComponents)
                component.Open();
        }

    }
}
