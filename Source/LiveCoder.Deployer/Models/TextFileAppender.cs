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

        public void AppendLine(string text)
        {
            using (StreamWriter writer = this.OpenForAppend())
            {
                writer.WriteLine(text);
            }
        }

        private StreamWriter OpenForAppend()
        {
            this.Directory.Create();
            return System.IO.File.AppendText(this.Target.FullName);
        }
    }
}
