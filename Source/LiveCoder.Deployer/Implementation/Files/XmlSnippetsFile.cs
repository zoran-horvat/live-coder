using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            this.Deploy(source, this.Destination(to));

        private IEnumerable<Artifact> Deploy(FileInfo source, FileInfo destination) =>
            this.Deploy(source, new XmlSnippetsReader(source).LoadMany(), destination);

        private IEnumerable<Artifact> Deploy(FileInfo source, IEnumerable<XmlSnippet> snippets, FileInfo destination)
        {
            new SnippetsScriptWriter(destination).Write(snippets);
            return new[] {new TranslatedSnippetsScript(source, destination)};
        }

        private FileInfo Destination(Directories to) =>
            new FileInfo(Path.Combine(to.InternalDestination.FullName, "script.lcs"));
    }
}