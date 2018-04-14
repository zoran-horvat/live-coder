using System;
using VSExtension.Interfaces;

namespace VSExtension.Implementation.Commands
{
    class SelectLine : IDemoCommand
    {
        private ISource Document { get; }
        private int LineIndex { get; }

        public SelectLine(ISource document, int lineIndex)
        {
            this.Document = document ?? throw new ArgumentNullException(nameof(document));
            this.LineIndex = lineIndex >= 0 ? lineIndex : throw new ArgumentException("Line index must be non-negative.");
        }

        public bool CanExecute =>
            this.Document.CursorLineIndex.Map(line => line == this.LineIndex).Reduce(false);

        public void Execute() => this.Document.SelectLine(this.LineIndex);

        public override string ToString() => $"select line {this.LineIndex} in {this.Document}";
    }
}
