using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using VSExtension.Functional;

namespace VSExtension.Implementation.Readers
{
    static class DocumentExtensions
    {
        public static Option<EnvDTE.Document> NotSaved(this Option<EnvDTE.Document> document) =>
            document.When(doc => !doc.Saved);

        public static IEnumerable<(string line, int index)> GetLines(this EnvDTE.Document document)
        {
            TextDocument textDoc = (TextDocument) document.Object("TextDocument");
            if (textDoc == null)
                return Enumerable.Empty<(string line, int index)>();
            return textDoc.GetLines();
        }

        private static IEnumerable<(string line, int index)> GetLines(this TextDocument document) =>
            document.CreateEditPoint()
                .GetText(document.EndPoint)
                .Split(new[] {Environment.NewLine}, StringSplitOptions.None)
                .Select((line, index) => (line, index));
    }
}
