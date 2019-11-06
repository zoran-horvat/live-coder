using System;
using LiveCoderExtension.Interfaces;

namespace LiveCoderExtension.Implementation.Commands
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

        public void Execute() => this.Document.SelectLine(this.LineIndex);

        public override string ToString() => $"select line {this.LineIndex} in {this.Document}";
    }
}
