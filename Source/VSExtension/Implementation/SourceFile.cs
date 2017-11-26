using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using VSExtension.Functional;
using VSExtension.Implementation.Commands;
using VSExtension.Interfaces;

namespace VSExtension.Implementation
{
    class SourceFile : ISource
    {
        public string Name => this.File.Name;
        private SourceReader Reader { get; }
        private VSConstants.VSITEMID ItemId { get; }
        private IVsProject Project { get; }
        private DTE Dte { get; }

        public IEnumerable<IDemoStep> DemoSteps =>
            this.Lines.Aggregate(new RunningDemoSteps(this), (steps, tuple) => steps.Add(tuple.line, tuple.index)).All;

        private FileInfo File { get; }

        public SourceFile(FileInfo file, VSConstants.VSITEMID itemId, IVsProject project, DTE dte, SourceReader reader)
        {
            this.File = file ?? throw new ArgumentNullException(nameof(file));
            this.ItemId = itemId;
            this.Dte = dte ?? throw new ArgumentNullException(nameof(dte));
            this.Project = project ?? throw new ArgumentNullException(nameof(project));
            this.Reader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        private IEnumerable<(string line, int index)> Lines => this.Reader.ReadAllLines();

        public void Open() => this.Project.Open(this.ItemId);

        public void Activate() => this.Dte.Documents.Item(this.File.FullName)?.Activate();

        public void MoveSelectionToLine(int lineIndex) =>
            this.Selection.Do(selection => selection.GotoLine(lineIndex + 1));

        public void SelectLine(int lineIndex)
        {
            this.MoveSelectionToLine(lineIndex);
            this.Selection.Do(selection => selection.SelectLine());
        }

        public void DeleteLine(int lineIndex)
        {
            this.SelectLine(lineIndex);
            this.Selection.Do(selection => selection.Delete());
        }

        public void SelectLines(int startLineIndex, int endLineIndex)
        {
            this.MoveSelectionToLine(startLineIndex);
            this.Selection.Do(selection => selection.MoveToLineAndOffset(endLineIndex + 1, 1, true));
        }

        private Option<TextSelection> Selection =>
            Option.FromNullable((TextSelection)this.Dte.Documents.Item(this.File.FullName)?.Selection);

        public override string ToString() => this.File.FullName;
    }
}
