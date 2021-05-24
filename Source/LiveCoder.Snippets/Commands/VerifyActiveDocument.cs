using System;
using LiveCoder.Api;
using LiveCoder.Snippets.Interfaces;

namespace LiveCoder.Snippets.Commands
{
    class VerifyActiveDocument : VerifyStep
    {
        private ISource File;

        private VerifyActiveDocument(ISource file)
        {
            this.File = file ?? throw new ArgumentNullException(nameof(file));
        }

        public static IDemoCommand WhenNotDebug(ISource file)
        {
#if DEBUG
            return new DoNothing();
#else
            return new VerifyActiveDocument(file);
#endif
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