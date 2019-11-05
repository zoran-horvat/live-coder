using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace LiveDemoRunner.Models
{
    class XmlSnippet
    {
        public int Number => this.ParseNumber();
        public string Code => this.ParseCode();

        private XPathDocument Document { get; }

        private static readonly string XpathNamespacePrefix = "x";
        private static readonly string ShortcutXpath = @"//x:CodeSnippets/x:CodeSnippet/x:Header/x:Shortcut/text()";
        private static readonly string ShortcutNumberPattern = @"^snp(?<number>\d+)$";
        private static readonly string CodeXpath = @"//x:CodeSnippets/x:CodeSnippet/x:Snippet/x:Code/text()";

        public XmlSnippet(Stream loadFrom)
        {
            loadFrom.Position = 0;
            using (XmlReader reader = new XmlTextReader(loadFrom))
            {
                this.Document = new XPathDocument(reader);
            }
        }

        private string EvaluateXpath(XPathDocument document, string xpath, string namespacePrefix)
        {
            XPathNavigator navigator = document.CreateNavigator();
            XPathExpression query = navigator.Compile(xpath);
            XmlNamespaceManager manager = new XmlNamespaceManager(navigator.NameTable);
            manager.AddNamespace(XpathNamespacePrefix, "http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet");
            query.SetContext(manager);
            XPathNavigator result = navigator.SelectSingleNode(query);
            return result?.Value ?? string.Empty;
        }

        private int ParseNumber() =>
            this.ParseNumber(this.EvaluateXpath(this.Document, ShortcutXpath, XpathNamespacePrefix));

        private int ParseNumber(string shortcut)
        {
            Match number = Regex.Match(shortcut, ShortcutNumberPattern);
            if (number.Success && int.TryParse(number.Groups["number"].Value, out int value))
                return value;
            return -1;
        }

        private string ParseCode() =>
            this.EvaluateXpath(this.Document, CodeXpath, XpathNamespacePrefix);
    }
}
