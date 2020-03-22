using System.Collections.Generic;

namespace LiveCoder.Scripting.Snippets
{
    interface IDemoStep
    {
        string SnippetShortcut { get; }
        IEnumerable<IDemoCommand> GetCommands(CodeSnippets script);
        string Label { get; }
    }
}
