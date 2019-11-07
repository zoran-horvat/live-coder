using System.IO;
using LiveDemoRunner.Interfaces;

namespace LiveDemoRunner.Models
{
    internal class XmlSnippetPublisher : IDestination
    {
        private TextFileAppender ScriptFile { get; }
        private DirectoryInfo Directory => this.ScriptFile.Directory;
        
        public XmlSnippetPublisher(FileInfo scriptFile)
        {
            this.ScriptFile = new TextFileAppender(scriptFile);
        }

        public void DeployFile(string name, Stream stream) =>
            this.Deploy(new XmlSnippet(stream));

        private void Deploy(XmlSnippet snippet) =>
            this.Deploy(snippet.Number, snippet.Code);

        private void Deploy(int snippetNumber, string content)
        {
            string endSnippet = "<-- end snippet";
            this.ScriptFile.AppendLine($"snippet {snippetNumber:00} until {endSnippet}");
            this.ScriptFile.AppendLine(content + endSnippet);
            this.ScriptFile.AppendLine(string.Empty);
        }

        public void DeployDirectory(string name) => 
            this.GetSubdirectory(name).Create();

        public IDestination GetDirectory(string name) => 
            new FileSystemDirectory(this.Directory);

        public IDeployedComponent GetFile(string name)
        {
            string path = Path.Combine(this.Directory.FullName, name);
            FileInfo file = new FileInfo(path);
            return new DestinationFile(file);
        }

        public bool DirectoryExists(string name) =>
            new DirectoryInfo(Path.Combine(this.Directory.FullName, name)).Exists;

        public bool FileExists(string name) => 
            new FileInfo(Path.Combine(this.Directory.FullName, name)).Exists;

        private string GetSubdirectoryPath(string name) => 
            Path.Combine(this.Directory.FullName, name);

        private DirectoryInfo GetSubdirectory(string name) => 
            new DirectoryInfo(this.GetSubdirectoryPath(name));
    }
}
