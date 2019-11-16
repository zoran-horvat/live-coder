using System.Diagnostics;
using System.IO;

namespace LiveCoder.Deployer.Implementation
{
    class SourceFile
    {
        private FileInfo Location { get; }
     
        private SourceFile(FileInfo location)
        {
            this.Location = location;
        }

        public static SourceFile From(FileInfo location)
        {
            Debug.WriteLine($"Preparing to deploy {location.FullName}");
            return new SourceFile(location);
        }
    }
}
