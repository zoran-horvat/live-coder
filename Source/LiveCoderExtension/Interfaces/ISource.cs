using System.Collections.Generic;
using LiveCoderExtension.Functional;

namespace LiveCoderExtension.Interfaces
{
    internal interface ISource
    {
        string Name { get; }
        IEnumerable<IDemoStep> DemoSteps { get; }
        IEnumerable<(string line, int index)> Lines { get; }
        IEnumerable<string> GetTextBetween(int startLineIndex, int endLineIndex);
        Option<string> GetLineContent(int lineIndex);
        bool IsActive { get; }
        Option<int> CursorLineIndex { get; }
        string SelectedText { get; }
        void Open();
        void Activate();
        void MoveSelectionToLine(int lineIndex);
        void SelectLine(int lineIndex);
        void DeleteLine(int lineIndex);
        void SelectLines(int startLineIndex, int endLineIndex);
        void ReplaceSelectionWithSnippet(string shortcut);
    }
}