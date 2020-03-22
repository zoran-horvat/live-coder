using System.Collections.Generic;

namespace LiveCoder.Snippets.Elements
{
    public class SnippetNumberComparer : IEqualityComparer<Snippet>
    {
        public bool Equals(Snippet x, Snippet y) =>
            Equals(x?.Number, y?.Number);

        public int GetHashCode(Snippet obj) =>
            obj?.GetHashCode() ?? 0;
    }
}
