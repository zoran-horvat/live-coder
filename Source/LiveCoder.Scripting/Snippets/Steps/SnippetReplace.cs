using System;
using System.Collections.Generic;
using LiveCoder.Scripting.Elements;
using LiveCoder.Scripting.Interfaces;
using LiveCoder.Scripting.Snippets.Commands;

namespace LiveCoder.Scripting.Snippets.Steps
{
    class SnippetReplace : IDemoStep
    {
        private Snippet Snippet { get; }
        public string SnippetShortcut => $"snp{this.Snippet.Number:00}";
        private string SnippetContent => this.Snippet.Content;
        private ISource File { get; }
        private int LineIndex { get; }

        public SnippetReplace(Snippet snippet, ISource file, int lineIndex)
        {
            this.Snippet = snippet ?? throw new ArgumentNullException(nameof(snippet));
            this.File = file ?? throw new ArgumentNullException(nameof(file));
            this.LineIndex = lineIndex >= 0 ? lineIndex : throw new ArgumentException("Line index must be non-negative.");
        }

        public MultilineSnippetReplace EndsOnLine(int index) =>
            new MultilineSnippetReplace(this.Snippet, this.File, this.LineIndex, index - this.LineIndex + 1);

        public IEnumerable<IDemoCommand> GetCommands(CodeSnippets script) =>
            new IDemoCommand[]
            {
                new OpenDocument(this.File),
                new ActivateDocument(this.File),
                new MoveToLine(this.File, this.LineIndex),
                new Pause(),
                new VerifyActiveDocument(this.File),
                new VerifyCursorPosition(this.File, this.LineIndex),
                new SelectLine(this.File, this.LineIndex),
                new Pause(),
                new VerifyActiveDocument(this.File),
                new VerifySelectionText(this.File, this.TextToSelect),
                new ExpandSelection(this.File, this.SnippetContent)
            };

        public string Label => $"Single-line snippet replacement {this.SnippetShortcut} in {this.File.Name} on line {this.LineIndex}";

        private string TextToSelect =>
            this.File.GetLineContent(this.LineIndex).Map(line => line + Environment.NewLine).Reduce(string.Empty);

        public override string ToString() => this.Label;
    }
}
