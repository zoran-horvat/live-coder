using System.Diagnostics;
using System.IO;

namespace LiveCoder.Deployer.Implementation.Files
{
    class XmlSnippetsFile : SourceFile
    {
        public XmlSnippetsFile(FileInfo location) : base(location)
        {
        }

        protected override void Deploy(FileInfo source, Directories to)
        {
            Debug.WriteLine($"Ignoring file {source.FullName}");
        }
    }
}