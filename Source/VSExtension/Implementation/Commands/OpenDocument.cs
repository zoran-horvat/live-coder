using System;
using VSExtension.Interfaces;

namespace VSExtension.Implementation.Commands
{
    class OpenDocument : IDemoCommand
    {
        private ISource Document { get; }

        public OpenDocument(ISource document)
        {
            this.Document = document ?? throw new ArgumentNullException(nameof(document));
        }

        public bool CanExecute => true;

        public void Execute() => this.Document.Open();

        public override string ToString() => $"open {this.Document}";
    }
}