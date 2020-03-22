using System;
using LiveCoder.Api;

namespace LiveCoder.Snippets.Commands
{
    class VerifyCursorPosition : VerifyStep
    {
        private ISource File { get; }
        private int ExpectedLineIndex { get; }

        public VerifyCursorPosition(ISource file, int expectedLineIndex)
        {
            this.File = file ?? throw new ArgumentNullException(nameof(file));
            this.ExpectedLineIndex = expectedLineIndex;
        }

        public override bool IsStateAsExpected =>
            this.File.CursorLineIndex.Map(index => index == this.ExpectedLineIndex).Reduce(false);

        public override string PrintableReport => this.IsStateAsExpected
            ? $"Cursor located in {this.File.Name} at line index {this.ExpectedLineIndex + 1} as expected"
            : this.ErrorPrintableReport;

        private string ErrorPrintableReport =>
            this.File.CursorLineIndex
                .Map(index => $"Cursor located in {this.File.Name} at line {index} when expected at line index {this.ExpectedLineIndex + 1}")
                .Reduce($"Cursor location not found in {this.File.Name} when expecting at line index {this.ExpectedLineIndex + 1}");

        public override string ToString() =>
            $"verify file={this.File.Name} cursor at line {this.ExpectedLineIndex + 1}";
    }
}