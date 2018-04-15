using System;
using VSExtension.Interfaces;

namespace VSExtension.Implementation.Commands
{
    internal class VerifyCursorPosition : VerifyStep
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

        public override string ToString() =>
            $"verify file={this.File.Name} cursor at line {this.ExpectedLineIndex + 1}";
    }
}