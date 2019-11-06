using System.Collections.Generic;

namespace LiveCoderExtension.Implementation
{
    abstract class SourceReader
    {
        public abstract IEnumerable<(string line, int index)> ReadAllLines();
    }
}
