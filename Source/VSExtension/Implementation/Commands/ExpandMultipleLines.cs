using System;
using VSExtension.Interfaces;

namespace VSExtension.Implementation.Commands
{
    class ExpandMultipleLines : IDemoCommand
    {
        private ISource File;
        private string SnippetShortcut { get; }
        private int StartLineIndex;
        private int EndLineIndex;

        public ExpandMultipleLines(ISource file, string snippetShortcut, int startLineIndex, int endLineIndex)
        {
            this.File = file ?? throw new ArgumentNullException(nameof(file));
            this.SnippetShortcut = snippetShortcut ?? throw new ArgumentNullException(nameof(snippetShortcut));
            this.StartLineIndex = startLineIndex;
            this.EndLineIndex = endLineIndex;
        }

        public bool CanExecute => false;

        public void Execute()
        {
        }
    }
}
