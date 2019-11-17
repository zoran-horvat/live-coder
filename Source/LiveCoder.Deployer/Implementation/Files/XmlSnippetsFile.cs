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

        protected override void Deploy(FileInfo source, Directories to) =>
            this.Deploy(source, this.Destination(to));

        private void Deploy(FileInfo source, FileInfo destination) =>
            this.Deploy(new XmlSnippetsReader(source).LoadMany(), destination);

        private void Deploy(IEnumerable<XmlSnippet> snippets, FileInfo destination)
        {
            new SnippetsScriptWriter(destination).Write(snippets);
            Debug.WriteLine($"Deployed {this} to {destination.FullName}");
        }

        private FileInfo Destination(Directories to) =>
            new FileInfo(Path.Combine(to.InternalDestination.FullName, "script.lcs"));
    }
}