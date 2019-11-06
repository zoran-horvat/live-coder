using System;
using LiveCoderExtension.Interfaces;

namespace LiveCoderExtension.Implementation.Commands
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

        public override string PrintableReport => this.IsStateAsExpected
            ? $"{this.File.Name} is active as expected"
            : $"{this.File.Name} is not active when expected to be active";

        public override string ToString() =>
            $"verify document {this.File.Name} is active";
    }
}