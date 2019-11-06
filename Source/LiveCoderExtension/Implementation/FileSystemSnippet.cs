using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using LiveCoderExtension.Functional;
using LiveCoderExtension.Interfaces;

namespace LiveCoderExtension.Implementation
{
    class FileSystemSnippet : ISnippet
    {
        private FileInfo File { get; }

        public FileSystemSnippet(FileInfo file)
        {
            this.File = file ?? throw new ArgumentNullException(nameof(file));
        }

        private XNamespace XmlNamespace =>
            "http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet";

        public Option<string> Content =>
            this.XmlSnippet.Map(text => text.Value);

        private Option<XText> XmlSnippet =>
            this.XmlDocument.Map(this.GetCode).Reduce(None.Value);

        private Option<XText> GetCode(XDocument document) =>
            document.DescendantNodes()
                .OfType<XText>()
                .Where(text => text.Parent?.Name == this.XmlNamespace + "Code")
                .FirstOrNone();

        private Option<Stream> OpenFile() =>
            this.File.Exists
                ? (Option<Stream>)this.File.Open(FileMode.Open, FileAccess.Read, FileShare.Read)
                : None.Value;

        private Option<TextReader> CreateFileReader() =>
            this.OpenFile().Map<TextReader>(stream => new StreamReader(stream, Encoding.UTF8, true, 1000, false));

        private Option<XDocument> XmlDocument
        {
            get
            {
                using (Option<TextReader> reader = CreateFileReader())
                {
                    return reader.Map(XDocument.Load);
                }
            }
        }
    }
}
