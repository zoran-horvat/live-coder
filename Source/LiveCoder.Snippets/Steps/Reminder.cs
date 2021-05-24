using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LiveCoder.Api;
using LiveCoder.Common.Optional;
using LiveCoder.Snippets.Commands;
using LiveCoder.Snippets.Interfaces;

namespace LiveCoder.Snippets.Steps
{
    class Reminder : IDemoStep
    {
        private ILogger Logger { get; }
        private StepSourceEntry Step { get; }
        public string SnippetShortcut => this.Step.SnippetShortcut;

        public float Ordinal =>
            float.Parse(Regex.Match(this.SnippetShortcut, @"\d+.\d+").Value);

        private ISource File { get; }
        private int LineIndex => this.Step.LineIndex;
        private string Text => this.Step.Description;
        private bool HasCode => !string.IsNullOrWhiteSpace(this.Step.Code);

        private string LineContent =>
            this.File.GetTextBetween(this.LineIndex, this.LineIndex)
                .FirstOrNone()
                .Reduce(string.Empty);

        public Reminder(ILogger logger, ISource file, StepSourceEntry step)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.File = file ?? throw new ArgumentNullException(nameof(file));
            this.Step = step;
        }

        public IEnumerable<IDemoCommand> GetCommands(CodeSnippets script) =>
            new IDemoCommand[]
            {
                new OpenDocument(this.File),
                new ActivateDocument(this.File),
                new MoveToLine(this.File, this.LineIndex),
                new ShowMessage(this.Logger, this.Text),
                this.SelectLineCommand,
                new Pause(),
                VerifyActiveDocument.WhenNotDebug(this.File),
                new VerifyCursorPosition(this.File, this.HasCode ? this.LineIndex : this.LineIndex + 1),
                new VerifyLineContent(this.File, this.LineIndex, this.LineContent),
                this.ReplacementCommand
            };

        private IDemoCommand SelectLineCommand =>
            this.HasCode ? new SelectLine(this.File, this.LineIndex)
            : (IDemoCommand)new DoNothing();

        private IDemoCommand ReplacementCommand =>
            this.HasCode ? new ExpandSelection(this.File, this.Step.Code + Environment.NewLine)
            : (IDemoCommand)new DeleteLine(this.File, this.LineIndex);

        public string Label => $"Reminder {this.SnippetShortcut} in {this.File.Name} on line {this.LineIndex}";

        public override string ToString() => this.Label;
    }
}
