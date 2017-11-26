using System.Collections.Generic;

namespace VSExtension.Implementation
{
    abstract class SourceReader
    {
        public abstract IEnumerable<(string line, int index)> ReadAllLines();
    }
}
