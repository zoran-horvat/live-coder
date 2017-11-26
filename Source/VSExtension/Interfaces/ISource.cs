using System.Collections.Generic;

namespace VSExtension.Interfaces
{
    internal interface ISource
    {
        string Name { get; }
        IEnumerable<IDemoStep> DemoSteps { get; }
        void Open();
        void Activate();
        void MoveSelectionToLine(int lineIndex);
    }
}