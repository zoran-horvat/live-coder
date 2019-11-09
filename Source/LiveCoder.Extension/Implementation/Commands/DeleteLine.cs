using System;
using LiveCoder.Extension.Interfaces;

namespace LiveCoder.Extension.Implementation.Commands
{
    class DeleteLine : IDemoCommand
    {
        private ISource Document { get; }
        private int LineIndex { get; }

        public DeleteLine(ISource document, int lineIndex)
        {
            this.Document = document ?? throw new ArgumentNullException(nameof(document));
            this.LineIndex = lineIndex >= 0 ? lineIndex : throw new ArgumentException("Line index must be non-negative.");
        }

        public void Execute() => this.Document.DeleteLine(this.LineIndex);

        public override string ToString() => $"delete line {this.LineIndex} in {this.Document}";
    }
}
