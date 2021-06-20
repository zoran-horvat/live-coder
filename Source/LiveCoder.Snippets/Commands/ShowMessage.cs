using System.Diagnostics;
using LiveCoder.Api;
using LiveCoder.Snippets.Elements;
using LiveCoder.Snippets.Events;
using LiveCoder.Snippets.Interfaces;

namespace LiveCoder.Snippets.Commands
{
    class ShowMessage : IDemoCommand
    {
        private string Text { get; }
        private ILogger Logger { get; }
        private int TotalSnippetsCount { get; }
        private int CurrentSnippetOrdinal { get; }

        public ShowMessage(ILogger logger, int currentSnippet, int totalSnippets, string text)
        {
            this.Text = text;
            this.Logger = logger;
            this.TotalSnippetsCount = totalSnippets;
            this.CurrentSnippetOrdinal = currentSnippet;
        }

        public void Execute()
        {
            if (!string.IsNullOrWhiteSpace(this.Text))
                this.Logger.Write(new SnippetText(this.Text, this.CurrentSnippetOrdinal, this.TotalSnippetsCount));
        }
    }
}
