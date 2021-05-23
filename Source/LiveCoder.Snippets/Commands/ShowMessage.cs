using System.Diagnostics;
using LiveCoder.Api;
using LiveCoder.Snippets.Events;
using LiveCoder.Snippets.Interfaces;

namespace LiveCoder.Snippets.Commands
{
    class ShowMessage : IDemoCommand
    {
        private string Text { get; }
        private ILogger Logger { get; }

        public ShowMessage(ILogger logger, string text)
        {
            this.Text = text;
            this.Logger = logger;
        }

        public void Execute()
        {
            if (!string.IsNullOrWhiteSpace(this.Text))
                this.Logger.Write(new SnippetText(this.Text));
        }
    }
}
