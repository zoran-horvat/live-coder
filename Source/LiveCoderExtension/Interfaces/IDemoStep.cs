using System.Collections.Generic;

namespace LiveCoderExtension.Interfaces
{
    interface IDemoStep
    {
        string SnippetShortcut { get; }
        IEnumerable<IDemoCommand> Commands { get; }
        string Label { get; }
    }
}
