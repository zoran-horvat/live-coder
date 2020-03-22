using System;
using LiveCoder.Api;
using LiveCoder.Snippets.Interfaces;

namespace LiveCoder.Snippets.Commands
{
    class OpenDocument : IDemoCommand
    {
        private ISource Document { get; }

        public OpenDocument(ISource document)
        {
            this.Document = document ?? throw new ArgumentNullException(nameof(document));
        }

        public void Execute() => this.Document.Open();

        public override string ToString() => $"open {this.Document}";
    }
}