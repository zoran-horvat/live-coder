using System.Collections.Generic;

namespace LiveCoder.Scripting.Snippets
{
    public interface IDemoStep
    {
        string SnippetShortcut { get; }
        IEnumerable<IDemoCommand> GetCommands(CodeSnippets script);
        string Label { get; }
    }
}
