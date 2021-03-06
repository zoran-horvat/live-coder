﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiveCoder.Common.Optional;

namespace LiveCoder.Common.Text.Documents
{
    public class NonEmptyText : IText
    {
        private string[] Content { get; }
        public int LineIndex { get; }
        private int RemainingLinesCount => this.Content.Length - this.LineIndex;
        public string CurrentLine => this.Content[this.LineIndex];
        public IEnumerable<string> Lines => this.Content.Skip(this.LineIndex);

        public NonEmptyText(string[] content) : this(content, 0) { }

        private NonEmptyText(string[] content, int lineIndex)
        {
            this.Content = content;
            this.LineIndex = lineIndex;
        }

        public IText ConsumeLine() =>
            this.LineIndex < this.Content.Length - 1
                ? (IText)new NonEmptyText(this.Content, this.LineIndex + 1)
                : new EmptyText();

        private IText ConsumeUntil(int lastConsumedIndex) =>
            lastConsumedIndex < this.Content.Length - 1
                ? (IText)new NonEmptyText(this.Content, lastConsumedIndex + 1)
                : new EmptyText();

        public Option<(IEnumerable<string> lines, IText remaining)> ConsumeUntilMatch(System.Text.RegularExpressions.Regex pattern) =>
            this.IndexMatching(pattern)
                .Map(index => (this.CopyUntil(index), this.ConsumeUntil(index)));

        private Option<int> IndexMatching(System.Text.RegularExpressions.Regex pattern) =>
            Enumerable
                .Range(this.LineIndex, this.RemainingLinesCount)
                .FirstOrNone(index => pattern.IsMatch(this.Content[index]));

        private IEnumerable<string> CopyUntil(int lastIndex) =>
            Enumerable
                .Range(this.LineIndex, lastIndex - this.LineIndex + 1)
                .Select(index => this.Content[index]);

        public override string ToString() =>
            string.Join(Environment.NewLine, this.Content, this.LineIndex, this.Content.Length - this.LineIndex);
    }
}