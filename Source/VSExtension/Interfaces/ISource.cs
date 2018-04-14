using System.Collections.Generic;
using VSExtension.Functional;

namespace VSExtension.Interfaces
{
    internal interface ISource
    {
        string Name { get; }
        IEnumerable<IDemoStep> DemoSteps { get; }
        IEnumerable<(string line, int index)> Lines { get; }
        IEnumerable<string> TextBetween(int startLineIndex, int endLineIndex);
        bool IsActive { get; }
        Option<int> CursorLineIndex { get; }
        void Open();
        void Activate();
        void MoveSelectionToLine(int lineIndex);
        void SelectLine(int lineIndex);
        void DeleteLine(int lineIndex);
        void SelectLines(int startLineIndex, int endLineIndex);
    }
}