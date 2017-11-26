using System;
using VSExtension.Interfaces;

namespace VSExtension.Implementation
{
    class Reminder : IDemoStep
    {
        public string SortKey { get; }
        private ISource File { get; }
        private int LineIndex { get; }

        public Reminder(string sortKey, ISource file, int lineIndex)
        {
            this.SortKey = sortKey ?? throw new ArgumentNullException(nameof(sortKey));
            this.File = file ?? throw new ArgumentNullException(nameof(file));
            this.LineIndex = lineIndex >= 0 ? lineIndex : throw new ArgumentException("Line index must be non-negative.");
        }

        public override string ToString() =>
            $"{this.SortKey} in {this.File.Name} reminder on line {this.LineIndex}";
    }
}
