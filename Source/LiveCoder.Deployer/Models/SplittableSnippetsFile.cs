using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
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
            using (XmlReader reader = XmlReader.Create(stream))
            {
                while (reader.Read())
                {
                    foreach (IDeployedComponent deployed in this.DeploySingleSnippet(reader, toDestination))
                        yield return deployed;
                }
            }
        }

        private IEnumerable<IDeployedComponent> DeploySingleSnippet(XmlReader reader, IDestination toDestination)
        {
            if (reader.Name != "CodeSnippet")
                yield break;

            string rawXml = reader.ReadOuterXml();
            XmlElement snippetElement = GetSnippetElement(rawXml);
            string fileName = GetSnippetFileName(snippetElement);

            using (Stream output = new MemoryStream())
            {
                using (XmlWriter writer = this.CreateXmlWriter(output))
                {
                    writer.WriteStartElement("CodeSnippets", "http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet");
                    snippetElement.WriteTo(writer);
                    writer.WriteEndElement();
                }

                output.Position = 0;
                toDestination.DeployFile(fileName, output);
                yield return toDestination.GetFile(fileName);
            }
        }

        private XmlElement GetSnippetElement(string rawXml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(rawXml);
            return doc.DocumentElement;
        }

        private XmlWriter CreateXmlWriter(Stream output) =>
            XmlWriter.Create(output, this.XmlWriterSettings);

        private XmlWriterSettings XmlWriterSettings =>
            new XmlWriterSettings()
            {
                Encoding = Encoding.UTF8,
                Indent = true,
                IndentChars = "    ",
                CloseOutput = false,
                NewLineOnAttributes = false,
                CheckCharacters = true
            };

        private string GetSnippetFileName(XmlElement snippetElement) =>
            $"{this.FileNameNoExtension}_{this.GetSnippetShortcut(snippetElement)}{this.FileExtension}";

        private string GetSnippetShortcut(XmlElement snippetElement) =>
            snippetElement["Header"]?["Shortcut"]?.FirstChild?.Value ?? string.Empty;
    }
}
