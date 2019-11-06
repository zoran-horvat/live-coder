using LiveCoderExtension.Functional;

namespace LiveCoderExtension.Interfaces
{
    interface IExpansionManager
    {
        Option<ISnippet> FindSnippet(string shortcut);
    }
}
