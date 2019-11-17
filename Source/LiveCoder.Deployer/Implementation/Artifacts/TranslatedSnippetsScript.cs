using System.IO;

namespace LiveCoder.Deployer.Implementation.Artifacts
{
    public class TranslatedSnippetsScript : Artifact
    {
        private FileInfo Snippets { get; }
        private FileInfo Script { get; }

        public TranslatedSnippetsScript(FileInfo snippets, FileInfo script)
        {
            this.Snippets = snippets;
            this.Script = script;
        }

        public override string ToString() =>
            $"Translated XML snippets {this.Snippets.FullName} -> {this.Script.FullName}";
    }
}
