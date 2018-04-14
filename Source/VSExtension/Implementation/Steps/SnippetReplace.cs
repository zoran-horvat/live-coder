using System;
using System.Collections.Generic;
using VSExtension.Implementation.Commands;
using VSExtension.Interfaces;

namespace VSExtension.Implementation.Steps
{
    class SnippetReplace : IDemoStep
    {
        public string SnippetShortcut { get; }
        private ISource File { get; }
        private int LineIndex { get; }

        public SnippetReplace(string snippetShortcut, ISource file, int lineIndex)
        {
            this.SnippetShortcut = snippetShortcut ?? throw new ArgumentNullException(nameof(snippetShortcut));
            this.File = file ?? throw new ArgumentNullException(nameof(file));
            this.LineIndex = lineIndex >= 0 ? lineIndex : throw new ArgumentException("Line index must be non-negative.");
        }

        public MultilineSnippetReplace EndsOnLine(int index) =>
            new MultilineSnippetReplace(this.SnippetShortcut, this.File, this.LineIndex, index - this.LineIndex + 1);

        public IEnumerable<IDemoCommand> Commands =>
            new IDemoCommand[]
            {
                new OpenDocument(this.File),
                new ActivateDocument(this.File),
                new MoveToLine(this.File, this.LineIndex),
                new Pause(),
                new SelectLine(this.File, this.LineIndex),
                new Pause(),
                new ExpandLine(this.File, this.SnippetShortcut, this.LineIndex)
            };

        public override string ToString() =>
            $"{this.SnippetShortcut} in {this.File.Name} line {this.LineIndex}";
    }
}
