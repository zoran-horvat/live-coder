using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveCoder.Common
{
    public static class StringEnumerableExtensions
    {
        public static string Join(this IEnumerable<string> sequence) =>
            sequence.Join(string.Empty);

        public static string Join(this IEnumerable<string> sequence, string separator) =>
            sequence.Aggregate(
                    (result: new StringBuilder(), prefix: string.Empty),
                    (acc, segment) => (acc.result.Append(acc.prefix).Append(segment), separator))
                .result.ToString();

        public static IEnumerable<string> AppendToLast(this IEnumerable<string> sequence, string suffix) =>
            Disposable.Using(sequence.GetEnumerator).Map(enumerator => AppendToLast(enumerator, suffix));

        private static IEnumerable<string> AppendToLast(IEnumerator<string> enumerator, string suffix)
        {
            if (!enumerator.MoveNext()) yield break;
            string pending = enumerator.Current;

            while (enumerator.MoveNext())
            {
                yield return pending;
                pending = enumerator.Current;
            }

            yield return pending + suffix;
        }
    }
}
