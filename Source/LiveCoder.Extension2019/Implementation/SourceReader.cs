using System.Collections.Generic;

namespace LiveCoder.Extension.Implementation
{
    abstract class SourceReader
    {
        public abstract IEnumerable<(string line, int lineIndex)> ReadAllLines();
    }
}
