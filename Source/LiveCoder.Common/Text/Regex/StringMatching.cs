using System.Text.RegularExpressions;
using LiveCoder.Common.Optional;

namespace LiveCoder.Common.Text.Regex
{
    public static class StringMatching
    {
        public static Option<string> Extract(this string input, string groupName, string usingPattern) =>
            System.Text.RegularExpressions.Regex.Match(input, usingPattern) is Match match &&
            match.Success &&
            match.Groups[groupName] is Group group &&
            group.Success
                ? Option.Of(group.Value)
                : None.Value;

        public static Option<string> Extract(this Option<string> input, string groupName, string usingPattern) =>
            input.MapOptional(value => value.Extract(groupName, usingPattern));
    }
}
