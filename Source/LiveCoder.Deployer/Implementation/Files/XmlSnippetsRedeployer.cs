using System.IO;
using LiveCoder.Common.Optional;
using LiveCoder.Deployer.Implementation.Artifacts;
using LiveCoder.Deployer.Implementation.Snippets;

namespace LiveCoder.Deployer.Implementation.Files
{
    public class XmlSnippetsRedeployer
    {
        private IAuditor Auditor { get; }
        private Directories Directories { get; }
        private FileInfo Snippets { get; }
        private FileInfo Script { get; }

        public XmlSnippetsRedeployer(IAuditor auditor, Directories directories, FileInfo snippets, FileInfo script)
        {
            this.Auditor = auditor;
            this.Directories = directories;
            this.Snippets = snippets;
            this.Script = script;
        }

        public Option<Artifact> TryRedeployConcurrently() =>
            this.TryNormalizeSnippets()
                .Map(Option.Of)
                .Reduce(this.TryRedeployNormalized);

        private Option<Artifact> TryNormalizeSnippets() =>
            new SnippetsNormalizer(this.Snippets).Normalize()
                ? Option.Of<Artifact>(new NormalizedSnippetsFile(this.Snippets))
                : None.Value;

        private Option<Artifact> TryRedeployNormalized() =>
            new XmlSnippetsFile(this.Auditor, this.Snippets)
                .DeployConcurrent(this.Auditor, this.Directories, this.Snippets, this.Script);
    }
}