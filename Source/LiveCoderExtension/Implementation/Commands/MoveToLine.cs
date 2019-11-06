using System;
using LiveCoderExtension.Interfaces;

namespace LiveCoderExtension.Implementation.Commands
{
    class MoveToLine : IDemoCommand
    {
        private ISource Document { get; }
        private int LineNumber { get; }

        public MoveToLine(ISource document, int lineNumber)
        {
            this.Document = document ?? throw new ArgumentNullException(nameof(document));
            LineNumber = lineNumber >= 0 ? lineNumber : throw new ArgumentException("Line number must be non-negative.");
        }

        public void Execute() => this.Document.MoveSelectionToLine(this.LineNumber);

        public override string ToString() => $"move to line {this.LineNumber} in {this.Document}";
    }
}
