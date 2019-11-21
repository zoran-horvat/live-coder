using System;
using System.IO;
using LiveCoder.Common.Optional;
using LiveCoder.Deployer.Implementation.Files;

namespace LiveCoder.Deployer.Implementation.Artifacts
{
    public class TranslatedSnippetsScript : Artifact
    {
        private XmlSnippetsRedeployer Redeployer { get; }
        private FileInfo Snippets { get; }
        private FileInfo Script { get; }
        private Option<FileSystemWatcher> ChangeWatcher { get; set; }

        public TranslatedSnippetsScript(XmlSnippetsRedeployer redeployer, FileInfo snippets, FileInfo script)
        {
            this.Redeployer = redeployer;
            this.Snippets = snippets;
            this.Script = script;
            this.ChangeWatcher = None.Value;
        }

        public void RedeployOnChange() => 
            this.ChangeWatcher = this.ChangeWatcher.Reduce(this.StartWatching);

        private FileSystemWatcher StartWatching()
        {
            FileSystemWatcher watcher = this.CreateWatcher();
            watcher.Changed += this.OnSnippetsChanged;
            return watcher;

        }
            
        private FileSystemWatcher CreateWatcher() =>
            new FileSystemWatcher(this.Snippets.Directory?.FullName ?? string.Empty, "*.snippet")
            {
                EnableRaisingEvents = true,
            };

        private void OnSnippetsChanged(object sender, EventArgs args) =>
            this.Redeployer.TryRedeployConcurrently();

        public override string ToString() =>
            $"Snippets {this.Snippets.FullName} -> {this.Script.FullName}";
    }
}
