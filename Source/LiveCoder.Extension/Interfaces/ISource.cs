using System.Collections.Generic;
using LiveCoder.Common.Optional;
using LiveCoder.Extension.Scripting;

namespace LiveCoder.Extension.Interfaces
{
    internal interface ISource
    {
        string Name { get; }
        IEnumerable<IDemoStep> GetDemoSteps(DemoScript script);
        IEnumerable<(string line, int lineIndex)> Lines { get; }
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
        void ReplaceSelectionWith(string content);
    }
}