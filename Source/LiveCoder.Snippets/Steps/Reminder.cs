using System;
using System.Collections.Generic;
using LiveCoder.Api;
using LiveCoder.Common.Optional;
using LiveCoder.Snippets.Commands;
using LiveCoder.Snippets.Interfaces;

namespace LiveCoder.Snippets.Steps
{
    class Reminder : IDemoStep
    {
        private ILogger Logger { get; }
        public string SnippetShortcut { get; }
        private ISource File { get; }
        private int LineIndex { get; }
        private string Text { get; }

        private string LineContent =>
            this.File.GetTextBetween(this.LineIndex, this.LineIndex)
                .FirstOrNone()
                .Reduce(string.Empty);

        public Reminder(ILogger logger, string snippetShortcut, ISource file, int lineIndex, string text)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.SnippetShortcut = snippetShortcut ?? throw new ArgumentNullException(nameof(snippetShortcut));
            this.File = file ?? throw new ArgumentNullException(nameof(file));
            this.LineIndex = lineIndex >= 0 ? lineIndex : throw new ArgumentException("Line index must be non-negative.");
            this.Text = text;
        }

        public IEnumerable<IDemoCommand> GetCommands(CodeSnippets script) =>
            new IDemoCommand[]
            {
                new OpenDocument(this.File),
                new ActivateDocument(this.File),
                new MoveToLine(this.File, this.LineIndex),
                new ShowMessage(this.Logger, this.Text),
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
