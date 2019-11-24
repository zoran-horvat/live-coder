﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using LiveCoder.Common.Optional;

namespace LiveCoder.Scripting.Text
{
    public class NonEmptyText : IText
    {
        private string[] Content { get; }
        public int LineIndex { get; }
        private int RemainingLinesCount => this.Content.Length - this.LineIndex;
        public string CurrentLine => this.Content[this.LineIndex];

        public NonEmptyText(string[] content) : this(content, 0) { }

        private NonEmptyText(string[] content, int lineIndex)
        {
            this.Content = content;
            this.LineIndex = lineIndex;
        }

        public static Option<NonEmptyText> Load(FileInfo source) =>
            LoadConcurrently(source) is string[] array && array.Length > 0
                ? Option.Of(new NonEmptyText(array))
                : None.Value;

        private static string[] LoadConcurrently(FileInfo source)
        {
            int repeats = 10;
            int waitMsec = 100;
            for (int repeat = 0; repeat < repeats; repeat++)
            {
                try
                {
                    return File.ReadAllLines(source.FullName, Encoding.UTF8);
                }
                catch 
                {
                    Thread.Sleep(waitMsec);
                }
            }

            return new string[0];
        }

        public IText ConsumeLine() =>
            this.LineIndex < this.Content.Length - 1
                ? (IText)new NonEmptyText(this.Content, this.LineIndex + 1)
                : new EmptyText();

        private IText ConsumeUntil(int lastConsumedIndex) =>
            lastConsumedIndex < this.Content.Length - 1
                ? (IText)new NonEmptyText(this.Content, lastConsumedIndex + 1)
                : new EmptyText();

        public Option<(IEnumerable<string> lines, IText remaining)> ConsumeUntilMatch(Regex pattern) =>
            this.IndexMatching(pattern)
                .Map(index => (this.CopyUntil(index), this.ConsumeUntil(index)));

        private Option<int> IndexMatching(Regex pattern) =>
            Enumerable
                .Range(this.LineIndex, this.RemainingLinesCount)
                .FirstOrNone(index => pattern.IsMatch(this.Content[index]));

        private IEnumerable<string> CopyUntil(int lastIndex) =>
            Enumerable
                .Range(this.LineIndex, lastIndex - this.LineIndex + 1)
                .Select(index => this.Content[index]);
    }
}