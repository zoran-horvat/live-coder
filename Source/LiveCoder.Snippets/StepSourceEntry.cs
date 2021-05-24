﻿namespace LiveCoder.Snippets
{
    class StepSourceEntry
    {
        public string SnippetShortcut { get; }
        public int LineIndex { get; }
        public string Description { get; }

        public StepSourceEntry(string snippetShortcut, int lineIndex, string description)
        {
            this.SnippetShortcut = snippetShortcut;
            this.LineIndex = lineIndex;
            this.Description = description;
        }
    }
}
