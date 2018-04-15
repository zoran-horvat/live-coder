using System;
using VSExtension.Interfaces;

namespace VSExtension.Implementation.Commands
{
    class VerifyActiveDocument : VerifyStep
    {
        private ISource File;

        public VerifyActiveDocument(ISource file)
        {
            this.File = file ?? throw new ArgumentNullException(nameof(file));
        }

        public override bool IsStateAsExpected =>
            this.File.IsActive;

        public override string ToString() =>
            $"verify document {this.File.Name} is active";
    }
}