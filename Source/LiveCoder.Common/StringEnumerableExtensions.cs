using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveCoder.Common
{
    public static class StringEnumerableExtensions
    {
        public static string Join(this IEnumerable<string> sequence, string separator) =>
            String.Join(separator, sequence.ToArray());

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
