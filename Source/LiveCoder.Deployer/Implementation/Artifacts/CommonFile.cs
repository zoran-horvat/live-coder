using System.IO;

namespace LiveCoder.Deployer.Implementation.Artifacts
{
    public class CommonFile : Artifact
    {
        private FileInfo Location { get; }

        public CommonFile(FileInfo location)
        {
            this.Location = location;
        }

        public override string ToString() =>
            $"File {this.Location.FullName}";
    }
}
