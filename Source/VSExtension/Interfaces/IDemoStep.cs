using System.Collections.Generic;

namespace VSExtension.Interfaces
{
    interface IDemoStep
    {
        string SnippetShortcut { get; }
        IEnumerable<IDemoCommand> Commands { get; }
    }
}
