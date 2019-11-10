using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveCoder.Common
{
    public static class StringEnumerableExtensions
    {
        public static string Join(this IEnumerable<string> sequence, string separator) =>
            String.Join(separator, sequence.ToArray());
    }
}
