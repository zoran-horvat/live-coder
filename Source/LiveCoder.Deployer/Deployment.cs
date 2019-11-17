using System;
using System.Collections.Generic;
using System.Linq;
using LiveCoder.Common.Optional;
using LiveCoder.Deployer.Implementation;

namespace LiveCoder.Deployer
{
    public class Deployment
    {
        private IEnumerable<SourceFile> Files { get; }
        private Func<Option<Directories>> DirectoriesFactory { get; }

        internal Deployment(IEnumerable<SourceFile> files, Func<Option<Directories>> directoriesFactory)
        {
            this.Files = files.ToList();
            this.DirectoriesFactory = directoriesFactory;
        }

        public void Execute()
        {
            Option<Directories> directories = this.DirectoriesFactory();
        }
    }
}