using System;
using VSExtension.Interfaces;

namespace VSExtension.Implementation.Steps
{
    class MultilineSnippetReplace : IDemoStep
    {
        public string SortKey { get; }
        private ISource File { get; }
        private int StartLineIndex { get; }
        private int LinesCount { get; }

        public MultilineSnippetReplace(string sortKey, ISource file, int startLineIndex, int linesCount)
        {
            this.SortKey = sortKey ?? throw new ArgumentNullException(nameof(sortKey));
            this.File = file ?? throw new ArgumentNullException(nameof(file));
            this.StartLineIndex = startLineIndex >= 0 ? startLineIndex : throw new ArgumentException("Start line index must be non-negative.");
            this.LinesCount = linesCount > 0 ? linesCount : throw new ArgumentException("Number of lines must be positive.");
        }

        public override string ToString() =>
            $"{this.SortKey} in {this.File.Name} lines {this.StartLineIndex}-{this.StartLineIndex + this.LinesCount - 1}";
    }
}
