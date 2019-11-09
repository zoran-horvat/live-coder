using System;
using LiveCoder.Extension.Interfaces;

namespace LiveCoder.Extension.Implementation.Commands
{
    class ExpandSelection : IDemoCommand
    {
        private ISource File { get; }
        private string SnippetContent { get; }

        public ExpandSelection(ISource file, string snippetContent)
        {
            this.File = file ?? throw new ArgumentNullException(nameof(file));
            this.SnippetContent = snippetContent ?? throw new ArgumentNullException(nameof(snippetContent));
        }

        public void Execute() =>
            this.File.ReplaceSelectionWith(this.SnippetContent);
    }
}
