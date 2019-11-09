using LiveCoder.Common.Optional;
using LiveCoder.Extension.Interfaces;

namespace LiveCoder.Extension.Implementation
{
    class NoExpansionManager : IExpansionManager
    {
        public Option<ISnippet> FindSnippet(string shortcut) => None.Value;
    }
}
