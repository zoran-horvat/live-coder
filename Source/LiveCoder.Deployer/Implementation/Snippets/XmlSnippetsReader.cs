using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using LiveCoder.Common.Optional;
using LiveCoder.Common.Text;
using LiveCoder.Common.Text.Regex;
using LiveCoder.Common.Xml;

namespace LiveCoder.Deployer.Implementation.Snippets
{
    class XmlSnippetsReader
    {
        private FileInfo File { get; }

        public XmlSnippetsReader(FileInfo file)
        {
            this.File = file;
        }

        public IEnumerable<XmlSnippet> LoadMany() =>
            this.LoadMany(this.File.LoadXml());

        public Option<IEnumerable<XmlSnippet>> LoadManyConcurrent() =>
            this.File.TryLoadConcurrent().Map(this.LoadMany);

        private IEnumerable<XmlSnippet> LoadMany(XDocument from) =>
            from.RootChildren().MapOptional(this.ToSnippet);

        private Option<XmlSnippet> ToSnippet(XElement snippet) =>
            from snippetNumber in snippet.ValueOf("Header", "Shortcut").Extract("number", @"^snp(?<number>\d+)$").ParseInt()
            from code in snippet.ValueOf("Snippet", "Code")
            select new XmlSnippet(snippetNumber, code);
    }
}
