using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using LiveCoder.Common.Optional;
using LiveCoder.Deployer.Implementation;

namespace LiveCoder.Deployer
{
    public class DeploymentSpecification
    {
        private ImmutableList<SourceFile> Files { get; }
        private Func<Option<Directories>> DirectoriesFactory { get; }

        internal DeploymentSpecification(IEnumerable<SourceFile> files, Func<Option<Directories>> directoriesFactory)
        {
            this.Files = files.ToImmutableList();
            this.DirectoriesFactory = directoriesFactory;
        }

        public Option<Deployment> Execute() =>
            this.Directories
                .Map(directories => Option.Of(this.DeployTo(directories)))
                .Reduce(this.OnFailedCreateDirectories);

        private Option<Directories> Directories => 
            this.DirectoriesFactory();

        private Deployment DeployTo(Directories directories) =>
            new Deployment(this.Files.SelectMany(file => file.Deploy(directories)));

        private Option<Deployment> OnFailedCreateDirectories()
        {
            Debug.WriteLine("Failed to create directories.");
            return None.Value;
        }
    }
}