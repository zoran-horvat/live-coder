using System;
using VSExtension.Functional;
using VSExtension.Interfaces;

namespace VSExtension.Implementation.Commands
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

        private string ActualLineContent =>
            this.File.TextBetween(this.LineIndex, this.LineIndex).FirstOrNone().Reduce(string.Empty);

        public override string ToString() =>
            $"verify file={this.File.Name} line={this.LineIndex} content={this.PrintableExpectedLineContent}";

        private string PrintableExpectedLineContent =>
            this.ExpectedLineContent.Length > 40
                ? this.ExpectedLineContent.Substring(0, 40) + "..."
                : this.ExpectedLineContent;
    }
}