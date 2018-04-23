using VSExtension.Functional;
using VSExtension.Interfaces;

namespace VSExtension.Implementation
{
    class NoExpansionManager : IExpansionManager
    {
        public Option<ISnippet> FindSnippet(string shortcut) => None.Value;
    }
}
