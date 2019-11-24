using System;
using System.IO;

namespace LiveCoder.Scripting
{
    public class ScriptLiveTracker
    {
        private FileSystemWatcher FileWatcher { get; }
    
        public ScriptLiveTracker(FileInfo scriptFile, Action<FileInfo> onFileChanged)
        {
            this.FileWatcher = new FileSystemWatcher(scriptFile?.DirectoryName ?? string.Empty)
            {
                NotifyFilter = NotifyFilters.LastWrite, 
                Filter = scriptFile?.Name ?? string.Empty
            };
            this.FileWatcher.Changed += (sender, args) => onFileChanged(scriptFile);
            this.FileWatcher.EnableRaisingEvents = true;
        }
    }
}
