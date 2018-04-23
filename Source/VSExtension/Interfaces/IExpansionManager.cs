using VSExtension.Functional;

namespace VSExtension.Interfaces
{
    interface IExpansionManager
    {
        Option<ISnippet> FindSnippet(string shortcut);
    }
}
