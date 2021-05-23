using System;
using System.Collections.Generic;
using LiveCoder.Api;
using LiveCoder.Common;
using LiveCoder.Snippets.Commands;
using LiveCoder.Snippets.Elements;
using LiveCoder.Snippets.Interfaces;

namespace LiveCoder.Snippets.Steps
{
    class MultilineSnippetReplace : IDemoStep
    {
        private ILogger Logger { get; }
        private Snippet Snippet { get; }
        private string SnippetContent => this.Snippet.Content;
        public string SnippetShortcut => $"snp{this.Snippet.Number:0000}";
        private ISource File { get; }
        private int StartLineIndex { get; }
        private int LinesCount { get; }
        private string Text { get; }

        public MultilineSnippetReplace(ILogger logger, Snippet snippet, ISource file, int startLineIndex, int linesCount, string text)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.Snippet = snippet ?? throw new ArgumentNullException(nameof(snippet));
            this.File = file ?? throw new ArgumentNullException(nameof(file));
            this.StartLineIndex = startLineIndex >= 0 ? startLineIndex : throw new ArgumentException("Start line index must be non-negative.");
            this.LinesCount = linesCount > 0 ? linesCount : throw new ArgumentException("Number of lines must be positive.");
            this.Text = text;
        }

        public IEnumerable<IDemoCommand> GetCommands(CodeSnippets script) =>
            new IDemoCommand[]
            {
                new OpenDocument(this.File),
                new ActivateDocument(this.File),
                new MoveToLine(this.File, this.StartLineIndex),
                new ShowMessage(this.Logger, this.Text),
                new Pause(),
                new SelectMultipleLines(this.File, this.StartLineIndex, this.StartLineIndex + this.LinesCount),
                new Pause(),
                new VerifyActiveDocument(this.File), 
                new VerifySelectionText(this.File, this.SelectedText), 
                new ExpandSelection(this.File, this.SnippetContent)
            };

        public string Label => $"Expand snippet {this.SnippetShortcut} in {this.File.Name} on lines {this.StartLineIndex + 1}-{this.EndLineIndex + 1}";

        private int EndLineIndex => this.StartLineIndex + this.LinesCount - 1;

        private string SelectedText =>
            this.File.GetTextBetween(this.StartLineIndex, this.EndLineIndex).Join(Environment.NewLine) + Environment.NewLine;

        public override string ToString() => this.Label;
    }
}
