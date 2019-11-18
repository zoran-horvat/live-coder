using System.Diagnostics;
using System.IO;

namespace LiveCoder.Deployer.Implementation.Artifacts
{
    public class VisualStudioSolution : Artifact
    {
        private FileInfo Location { get; }
     
        private VisualStudioSolution(FileInfo location)
        {
            this.Location = location;
        }

        public static Artifact At(FileInfo location) =>
            new VisualStudioSolution(location);

        public void Open() =>
            Process.Start(this.Location.FullName);

        public override string ToString() =>
            $"VisualStudio solution file {this.Location.FullName}";
    }
}
