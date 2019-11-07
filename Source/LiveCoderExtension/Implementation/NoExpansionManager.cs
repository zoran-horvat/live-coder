using Common.Optional;
using LiveCoderExtension.Interfaces;

namespace LiveCoderExtension.Implementation
{
    class NoExpansionManager : IExpansionManager
    {
        public Option<ISnippet> FindSnippet(string shortcut) => None.Value;
    }
}
