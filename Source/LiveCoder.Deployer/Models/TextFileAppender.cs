using System;
using System.IO;

namespace LiveCoder.Deployer.Models
{
    class TextFileAppender
    {
        private FileInfo Target { get; }
        public DirectoryInfo Directory => this.Target.Directory;
     
        public TextFileAppender(FileInfo target)
        {
            this.Target = target;
        }

        public void AppendLine(string text) =>
            this.WriteLine(FileMode.Append, text);

        public void WriteFirstLine(string text) =>
            this.WriteLine(FileMode.Create, text);

        private void WriteLine(FileMode mode, string text) =>
            this.UseFile(mode, stream => this.WriteLine(stream, text));

        private void WriteLine(FileStream stream, string text)
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine(text);
            }
        }

        private void UseFile(FileMode mode, Action<FileStream> action)
        {
            this.Directory.Create();
            using (FileStream stream = System.IO.File.Open(this.Target.FullName, mode))
            {
                action(stream);
            }
        }
    }
}
