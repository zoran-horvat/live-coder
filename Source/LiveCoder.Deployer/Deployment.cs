using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using LiveCoder.Common.Optional;
using LiveCoder.Deployer.Implementation;

namespace LiveCoder.Deployer
{
    public class Deployment
    {
        private ImmutableList<SourceFile> Files { get; }
        private Func<Option<Directories>> DirectoriesFactory { get; }

        internal Deployment(IEnumerable<SourceFile> files, Func<Option<Directories>> directoriesFactory)
        {
            this.Files = files.ToImmutableList();
            this.DirectoriesFactory = directoriesFactory;
        }

        public void Execute() =>
            this.Directories.Do(this.DeployTo, this.OnFailedCreateDirectories);

        private Option<Directories> Directories => 
            this.DirectoriesFactory();

        private void DeployTo(Directories directories) => 
            this.Files.ForEach(file => file.Deploy(directories));

        private void OnFailedCreateDirectories() => 
            Debug.WriteLine("Failed to create directories.");
    }
}