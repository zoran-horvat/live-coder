using VSExtension.Functional;

namespace VSExtension.Interfaces
{
    interface ISnippet
    {
        Option<string> Content { get; }
    }
}
