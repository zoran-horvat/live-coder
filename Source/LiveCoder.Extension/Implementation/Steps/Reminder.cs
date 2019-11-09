using System;
using System.Collections.Generic;
using LiveCoder.Common.Optional;
using LiveCoder.Extension.Implementation.Commands;
using LiveCoder.Extension.Interfaces;

namespace LiveCoder.Extension.Implementation.Steps
{
    class Reminder : IDemoStep
    {
        public string SnippetShortcut { get; }
        private ISource File { get; }
        private int LineIndex { get; }

        private string LineContent =>
            this.File.GetTextBetween(this.LineIndex, this.LineIndex)
                .FirstOrNone()
                .Reduce(string.Empty);

        public Reminder(string snippetShortcut, ISource file, int lineIndex)
        {
            this.SnippetShortcut = snippetShortcut ?? throw new ArgumentNullException(nameof(snippetShortcut));
            this.File = file ?? throw new ArgumentNullException(nameof(file));
            this.LineIndex = lineIndex >= 0 ? lineIndex : throw new ArgumentException("Line index must be non-negative.");
        }

        public IEnumerable<IDemoCommand> Commands =>
            new IDemoCommand[]
            {
                new OpenDocument(this.File),
                new ActivateDocument(this.File),
                new MoveToLine(this.File, this.LineIndex),
                new Pause(),
                new VerifyActiveDocument(this.File),
                new VerifyCursorPosition(this.File, this.LineIndex),
                new VerifyLineContent(this.File, this.LineIndex, this.LineContent),
                new DeleteLine(this.File, this.LineIndex)
            };

        public string Label => $"Reminder {this.SnippetShortcut} in {this.File.Name} on line {this.LineIndex}";

        public override string ToString() => this.Label;
    }
}
