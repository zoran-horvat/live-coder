﻿using System;
using System.Collections.Generic;
using VSExtension.Implementation.Commands;
using VSExtension.Interfaces;

namespace VSExtension.Implementation.Steps
{
    class SnippetReplace : IDemoStep
    {
        public string SortKey { get; }
        private ISource File { get; }
        private int LineIndex { get; }

        public SnippetReplace(string sortKey, ISource file, int lineIndex)
        {
            this.SortKey = sortKey ?? throw new ArgumentNullException(nameof(sortKey));
            this.File = file ?? throw new ArgumentNullException(nameof(file));
            this.LineIndex = lineIndex >= 0 ? lineIndex : throw new ArgumentException("Line index must be non-negative.");
        }

        public MultilineSnippetReplace EndsOnLine(int index) =>
            new MultilineSnippetReplace(this.SortKey, this.File, this.LineIndex, index - this.LineIndex + 1);

        public IEnumerable<IDemoCommand> Commands =>
            new IDemoCommand[]
            {
                new OpenDocument(this.File),
                new ActivateDocument(this.File),
                new MoveToLine(this.File, this.LineIndex),
                new Pause(),
                new SelectLine(this.File, this.LineIndex)
            };

        public override string ToString() =>
            $"{this.SortKey} in {this.File.Name} line {this.LineIndex}";
    }
}
