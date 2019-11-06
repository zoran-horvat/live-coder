using System;
using System.Collections.Generic;
using System.IO;
using EnvDTE;
using LiveCoderExtension.Functional;

namespace LiveCoderExtension.Implementation.Readers
{
    class OpenDocumentReader : SourceReader
    {
        private DTE Dte { get; }
        private FileInfo File { get; }
        private SourceReader Next { get; }

        public OpenDocumentReader(EnvDTE.DTE dte, FileInfo file, SourceReader next)
        {
            this.Dte = dte ?? throw new ArgumentNullException(nameof(dte));
            this.File = file ?? throw new ArgumentNullException(nameof(file));
            this.Next = next ?? throw new ArgumentNullException(nameof(next));
        }

        private IEnumerable<EnvDTE.Document> OpenDocuments
        {
            get
            {
                for (int i = 0; i < this.Dte.Documents.Count; i++)
                    yield return this.Dte.Documents.Item(i + 1);
            }
        }

        private Option<EnvDTE.Document> TargetDocument =>
            this.OpenDocuments.FirstOrNone(doc =>doc.FullName == this.File.FullName).NotSaved();

        public override IEnumerable<(string line, int index)> ReadAllLines() =>
            this.TargetDocument
                .Map(doc => doc.GetLines())
                .Reduce(() => this.Next.ReadAllLines());
    }
}
