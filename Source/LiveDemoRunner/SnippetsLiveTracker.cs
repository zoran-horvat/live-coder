using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiveDemoRunner.Interfaces;

namespace LiveDemoRunner
{
    internal class SnippetsLiveTracker
    {
        private DirectoryInfo SourceDirectory { get; }
        private IDeployer SnippetsDeployer { get; }
        private ManualResetEvent ChangeEvent { get; }
        private bool ShouldStop { get; set; }
        private Task Worker { get; set; }

        public SnippetsLiveTracker(DirectoryInfo sourceDirectory, IDeployer deployer)
        {
            this.SourceDirectory = sourceDirectory;
            this.SnippetsDeployer = deployer;
            this.ChangeEvent = new ManualResetEvent(false);
        }

        private void OnFileChanged(object sender, EventArgs args)
        {
            this.ChangeEvent.Set();
        }

        private void TrackChanges()
        {
            this.RegisterWatchers();

            while (true)
            {

                this.ChangeEvent.WaitOne();

                if (this.ShouldStop)
                    return;

                this.SnippetsDeployer.Deploy();
                this.ChangeEvent.Reset();

            }
        }

        private void RegisterWatchers()
        {
            foreach (FileSystemWatcher watcher in this.Watchers)
            {
                watcher.NotifyFilter = NotifyFilters.LastWrite;
                watcher.Filter = "*.snippet";
                watcher.Changed += this.OnFileChanged;
                watcher.EnableRaisingEvents = true;
            }
        }

        private IEnumerable<FileInfo> TargetFiles => this.SourceDirectory.EnumerateFiles("*.snippet", SearchOption.AllDirectories);

        private IEnumerable<string> TargetDirectoryPaths => this.TargetFiles.Select(file => file.Directory.FullName.ToLower()).Distinct();

        private IEnumerable<FileSystemWatcher> Watchers => this.TargetDirectoryPaths.Select(dir => new FileSystemWatcher(dir));

        public void StartTracking()
        {
            this.Worker = Task.Run(() => this.TrackChanges());
        }

        public void StopTracking()
        {
            this.ShouldStop = true;
            this.ChangeEvent.Set();
            this.Worker.Wait();
        }
    }
}
