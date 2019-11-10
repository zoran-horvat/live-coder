using System;
using System.IO;

namespace LiveCoder.Extension.Scripting
{
    class ScriptLiveTracker
    {
        private FileInfo ScriptFile { get; }
        private Action<FileInfo> OnFileChanged { get; }
     
        public ScriptLiveTracker(FileInfo scriptFile, Action<FileInfo> onFileChanged)
        {
            this.ScriptFile = scriptFile;
            this.OnFileChanged = onFileChanged;
        }
    }
}
