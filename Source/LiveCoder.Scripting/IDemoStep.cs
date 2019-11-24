using System.Collections.Generic;

namespace LiveCoder.Scripting
{
    public interface IDemoStep
    {
        string SnippetShortcut { get; }
        IEnumerable<IDemoCommand> GetCommands(DemoScript script);
        string Label { get; }
    }
}
