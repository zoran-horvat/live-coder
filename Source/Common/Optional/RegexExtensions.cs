using System.Text.RegularExpressions;

namespace Common.Optional
{
    public static class RegexExtensions
    {
        public static Option<GroupCollection> TryExtract(this Regex expression, string input) =>
            expression.Match(input) is Match match &&
            match.Success &&
            match.Groups is GroupCollection groups
                ? Option.Of(groups)
                : None.Value;
    }
}
