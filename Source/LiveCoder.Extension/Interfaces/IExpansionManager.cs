using LiveCoder.Common.Optional;

namespace LiveCoder.Extension.Interfaces
{
    interface IExpansionManager
    {
        Option<ISnippet> FindSnippet(string shortcut);
    }
}
