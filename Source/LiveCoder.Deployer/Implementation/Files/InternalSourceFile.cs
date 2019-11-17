using System.Diagnostics;
using System.IO;

namespace LiveCoder.Deployer.Implementation.Files
{
    class InternalSourceFile : SourceFile
    {
        public InternalSourceFile(FileInfo location) : base(location)
        {
        }

        protected override void Deploy(FileInfo source, Directories directories)
        {
            Debug.WriteLine($"Ignoring deployment of {source.FullName}");
        }
    }
}