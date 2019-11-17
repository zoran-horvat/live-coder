using LiveCoder.Common.Optional;

namespace LiveCoder.Common.Text
{
    public static class IntParsing
    {
        public static Option<int> ParseInt(this string value) =>
            int.TryParse(value, out int number) ? Option.Of(number) : None.Value;

        public static Option<int> ParseInt(this Option<string> value) =>
            value.MapOptional(ParseInt);
    }
}
