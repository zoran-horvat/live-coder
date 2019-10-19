using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using LiveDemoRunner.Interfaces;

namespace LiveDemoRunner.Infrastructure
{
    public class SnippetsNormalizer
    {
        private ILogger Logger { get; }

        public SnippetsNormalizer(ILogger logger)
        {
            Contract.Requires(logger != null, "Logger must be non-null.");
            this.Logger = logger;
        }

        public void Normalize(FileInfo file)
        {

            string content = this.ReadFile(file);
            string[] snippets = this.GetOriginalShortcuts(content).ToArray();
            string rewrittenContent = this.Rewrite(content, snippets);

            if (rewrittenContent == content)
            {
                this.Logger.Log("Snippets file normalization check OK... nothing to do.");
                return;
            }

            this.Logger.Log("Normalizing snippets file.");
            this.WriteFile(file, rewrittenContent);

        }

        private string ReadFile(FileInfo file)
        {
            file.Refresh();
            byte[] buffer = new byte[file.Length];
            using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                stream.Read(buffer, 0, buffer.Length);
            }
            return Encoding.UTF8.GetString(buffer);
        }

        private void WriteFile(FileInfo file, string rewrittenContent)
        {
            FileInfo destFile = file.CopyTo(file.FullName + ".bak", true);
            byte[] buffer = Encoding.UTF8.GetBytes(rewrittenContent.ToCharArray());
            using (FileStream stream = file.OpenWrite())
            {
                stream.Write(buffer, 0, buffer.Length);
                stream.SetLength(buffer.Length);
            }
        }

        private IEnumerable<string> GetOriginalShortcuts(string content)
        {
            return Regex
                .Matches(content, @"\<Shortcut\>(?<snippet>\w+)\<\/Shortcut\>")
                .Cast<Match>()
                .Select(match => match.Groups["snippet"].Value);
        }

        private string Rewrite(string content, string[] snippets)
        {
            return string.Join(string.Empty, this.GetRewrittenSegments(content, snippets).ToArray());
        }

        private IEnumerable<string> GetRewrittenSegments(string content, string[] snippets)
        {
            int pos = 0;

            while (pos < content.Length)
            {
                var potentialWinner =
                    snippets
                        .Select((snippet, snippetIndex) =>
                            new
                            {
                                Snippet = snippet,
                                Index = content.IndexOf(snippet, pos, StringComparison.InvariantCulture),
                                RewrittenSnippet = $"snp{snippetIndex + 1:00}"
                            }
                        )
                        .Where(pair => pair.Index >= 0)
                        .OrderBy(pair => pair.Index)
                        .ThenByDescending(pair => pair.Snippet.Length)
                        .Take(1);

                if (!potentialWinner.Any())
                {
                    yield return content.Substring(pos);
                    yield break;
                }

                var winner = potentialWinner.Single();

                if (winner.Index > pos)
                    yield return content.Substring(pos, winner.Index - pos);

                yield return winner.RewrittenSnippet;

                pos = winner.Index + winner.Snippet.Length;

            }
        }

    }
}
