using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using LiveCoder.Common;

namespace LiveCoder.Deployer.Implementation.Snippets
{
    class XmlSnippetsReader
    {
        public IEnumerable<XmlSnippet> LoadMany(FileInfo from) =>
            Disposable.Using(() => File.OpenRead(from.FullName)).Map(this.LoadMany);

        private IEnumerable<XmlSnippet> LoadMany(FileStream stream) =>
            this.LoadMany(XDocument.Load(stream));

        private IEnumerable<XmlSnippet> LoadMany(XDocument from) =>
            this.LoadMany(from.Root?.Elements() ?? new XElement[0]);

        private IEnumerable<XmlSnippet> LoadMany(IEnumerable<XElement> elements) =>
            elements.Select(this.ToSnippet);

        private XmlSnippet ToSnippet(XElement snippet) =>
            this.ToSnippet(this.GetShortcut(snippet), this.GetCode(snippet));

        private string GetShortcut(XElement snippet) =>
            snippet.Element(this.Name("Header"))?.Element(this.Name("Shortcut"))?.Value ?? string.Empty;

        private string GetCode(XElement snippet) =>
            snippet.Element(this.Name("Snippet"))?.Element(this.Name("Code"))?.Value ?? string.Empty;

        private XName Name(string of) =>
            XName.Get(of, this.Namespace);

        private string Namespace => 
            @"http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet";

        private XmlSnippet ToSnippet(string shortcut, string code) =>
            new XmlSnippet(this.SnippetNumber(shortcut), code);

        private int SnippetNumber(string shortcut) =>
            int.TryParse(this.RawSnippetNumber(shortcut), out int number) ? number : -1;

        private string RawSnippetNumber(string shortcut) =>
            Regex.Match(shortcut, @"^snp(?<number>\d+)$") is Match match && match.Success
                ? match.Groups["number"].Value
                : string.Empty;

    }
}
