using System.Collections.Generic;
using LiveCoder.Extension.Scripting;

namespace LiveCoder.Extension.Interfaces
{
    interface IDemoStep
    {
        string SnippetShortcut { get; }
        IEnumerable<IDemoCommand> GetCommands(DemoScript script);
        string Label { get; }
    }
}
