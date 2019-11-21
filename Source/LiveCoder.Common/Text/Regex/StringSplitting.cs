using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LiveCoder.Common.Text.Regex
{
    public static class StringSplitting
    {
        public static IEnumerable<string> SplitIncludeSeparators(this string value, System.Text.RegularExpressions.Regex separator)
        {
            int pos = 0;
            while (pos < value.Length && separator.Match(value, pos) is Match match && match.Success)
            {
                if (pos < match.Index)
                    yield return value.Substring(pos, match.Index - pos);
                yield return match.Value;
                pos = match.Index + match.Length;
            }

            if (pos < value.Length)
                yield return value.Substring(pos);
        }
    }
}