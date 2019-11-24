using System.IO;
using LiveCoder.Extension.Interfaces;
using LiveCoder.Scripting;

namespace LiveCoder.Extension.Events
{
    public class ScriptFileFound : IEvent
    {
        public string Label =>
            $"Script file found: {this.File.FullName}";

        private FileInfo File { get; }

        public ScriptFileFound(FileInfo file)
        {
            this.File = file;
        }
    }
}
