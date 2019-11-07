using Common.Optional;

namespace LiveCoderExtension.Interfaces
{
    interface ISnippet
    {
        Option<string> Content { get; }
    }
}
