using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace LiveDemoRunner.Models
{
    class XmlSnippet
    {
        public int Number => this.ParseNumber();
        public string Code => this.ParseCode();

        private XmlDocument Document { get; }
        private XmlNamespaceManager Namespaces { get; }

        private static readonly string XpathNamespacePrefix = "x";
        private static readonly string SnippetsNamespace = @"http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet";
        private static readonly string ShortcutXpath = @"//x:CodeSnippets/x:CodeSnippet/x:Header/x:Shortcut/text()";
        private static readonly string ShortcutNumberPattern = @"^snp(?<number>\d+)$";
        private static readonly string CodeXpath = @"//x:CodeSnippets/x:CodeSnippet/x:Snippet/x:Code/text()";

        public XmlSnippet(Stream loadFrom)
        {
            loadFrom.Position = 0;
            using (XmlReader reader = new XmlTextReader(loadFrom))
            {
                this.Document = new XmlDocument();
                this.Document.Load(reader);

                this.Namespaces = new XmlNamespaceManager(this.Document.NameTable);
                this.Namespaces.AddNamespace(XpathNamespacePrefix, SnippetsNamespace);
            }
        }

        private int ParseNumber() =>
            this.ParseNumber(
                this.Document.SelectSingleNode(ShortcutXpath, this.Namespaces)?.Value ?? string.Empty);

        private int ParseNumber(string shortcut) =>
            Regex.Match(shortcut, ShortcutNumberPattern) is Match match &&
            match.Success &&
            int.TryParse(match.Groups["number"].Value, out int value)
                ? value
                : -1;

        private string ParseCode() =>
            this.Document.SelectSingleNode(CodeXpath, this.Namespaces)?.Value ?? string.Empty;
    }
}
