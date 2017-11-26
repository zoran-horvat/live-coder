using System;
using System.Collections.Generic;
using System.Linq;

namespace VSExtension.Functional
{
    static class EnumerableExtensions
    {
        public static Option<T> FirstOrNone<T>(this IEnumerable<T> sequence) =>
            sequence.Take(1).Select<T, Option<T>>(x => x).DefaultIfEmpty(None.Value).Single();

        public static Option<T> FirstOrNone<T>(this IEnumerable<T> sequence, Func<T, bool> predicate) =>
            sequence.Where(predicate).FirstOrNone();
    }
}
