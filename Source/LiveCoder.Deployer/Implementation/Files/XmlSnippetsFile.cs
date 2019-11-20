using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiveCoder.Common.Optional;
using LiveCoder.Deployer.Implementation.Artifacts;
using LiveCoder.Deployer.Implementation.Snippets;

namespace LiveCoder.Deployer.Implementation.Files
{
    class XmlSnippetsFile : SourceFile
    {
        public XmlSnippetsFile(FileInfo location) : base(location)
        {
        }

        protected override IEnumerable<Artifact> Deploy(FileInfo source, Directories to) =>
            this.Deploy(to, source, this.Destination(to))
                .Map<IEnumerable<Artifact>>(artifact => new[] {artifact})
                .Reduce(Enumerable.Empty<Artifact>());

        private Option<Artifact> Deploy(Directories directories, FileInfo source, FileInfo destination) =>
            this.Deploy(directories, source, new XmlSnippetsReader(source).LoadMany(), destination);

        public Option<Artifact> DeployConcurrent(Directories directories, FileInfo source, FileInfo destination) =>
            new XmlSnippetsReader(source).LoadManyConcurrent().MapOptional(snippets => this.Deploy(directories, source, snippets, destination));

        private Option<Artifact> Deploy(Directories directories, FileInfo source, IEnumerable<XmlSnippet> snippets, FileInfo destination) =>
            snippets.ToList() is List<XmlSnippet> list && list.Any()
                ? this.DeployNonEmpty(directories, source, list, destination)
                : None.Value;

        private Option<Artifact> DeployNonEmpty(Directories directories, FileInfo source, List<XmlSnippet> snippets, FileInfo destination)
        {
            if (!new SnippetsScriptWriter(destination).WriteIfModified(snippets))
                return None.Value;

            XmlSnippetsRedeployer redeployer = new XmlSnippetsRedeployer(directories, source, destination);
            return new TranslatedSnippetsScript(redeployer, source, destination);
        }

        private FileInfo Destination(Directories to) =>
            new FileInfo(Path.Combine(to.InternalDestination.FullName, "script.lcs"));
    }
}