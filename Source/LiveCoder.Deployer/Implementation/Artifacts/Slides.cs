using System.IO;

namespace LiveCoder.Deployer.Implementation.Artifacts
{
    public class Slides : Artifact
    {
        private FileInfo Location {get;}
     
        private Slides(FileInfo location)
        {
            this.Location = location;
        }

        public static Artifact At(FileInfo location) =>
            new Slides(location);

        public override string ToString() =>
            $"Slides file {this.Location.FullName}";
    }
}
