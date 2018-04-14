using System;
using System.Linq;
using VSExtension.Functional;
using VSExtension.Interfaces;

namespace VSExtension.Implementation.Commands
{
    class DeleteLine : IDemoCommand
    {
        private ISource Document { get; }
        private int LineIndex { get; }
        private string ExpectedLineContent { get; }

        public DeleteLine(ISource document, int lineIndex, string expectedContent)
        {
            this.Document = document ?? throw new ArgumentNullException(nameof(document));
            this.LineIndex = lineIndex >= 0 ? lineIndex : throw new ArgumentException("Line index must be non-negative.");
            this.ExpectedLineContent = expectedContent ?? string.Empty;
        }

        public void Execute() => this.DeletionStrategy();

        private Action DeletionStrategy => this.ShouldDeleteLine
            ? (Action)(() => this.Document.DeleteLine(this.LineIndex))
            : () => { };

        private bool ShouldDeleteLine =>
            this.IsDocumentActive && this.IsCursorOnLine && this.IsContentEqual;

        private bool IsCursorOnLine =>
            this.Document.CursorLineIndex.Map(line => line == this.LineIndex).Reduce(false);

        private bool IsDocumentActive =>
            this.Document.IsActive;

        private bool IsContentEqual =>
            this.ContainsTargetLine && this.CurrentLineContent == this.ExpectedLineContent;

        public bool ContainsTargetLine =>
            this.Document.TextBetween(this.LineIndex, this.LineIndex).Any();

        public string CurrentLineContent =>
            this.Document.TextBetween(this.LineIndex, this.LineIndex).First();

        public override string ToString() => $"delete line {this.LineIndex} in {this.Document}";
    }
}
