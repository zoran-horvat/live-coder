using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using LiveCoder.Common;
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
            this.Redeployer.TryRedeployConcurrently().Do(this.Report);

        private void Report(IEnumerable<Artifact> artifacts) =>
            this.ToString(artifacts).Do(report => Debug.WriteLine(report));

        private Option<string> ToString(IEnumerable<Artifact> artifacts) =>
            artifacts.Select(artifact => $"{artifact}").Join(Environment.NewLine) is string report && report.Length > 0
                ? Option.Of(report)
                : None.Value;

        public override string ToString() =>
            $"Translated XML snippets {this.Snippets.FullName} -> {this.Script.FullName}";
    }
}
