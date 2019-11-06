using LiveCoderExtension.Functional;

namespace LiveCoderExtension.Interfaces
{
    interface ISnippet
    {
        Option<string> Content { get; }
    }
}
