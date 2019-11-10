using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace LiveCoder.Deployer.Models
{
    class XmlSnippetsReader
    {
        public IEnumerable<XmlSnippet> LoadMany(Stream from)
        {
            using (XmlReader reader = XmlReader.Create(from))
            {
                while (reader.Read())
                {
                    foreach (XmlSnippet snippet in TryReadSnippet(reader))
                        yield return snippet;
                }
            }
        }

        private IEnumerable<XmlSnippet> TryReadSnippet(XmlReader reader)
        {
            if (reader.Name != "CodeSnippet")
                yield break;

            string rawXml = reader.ReadOuterXml();
            XmlElement snippetElement = GetSnippetElement(rawXml);

            using (Stream output = new MemoryStream())
            {
                using (XmlWriter writer = CreateXmlWriter(output))
                {
                    writer.WriteStartElement("CodeSnippets", "http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet");
                    snippetElement.WriteTo(writer);
                    writer.WriteEndElement();
                }

                output.Position = 0;
                yield return new XmlSnippet(output);
            }
        }

        private XmlElement GetSnippetElement(string rawXml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(rawXml);
            return doc.DocumentElement;
        }

        private XmlWriter CreateXmlWriter(Stream output) =>
            XmlWriter.Create(output, XmlWriterSettings);

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

    }
}
