using System.Collections.Generic;
using System.IO;
using LiveCoder.Common.Optional;

namespace LiveCoder.Deployer.Implementation.Files
{
    public class XmlSnippetsRedeployer
    {
        private Directories Directories { get; }
        private FileInfo Snippets { get; }
        private FileInfo Script { get; }

        public XmlSnippetsRedeployer(Directories directories, FileInfo snippets, FileInfo script)
        {
            this.Directories = directories;
            this.Snippets = snippets;
            this.Script = script;
        }

        public Option<IEnumerable<Artifact>> TryRedeployConcurrently() => 
            new XmlSnippetsFile(this.Snippets).DeployConcurrent(this.Directories, this.Snippets, this.Script);
    }
}