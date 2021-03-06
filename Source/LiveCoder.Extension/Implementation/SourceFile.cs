﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnvDTE;
using LiveCoder.Api;
using LiveCoder.Common.Optional;
using LiveCoder.Extension.Implementation.Readers;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace LiveCoder.Extension.Implementation
{
    class SourceFile : ISource
    {
        public string Name => this.File.Name;
        private SourceReader Reader { get; }
        public VSConstants.VSITEMID ItemId { get; }
        private IVsProject Project { get; }
        public DTE Dte { get; }

        private FileInfo File { get; }

        public SourceFile(FileInfo file, VSConstants.VSITEMID itemId, IVsProject project, DTE dte)
        {
            this.File = file ?? throw new ArgumentNullException(nameof(file));
            this.ItemId = itemId;
            this.Dte = dte ?? throw new ArgumentNullException(nameof(dte));
            this.Project = project ?? throw new ArgumentNullException(nameof(project));
            this.Reader = this.ReaderFor(dte, file);
        }

        private SourceReader ReaderFor(DTE dte, FileInfo file) =>
            new OpenDocumentReader(dte, file, new FileReader(file));

        public IEnumerable<(string line, int lineIndex)> Lines => this.Reader.ReadAllLines();

        public IEnumerable<string> GetTextBetween(int startLineIndex, int endLineIndex) =>
            this.Lines.Skip(startLineIndex).Take(endLineIndex - startLineIndex + 1).Select(tuple => tuple.line);

        public Option<string> GetLineContent(int lineIndex) =>
            this.GetTextBetween(lineIndex, lineIndex).FirstOrNone();

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

        public void ReplaceSelectionWith(string content) => 
            this.TextSelection.Do(sel => sel.Insert(content));

        private Option<Document> Document => 
            this.Dte.Documents.Item(this.File.FullName).FromNullable();

        public string SelectedText =>
            this.TextSelection.Map(sel => sel.Text).Reduce(string.Empty);

        private Option<TextSelection> TextSelection =>
            this.Document.Map(doc => doc.Selection).OfType<TextSelection>();

        public override string ToString() => this.File.FullName;
    }
}
