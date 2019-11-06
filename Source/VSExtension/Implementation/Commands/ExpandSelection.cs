using System;
using LiveCoderExtension.Interfaces;

namespace LiveCoderExtension.Implementation.Commands
{
    class ExpandSelection : IDemoCommand
    {
        private ISource File { get; }
        private string SnippetShortcut { get; }

        public ExpandSelection(ISource file, string snippetShortcut)
        {
            this.File = file ?? throw new ArgumentNullException();
            this.SnippetShortcut = snippetShortcut ?? throw new ArgumentNullException();
        }

        public void Execute() =>
            this.File.ReplaceSelectionWithSnippet(this.SnippetShortcut);
    }
}
