namespace LiveCoder.Common.Optional
{
    public static class StringExtensions
    {
        public static Option<int> IndexOfOrNone(this string s, string searchFor, int startIndex) =>
            s?.IndexOf(searchFor, startIndex) is int pos && pos >= 0
                ? Option.Of(pos)
                : Option.None<int>();
    }
}
