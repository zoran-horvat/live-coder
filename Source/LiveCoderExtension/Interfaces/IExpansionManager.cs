using Common.Optional;

namespace LiveCoderExtension.Interfaces
{
    interface IExpansionManager
    {
        Option<ISnippet> FindSnippet(string shortcut);
    }
}
