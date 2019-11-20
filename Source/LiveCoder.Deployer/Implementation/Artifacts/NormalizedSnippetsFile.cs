using System.IO;

namespace LiveCoder.Deployer.Implementation.Artifacts
{
    public class NormalizedSnippetsFile : Artifact
    {
        private FileInfo Location { get; }
     
        public NormalizedSnippetsFile(FileInfo location)
        {
            this.Location = location;
        }

        public override string ToString() =>
            $"Normalized snippets {this.Location.FullName}";
    }
}
