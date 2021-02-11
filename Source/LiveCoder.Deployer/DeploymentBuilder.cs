using System.IO;
using System.Linq;
using LiveCoder.Common.Optional;
using LiveCoder.Deployer.Implementation;

namespace LiveCoder.Deployer
{
    public class DeploymentBuilder
    {
        private IAuditor Auditor { get; }
        private Option<DirectoryInfo> Source { get; set; }
        private DirectoryInfo Destination { get; set; }

        public DeploymentBuilder(IAuditor auditor)
        {
            this.Auditor = auditor;
            this.Source = None.Value;
            this.Destination = Directories.GetDestinationDirectoryIn(new DirectoryInfo(@"C:\Demo"));
        }

        public DeploymentBuilder From(DirectoryInfo source)
        {
            if (Directory.Exists(source?.FullName))
                this.Source = Option.Of(source);
            return this;
        }

        public DeploymentBuilder To(DirectoryInfo destination)
        {
            this.Destination = destination;
            return this;
        }

        public Option<DeploymentSpecification> TryBuild() =>
            this.Source
                .Map(DirectoryBrowser.For)
                .Map(browser => browser.GetAllFiles())
                .Map(files => files.Select(this.CreateSourceFile))
                .Map(files => new DeploymentSpecification(this.Auditor, files, this.DirectoriesFactory));

        private SourceFile CreateSourceFile(FileInfo file) =>
            SourceFile.From(this.Auditor, file);

        private Option<Directories> DirectoriesFactory() =>
            this.Source.MapOptional(this.DirectoriesFactory);

        private Option<Directories> DirectoriesFactory(DirectoryInfo source) =>
            Directories.TryCreateDestinationIn(this.Auditor, this.Destination, source);
    }
}
