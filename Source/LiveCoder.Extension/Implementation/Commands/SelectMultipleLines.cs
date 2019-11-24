using System;
using LiveCoder.Extension.Interfaces;
using LiveCoder.Scripting;

namespace LiveCoder.Extension.Implementation.Commands
{
    class SelectMultipleLines : IDemoCommand
    {
        private ISource Document { get; }
        private int StartLineIndex { get; }
        private int EndLineIndex { get; }

        public SelectMultipleLines(ISource document, int startLineIndex, int endLineIndex)
        {
            this.Document = document ?? throw new ArgumentNullException(nameof(document));
            this.StartLineIndex = startLineIndex >= 0 ? startLineIndex : throw new ArgumentException("Line index must be non-negative.");
            this.EndLineIndex = endLineIndex >= 0 ? endLineIndex : throw new ArgumentException("Line index must be non-negative.");
        }

        public void Execute()
        {
            this.Document.SelectLines(this.StartLineIndex, this.EndLineIndex);
        }

        public override string ToString() => $"select lines {this.StartLineIndex}-{this.EndLineIndex} in {this.Document}";
    }
}
