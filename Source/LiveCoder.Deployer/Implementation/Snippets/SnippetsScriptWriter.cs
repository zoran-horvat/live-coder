using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiveCoder.Common;

namespace LiveCoder.Deployer.Implementation.Snippets
{
    class SnippetsScriptWriter
    {
        private FileInfo File { get; }
     
        public SnippetsScriptWriter(FileInfo file)
        {
            this.File = file;
        }

        public void Write(IEnumerable<XmlSnippet> snippets) =>
            System.IO.File.WriteAllLines(this.File.FullName, this.GetScriptLines(snippets));

        private IEnumerable<string> GetScriptLines(IEnumerable<XmlSnippet> snippets) =>
            snippets.Select(this.GetScriptLines)
                .SelectMany(snippet => this.BlankLine.Concat(snippet))
                .Skip(1);

        private IEnumerable<string> GetScriptLines(XmlSnippet snippet) =>
            this.BeginSnippetLine(snippet)
                .Concat(snippet.CodeLines.AppendToLast(this.EndSnippet));

        private IEnumerable<string> BlankLine => 
            new[] { string.Empty };

        private IEnumerable<string> BeginSnippetLine(XmlSnippet snippet) =>
            new[] {this.BeginSnippetStatement(snippet)};

        private string BeginSnippetStatement(XmlSnippet snippet) =>
            $"snippet {snippet.Number:00} until {this.EndSnippet}";

        private string EndSnippet => 
            "<-- end snippet";
    }
}
