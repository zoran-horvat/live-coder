using System.Collections.Generic;

namespace LiveCoder.Extension.Interfaces
{
    interface IDemoStep
    {
        string SnippetShortcut { get; }
        IEnumerable<IDemoCommand> Commands { get; }
        string Label { get; }
    }
}
