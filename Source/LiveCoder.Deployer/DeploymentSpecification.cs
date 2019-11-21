using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using LiveCoder.Common.Optional;
using LiveCoder.Deployer.Implementation;

namespace LiveCoder.Deployer
{
    public class DeploymentSpecification
    {
        private IAuditor Auditor { get; }
        private ImmutableList<SourceFile> Files { get; }
        private Func<Option<Directories>> CreateDirectories { get; }

        internal DeploymentSpecification(IAuditor auditor, IEnumerable<SourceFile> files, Func<Option<Directories>> createDirectories)
        {
            this.Auditor = auditor;
            this.Files = files.ToImmutableList();
            this.CreateDirectories = createDirectories;
        }

        public Option<Deployment> Execute() =>
            this.CreateDirectories()
                .Map(this.DeployTo)
                .AuditNone(() => this.Auditor.FailedToCreateDestination());

        private Deployment DeployTo(Directories directories) =>
            new Deployment(this.Files.SelectMany(file => file.Deploy(directories)));
    }
}