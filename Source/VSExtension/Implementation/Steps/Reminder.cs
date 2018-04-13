using System;
using System.Collections.Generic;
using System.Linq;
using VSExtension.Functional;
using VSExtension.Implementation.Commands;
using VSExtension.Interfaces;

namespace VSExtension.Implementation.Steps
{
    class Reminder : IDemoStep
    {
        public string SortKey { get; }
        private ISource File { get; }
        private int LineIndex { get; }

        private string LineContent =>
            this.File.TextBetween(this.LineIndex, this.LineIndex)
                .FirstOrNone()
                .Reduce(string.Empty);

        public Reminder(string sortKey, ISource file, int lineIndex)
        {
            this.SortKey = sortKey ?? throw new ArgumentNullException(nameof(sortKey));
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
                new DeleteLine(this.File, this.LineIndex, this.LineContent)
            };

        public override string ToString() =>
            $"{this.SortKey} in {this.File.Name} reminder on line {this.LineIndex}";
    }
}
