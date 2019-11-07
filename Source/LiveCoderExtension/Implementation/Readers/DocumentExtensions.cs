using System.Collections.Generic;
using System.Linq;
using Common.Optional;
using EnvDTE;

namespace LiveCoderExtension.Implementation.Readers
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

        private static IEnumerable<(string line, int index)> GetLines(this TextDocument document)
        {
            EditPoint currentLine = document.CreateEditPoint();
            int processedLineIndex;
            do
            {
                string lineText = currentLine.GetText(currentLine.LineLength);
                yield return (lineText, currentLine.Line - 1);
                processedLineIndex = currentLine.Line;
                currentLine.LineDown();
            }
            while (processedLineIndex != currentLine.Line);
        }
    }
}
