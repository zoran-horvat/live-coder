using System;
using LiveCoder.Common.Optional;
using LiveCoder.Extension.Interfaces;
using LiveCoder.Extension.Scripting.Elements;

namespace LiveCoder.Extension.Implementation.Commands
{
    class ExpandSelection : IDemoCommand
    {
        private ISource File { get; }
        private string SnippetShortcut { get; }
        private Option<string> SnippetContent { get; }

        public ExpandSelection(ISource file, string snippetShortcut, Option<Snippet> snippet)
        {
            this.File = file ?? throw new ArgumentNullException();
            this.SnippetShortcut = snippetShortcut ?? throw new ArgumentNullException();
            this.SnippetContent = snippet.Map(s => s.Content);
        }

        public void Execute() =>
            this.File.ReplaceSelectionWithSnippet(this.SnippetShortcut, this.SnippetContent);
    }
}
