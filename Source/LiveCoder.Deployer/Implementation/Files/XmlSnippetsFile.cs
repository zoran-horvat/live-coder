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

        protected override void Deploy(FileInfo source, Directories to)
        {
            new XmlSnippetsReader().LoadMany(source).ToList().ForEach(snippet => Debug.WriteLine(snippet));
            Debug.WriteLine($"Ignoring file {source.FullName}");
        }
    }
}