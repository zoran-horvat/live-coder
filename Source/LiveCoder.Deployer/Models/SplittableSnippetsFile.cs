using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiveCoder.Deployer.Interfaces;

namespace LiveCoder.Deployer.Models
{
    public class SplittableSnippetsFile: File
    {

        private string FileNameNoExtension { get; }
        private string FileExtension { get; }

        public SplittableSnippetsFile(FileInfo file, ILogger logger, Action<FileInfo> beforeDeploy) : base(file, logger, beforeDeploy)
        {
            this.FileExtension = file.Extension;
            this.FileNameNoExtension = file.Name.Substring(0, file.Name.Length - file.Extension.Length);
        }

        protected override IEnumerable<IDeployedComponent> Deploy(Stream stream, IDestination toDestination)
        {
            if (toDestination is XmlSnippetsPublisher snippetsPublisher)
            {
                toDestination.DeployFile(snippetsPublisher.ScriptFile.Name, stream);
                return new[] {new DestinationFile(snippetsPublisher.ScriptFile)};
            }

            return Enumerable.Empty<IDeployedComponent>();
        }
    }
}
