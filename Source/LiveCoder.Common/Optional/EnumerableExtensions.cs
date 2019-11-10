using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveCoder.Common.Optional
{
    public static class EnumerableExtensions
    {
        public static Option<T> FirstOrNone<T>(this IEnumerable<T> sequence) =>
            sequence.Take(1).Select<T, Option<T>>(x => x).DefaultIfEmpty(None.Value).Single();

        public static Option<T> FirstOrNone<T>(this IEnumerable<T> sequence, Func<T, bool> predicate) =>
            sequence.Where(predicate).FirstOrNone();

        public static Option<T> WithMinOrNone<T, TKey>(this IEnumerable<T> sequence, Func<T, TKey> keyExtractor) where TKey : IComparable<TKey> =>
            sequence.Select(x => (key: keyExtractor(x), value: x))
                .Aggregate(
                    Option.None<(TKey key, T value)>(),
                    (optionalWinner, current) => 
                        optionalWinner is Some<(TKey key, T value)> someWinner && 
                        someWinner.Content.key.CompareTo(current.key) <= 0 
                            ? optionalWinner 
                            : Option.Of(current))
                .Map(winner => winner.value);

        public static IEnumerable<TResult> MapOptional<T, TResult>(this IEnumerable<T> sequence, Func<T, Option<TResult>> map) =>
            sequence.Select(map)
                .OfType<Some<TResult>>()
                .Select(x => x.Content);
    }
}
