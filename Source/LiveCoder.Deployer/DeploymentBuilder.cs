using System.IO;
using LiveCoder.Common.Optional;

namespace LiveCoder.Deployer
{
    public class DeploymentBuilder
    {
        private Option<DirectoryInfo> Source { get; set; }

        public DeploymentBuilder From(DirectoryInfo source)
        {
            if (source?.Exists ?? false)
                this.Source = Option.Of(source);
            return this;
        }

        public Option<Deployment> TryBuild() =>
            None.Value;
    }
}
