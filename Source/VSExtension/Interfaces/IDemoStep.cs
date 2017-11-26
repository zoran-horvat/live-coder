using System.Collections.Generic;

namespace VSExtension.Interfaces
{
    interface IDemoStep
    {
        string SortKey { get; }
        IEnumerable<IDemoCommand> Commands { get; }
    }
}
