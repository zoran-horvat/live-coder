using LiveCoder.Common.Optional;

namespace LiveCoder.Scripting.Interfaces
{
    interface ISnippet
    {
        Option<string> Content { get; }
    }
}
