using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using LiveCoder.Common.Optional;
using LiveCoder.Deployer.Tool.Interfaces;

namespace LiveCoder.Deployer.Tool.Infrastructure
{
    public class SnippetsNormalizer
    {
        private ILogger Logger { get; }

        public SnippetsNormalizer(ILogger logger)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
            int retries = 10;
            int waitMsec = 100;
            string content = string.Empty;

            for (int retry = 0; retry < retries; retry++)
            {
                content = this.ReadFileRaw(file);
                if (content.Length > 0)
                    return content;
                Thread.Sleep(waitMsec);
            }

            return content;
        }

        private string ReadFileRaw(FileInfo file)
        {
            using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (TextReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private void WriteFile(FileInfo file, string rewrittenContent)
        {
            file.CopyTo(file.FullName + ".bak", true);
            byte[] buffer = Encoding.UTF8.GetBytes(rewrittenContent.ToCharArray());
            using (FileStream stream = File.Open(file.FullName, FileMode.Create))
            {
                stream.Write(buffer, 0, buffer.Length);
                stream.SetLength(buffer.Length);
            }
        }

        private IEnumerable<(string shortcut, string becomes)> GetShortcutRewrites(string content) =>
            this.GetOriginalShortcuts(content)
                .Select((shortcut, index) => (shortcut: shortcut, becomes: $"snp{index + 1:00}"))
                .Where(pair => pair.shortcut != pair.becomes);

        private IEnumerable<string> GetOriginalShortcuts(string content) =>
            Regex
                .Matches(content, @"\<Shortcut\>(?<snippet>\w+)\<\/Shortcut\>")
                .Cast<Match>()
                .Select(match => match.Groups["snippet"].Value);

        private string Rewrite(string content, string[] snippets)
        {
            List<(string shortcut, string becomes)> changes = this.GetShortcutRewrites(content).ToList();
            if (changes.Count == 0) return content;

            StringBuilder newContent = new StringBuilder();

            int pos = 0;
            while (pos < content.Length)
            {
                Option<(string shortcut, string becomes, int at)> possibleUpdate =
                    changes.MapOptional(change => content
                        .IndexOfOrNone(change.shortcut, pos)
                        .Map(occurencePos => (shortcut: change.shortcut, becomes: change.becomes, at: occurencePos)))
                        .WithMinOrNone(tuple => tuple.at);

                if (possibleUpdate is Some<(string shortcut, string becomes, int at)> some)
                {
                    (string shortcut, string becomes, int at) = some.Content;
                    if (at > pos) newContent.Append(content.Substring(pos, at - pos));
                    newContent.Append(becomes);
                    pos = at + shortcut.Length;
                }
                else
                {
                    newContent.Append(content.Substring(pos));
                    pos = content.Length;
                }
            }

            return newContent.ToString();
        }
    }
}
