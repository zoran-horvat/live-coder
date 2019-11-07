using System;
using Common.Optional;
using LiveCoderExtension.Functional;
using LiveCoderExtension.Interfaces;

namespace LiveCoderExtension.Implementation.Commands
{
    internal class VerifyLineContent : VerifyStep
    {
        private ISource File { get; }
        private int LineIndex { get; }
        private string ExpectedLineContent { get; }

        public VerifyLineContent(ISource file, int lineIndex, string expectedLineContent)
        {
            this.File = file ?? throw new ArgumentNullException(nameof(file));
            this.LineIndex = lineIndex;
            this.ExpectedLineContent = expectedLineContent ?? throw new ArgumentNullException(nameof(expectedLineContent));
        }

        public override bool IsStateAsExpected =>
            this.ExpectedLineContent == this.ActualLineContent;

        public override string PrintableReport => this.IsStateAsExpected
            ? $"Content of line {this.LineIndex + 1} in {this.File.Name} as expected: {this.PrintableExpectedLineContent}"
            : $"Content of line {this.LineIndex + 1} in {this.File.Name} not as expected\nExpected: {this.PrintableExpectedLineContent}\n  Actual: {this.PrintableActualLineContent}";

        private string ActualLineContent =>
            this.File.GetTextBetween(this.LineIndex, this.LineIndex).FirstOrNone().Reduce(string.Empty);

        private string PrintableExpectedLineContent => this.ExpectedLineContent.WithPrintableNewLines();

        private string PrintableActualLineContent => this.ActualLineContent.WithPrintableNewLines();

        public override string ToString() =>
            $"verify file={this.File.Name} line={this.LineIndex} content={this.ShortenedExpectedLineContent}";

        private string ShortenedExpectedLineContent => this.Shorten(this.PrintableExpectedLineContent, 40);

        private string Shorten(string content, int maxLength) =>
            content.Length > 40
                ? content.Substring(0, 40) + "..."
                : content;
    }
}