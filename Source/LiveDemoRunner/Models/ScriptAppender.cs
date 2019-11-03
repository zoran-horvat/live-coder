using System.IO;
using System.Xml;
using LiveDemoRunner.Interfaces;

namespace LiveDemoRunner.Models
{
    internal class ScriptAppender : IDestination
    {
        private FileInfo ScriptFile { get; }
        private DirectoryInfo Directory => this.ScriptFile.Directory;
        
        public ScriptAppender(FileInfo scriptFile)
        {
            this.ScriptFile = scriptFile;
        }

        public void DeployFile(string name, Stream stream)
        {
        }

        public void DeployDirectory(string name)
        {
            this.GetSubdirectory(name).Create();
        }

        public IDestination GetDirectory(string name) => 
            new FileSystemDirectory(this.Directory);

        public IDeployedComponent GetFile(string name)
        {
            string path = Path.Combine(this.Directory.FullName, name);
            FileInfo file = new FileInfo(path);
            return new DestinationFile(file);
        }

        public bool DirectoryExists(string name)
        {
            throw new System.NotImplementedException();
        }

        public bool FileExists(string name)
        {
            throw new System.NotImplementedException();
        }

        private string GetSubdirectoryPath(string name) => Path.Combine(this.Directory.FullName, name);

        private DirectoryInfo GetSubdirectory(string name) => new DirectoryInfo(this.GetSubdirectoryPath(name));
    }
}
