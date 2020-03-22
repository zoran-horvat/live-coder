using System.Collections.Generic;

namespace LiveCoder.Snippets.Interfaces
{
    interface IDemoStep
    {
        string SnippetShortcut { get; }
        IEnumerable<IDemoCommand> GetCommands(CodeSnippets script);
        string Label { get; }
    }
}
