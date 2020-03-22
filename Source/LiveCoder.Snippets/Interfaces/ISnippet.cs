using LiveCoder.Common.Optional;

namespace LiveCoder.Snippets.Interfaces
{
    interface ISnippet
    {
        Option<string> Content { get; }
    }
}
