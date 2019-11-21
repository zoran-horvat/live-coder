using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using LiveCoder.Common;
using LiveCoder.Common.IO;
using LiveCoder.Common.Optional;
using LiveCoder.Common.Text.Regex;

namespace LiveCoder.Deployer.Implementation.Snippets
{
    public class SnippetsNormalizer
    {
        private FileInfo SnippetsFile { get; }
        
        public SnippetsNormalizer(FileInfo snippetsFile)
        {
            this.SnippetsFile = snippetsFile;
        }

        public bool Normalize() =>
            this.SnippetsFile.TryReadAllLines(Encoding.UTF8).Map(this.Normalize).Reduce(false);

        private bool Normalize(string[] lines) =>
            this.RewriteFile(lines, this.FixShortcuts(lines));

        private string[] FixShortcuts(string[] lines) =>
            this.FixShortcuts(lines, this.GetShortcutRewrites(lines));

        private string[] FixShortcuts(string[] lines, IDictionary<string, string> shortcutToRewrite) =>
            lines.Select(line => this.FixShortcuts(line, shortcutToRewrite)).ToArray();

        private string FixShortcuts(string line, IDictionary<string, string> shortcutToRewrite) =>
            line.SplitIncludeSeparators(new Regex(@"snp\d+[a-z]?"))
                .Select(segment => shortcutToRewrite.TryGetValue(segment).Reduce(segment))
                .Join();

        private bool RewriteFile(string[] originalLines, string[] rewrittenLines)
        {
            if (originalLines.SequenceEqual(rewrittenLines)) return false;
            this.BackupSnippets();
            this.SnippetsFile.RewriteIfModified(rewrittenLines, Encoding.UTF8);
            return true;
        }

        private void BackupSnippets() => 
            File.Copy(this.SnippetsFile.FullName, this.SnippetsFileBackup.FullName, true);

        private FileInfo SnippetsFileBackup =>
            new FileInfo(this.SnippetsFile.FullName + ".bak");

        private IDictionary<string, string> GetShortcutRewrites(string[] lines) =>
            this.GetOriginalShortcuts(lines)
                .Select((shortcut, index) => (shortcut: shortcut, becomes: $"snp{index + 1:00}"))
                .Where(pair => pair.shortcut != pair.becomes)
                .ToDictionary(pair => pair.shortcut, pair => pair.becomes);

        private IEnumerable<string> GetOriginalShortcuts(string[] content) =>
            content.SelectMany(this.GetOriginalShortcuts);

        private IEnumerable<string> GetOriginalShortcuts(string line) =>
            Regex
                .Matches(line, @"\<Shortcut\>(?<snippet>\w+)\<\/Shortcut\>")
                .Cast<Match>()
                .Select(match => match.Groups["snippet"].Value);
    }
}
