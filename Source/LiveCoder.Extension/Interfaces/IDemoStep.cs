using System.Collections.Generic;
using LiveCoder.Scripting;

namespace LiveCoder.Extension.Interfaces
{
    interface IDemoStep
    {
        string SnippetShortcut { get; }
        IEnumerable<IDemoCommand> GetCommands(DemoScript script);
        string Label { get; }
    }
}
