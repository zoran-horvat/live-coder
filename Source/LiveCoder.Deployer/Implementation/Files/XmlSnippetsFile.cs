using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using LiveCoder.Deployer.Implementation.Snippets;

namespace LiveCoder.Deployer.Implementation.Files
{
    class XmlSnippetsFile : SourceFile
    {
        public XmlSnippetsFile(FileInfo location) : base(location)
        {
        }

        protected override IEnumerable<Artefact> Deploy(FileInfo source, Directories to) =>
            this.Deploy(source, this.Destination(to));

        private IEnumerable<Artefact> Deploy(FileInfo source, FileInfo destination) =>
            this.Deploy(new XmlSnippetsReader(source).LoadMany(), destination);

        private IEnumerable<Artefact> Deploy(IEnumerable<XmlSnippet> snippets, FileInfo destination)
        {
            new SnippetsScriptWriter(destination).Write(snippets);
            return Enumerable.Empty<Artefact>();
        }

        private FileInfo Destination(Directories to) =>
            new FileInfo(Path.Combine(to.InternalDestination.FullName, "script.lcs"));
    }
}