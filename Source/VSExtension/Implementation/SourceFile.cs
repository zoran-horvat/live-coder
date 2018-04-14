﻿using System;
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

        public IEnumerable<(string line, int index)> Lines => this.Reader.ReadAllLines();

        public IEnumerable<string> TextBetween(int startLineIndex, int endLineIndex) =>
            this.Lines.Skip(startLineIndex).Take(endLineIndex - startLineIndex + 1).Select(tuple => tuple.line);

        public void Open() => this.Project.Open(this.ItemId);

        public Option<int> CursorLineIndex =>
            this.TextSelection.Map(sel => sel.ActivePoint.Line - 1);

        public bool IsActive =>
            this.ActiveDocument.Map(doc => doc.FullName == this.File.FullName).Reduce(false);

        private Option<EnvDTE.Document> ActiveDocument =>
            this.Dte.ActiveDocument.FromNullable();

        public void Activate() =>
            this.Document.Do(doc => doc.Activate());

        public void MoveSelectionToLine(int lineIndex) =>
            this.TextSelection.Do(selection => selection.GotoLine(lineIndex + 1));

        public void SelectLine(int lineIndex)
        {
            this.MoveSelectionToLine(lineIndex);
            this.TextSelection.Do(selection => selection.SelectLine());
        }

        public void DeleteLine(int lineIndex)
        {
            this.SelectLine(lineIndex);
            this.TextSelection.Do(selection => selection.Delete());
        }

        public void SelectLines(int startLineIndex, int endLineIndex)
        {
            this.MoveSelectionToLine(startLineIndex);
            this.TextSelection.Do(selection => selection.MoveToLineAndOffset(endLineIndex + 1, 1, true));
        }

        private Option<Document> Document => 
            this.Dte.Documents.Item(this.File.FullName).FromNullable();

        private Option<TextSelection> TextSelection =>
            this.Document.Map(doc => doc.Selection).OfType<TextSelection>();

        public override string ToString() => this.File.FullName;
    }
}