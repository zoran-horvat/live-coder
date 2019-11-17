using System.IO;

namespace LiveCoder.Deployer.Implementation.Artifacts
{
    public class VisualStudioSolution : Artifact
    {
        public FileInfo Location { get; }
     
        private VisualStudioSolution(FileInfo location)
        {
            this.Location = location;
        }

        public static Artifact At(FileInfo location) =>
            new VisualStudioSolution(location);

        public override string ToString() =>
            $"VisualStudio solution file {this.Location.FullName}";
    }
}
