using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiveCoder.Deployer.Tool.Interfaces;

namespace LiveCoder.Deployer.Tool.Models
{
    internal class XmlSnippetsPublisher : IDestination
    {
        public FileInfo ScriptFile { get; }
        private DirectoryInfo Directory => this.ScriptFile.Directory;
        
        public XmlSnippetsPublisher(FileInfo scriptFile)
        {
            this.ScriptFile = scriptFile;
        }

        public void DeployFile(string name, Stream stream) => 
            this.Write(this.ToScript(stream));

        private void Write(IEnumerable<string> script)
        {
            if (this.ScriptFile is FileInfo file)
            {
                file.Directory?.Create();
                System.IO.File.WriteAllLines(file.FullName, script);
            }
        }

        private IEnumerable<string> ToScript(Stream inputSnippets) =>
            new XmlSnippetsReader().LoadMany(inputSnippets)
                .SelectMany(this.ToScript);

        private IEnumerable<string> ToScript(XmlSnippet snippet) =>
            this.ToScript(snippet.Number, snippet.Code);

        private IEnumerable<string> ToScript(int snippetNumber, string content)
        {
            string endSnippet = "<-- end snippet";

            string snippetCommand = $"snippet {snippetNumber:00} until {endSnippet}";

            IEnumerable<string> command = snippetNumber == 1 
                ? new[] {snippetCommand} 
                : new[] {string.Empty, snippetCommand};
            
            string[] contentLines = content.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None);
            contentLines[contentLines.Length - 1] = contentLines[contentLines.Length - 1] + endSnippet;

            return command.Concat(contentLines);
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
            System.IO.Directory.Exists(Path.Combine(this.Directory.FullName, name));

        public bool FileExists(string name) =>
            System.IO.File.Exists(Path.Combine(this.Directory.FullName, name));

        private string GetSubdirectoryPath(string name) => 
            Path.Combine(this.Directory.FullName, name);

        private DirectoryInfo GetSubdirectory(string name) => 
            new DirectoryInfo(this.GetSubdirectoryPath(name));
    }
}
