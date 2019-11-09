using LiveCoder.Common.Optional;

namespace LiveCoder.Extension.Interfaces
{
    interface ISnippet
    {
        Option<string> Content { get; }
    }
}
