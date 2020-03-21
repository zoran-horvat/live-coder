using System;
using LiveCoder.Common;
using LiveCoder.Scripting.Interfaces;

namespace LiveCoder.Scripting.Snippets.Commands
{
    internal class VerifySelectionText : VerifyStep
    {
        private ISource File { get; }
        private string ExpectedSelectionText { get; }

        public VerifySelectionText(ISource file, string expectedSelectionText)
        {
            this.File = file ?? throw new ArgumentNullException(nameof(file));
            this.ExpectedSelectionText = expectedSelectionText ?? throw new ArgumentNullException(nameof(expectedSelectionText));
        }

        public override bool IsStateAsExpected =>
            this.NormalizedSelectedText == this.NormalizedExpectedText;

        private string NormalizedSelectedText => this.SelectedText.WithNormalizedNewLines();

        private string NormalizedExpectedText => this.ExpectedSelectionText.WithNormalizedNewLines();

        public override string PrintableReport => this.IsStateAsExpected
            ? $"Selected text in {this.File.Name} as expected: {this.PrintableExpectedSelectedText}"
            : $"Selected text in {this.File.Name} not as expected\nExpected: {this.PrintableExpectedSelectedText}\n  Actual: {this.PrintableActualSelectedText}";

        private string PrintableExpectedSelectedText => this.NormalizedExpectedText.WithPrintableNewLines();

        private string PrintableActualSelectedText => this.NormalizedSelectedText.WithPrintableNewLines();

        private string SelectedText => this.File.SelectedText;
    }
}