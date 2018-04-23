using System;
using VSExtension.Interfaces;

namespace VSExtension.Implementation.Commands
{
    class ExpandLine : IDemoCommand
    {
        private ISource File { get; }
        private string SnippetShortcut { get; }
        private int LineIndex { get; }

        public ExpandLine(ISource file, string snippetShortcut, int lineIndex)
        {
            this.File = file ?? throw new ArgumentNullException();
            this.SnippetShortcut = snippetShortcut ?? throw new ArgumentNullException();
            this.LineIndex = lineIndex;
        }

        public void Execute() =>
            this.File.ReplaceSelectionWithSnippet(this.SnippetShortcut);
    }
}
