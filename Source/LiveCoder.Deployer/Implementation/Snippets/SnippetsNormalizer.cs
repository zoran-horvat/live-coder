using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using LiveCoder.Common.IO;

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
            this.FixShortcuts(line, shortcutToRewrite, new Regex(@"snp\d+[a-z]?"), 0, string.Empty);

        private string FixShortcuts(string line, IDictionary<string, string> shortcutToRewrite, Regex pattern, int pos, string rewritten) =>
            pos >= line.Length ? rewritten
            : pattern.Match(line, pos) is Match match && match.Success ? this.FixShortcuts(line, shortcutToRewrite, pattern, pos, rewritten, match)
            : rewritten + line.Substring(pos);

        private string FixShortcuts(string line, IDictionary<string, string> shortcutToRewrite, Regex pattern, int pos, string rewritten, Match match) =>
            match.Index > pos ? this.FixShortcuts(line, shortcutToRewrite, pattern, match.Index, rewritten + line.Substring(pos, match.Index - pos), match)
            : shortcutToRewrite.TryGetValue(match.Value, out string toReplace) ? this.FixShortcuts(line, shortcutToRewrite, pattern, match.Index + match.Length, rewritten + toReplace)
            : this.FixShortcuts(line, shortcutToRewrite, pattern, match.Index + match.Length, rewritten + match.Value);

        private bool RewriteFile(string[] originalLines, string[] rewrittenLines)
        {
            if (originalLines.SequenceEqual(rewrittenLines)) return false;
            this.SnippetsFile.RewriteIfModified(rewrittenLines, Encoding.UTF8);
            return true;
        }

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
