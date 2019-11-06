using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveCoderExtension.Functional
{
    static class EnumerableExtensions
    {
        public static Option<T> FirstOrNone<T>(this IEnumerable<T> sequence) =>
            sequence.Take(1).Select<T, Option<T>>(x => x).DefaultIfEmpty(None.Value).Single();

        public static Option<T> FirstOrNone<T>(this IEnumerable<T> sequence, Func<T, bool> predicate) =>
            sequence.Where(predicate).FirstOrNone();

        public static T WithMinimum<T, TKey>(this IEnumerable<T> sequence, Func<T, TKey> keySelector) where TKey : IComparable<TKey> =>
            sequence
                .Select<T, (TKey key, T value)>(x => (keySelector(x), x))
                .Aggregate((min, cur) => cur.key.CompareTo(min.key) < 0 ? cur : min)
                .value;

        public static string Join(this IEnumerable<string> sequence, string separator) =>
            string.Join(separator, sequence.ToArray());
    }
}
