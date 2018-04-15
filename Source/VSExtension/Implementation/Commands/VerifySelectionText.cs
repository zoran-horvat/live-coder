using System;
using VSExtension.Interfaces;

namespace VSExtension.Implementation.Commands
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
            this.File.SelectedText == this.ExpectedSelectionText;
    }
}