using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiveCoder.Common.Optional;
using LiveCoder.Deployer.Implementation;

namespace LiveCoder.Deployer
{
    public class DeploymentBuilder
    {
        private Option<DirectoryInfo> Source { get; set; }

        public DeploymentBuilder From(DirectoryInfo source)
        {
            if (source?.Exists ?? false)
                this.Source = Option.Of(source);
            return this;
        }

        public Option<DeploymentSpecification> TryBuild() =>
            this.Source
                .Map(DirectoryBrowser.For)
                .Map(browser => browser.GetAllFiles())
                .Map(files => files.Select(SourceFile.From))
                .Map(files => new DeploymentSpecification(files, this.DirectoriesFactory));

        private Option<Directories> DirectoriesFactory() =>
            this.Source.MapOptional(this.DirectoriesFactory);

        private Option<Directories> DirectoriesFactory(DirectoryInfo source) =>
            Directories.TryCreateDestinationIn(new DirectoryInfo(@"C:\Demo"), source);
    }
}
