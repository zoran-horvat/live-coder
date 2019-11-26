using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using LiveCoder.Common.Optional;

namespace LiveCoder.Scripting.Lexing.Lexemes
{
    public class StringLiteral : Token
    {
        private static (Regex pattern, string replace)[] Patterns { get; } = new (Regex pattern, string replace)[]
        {
            (new Regex("^\""), string.Empty),
            (new Regex("^(?<content>.*)\"$"), "${content}"),
            (new Regex(@"^(?<content>[^\\]*)\\n"), @"${content}\n"),
            (new Regex(@"^(?<content>[^\\]*)\\(?<character>[""\\])"), @"${content}${character}"),
        };

        public string RawValue { get; }

        private StringLiteral(string inputValue, string unescapedValue) : base(inputValue)
        {
            this.RawValue = unescapedValue;
        }

        public static Token Of(string value) => 
            new StringLiteral(value, Unescape(value));

        private static string Unescape(string value)
        {
            string remaining = value;
            StringBuilder result = new StringBuilder();

            while (remaining.Length > 0)
            {
                (string remove, string append) = ShortestMatch(remaining);
                remaining = remaining.Substring(remove.Length);
                result.Append(append);
            }

            return result.ToString();
        }

        private static (string input, string output) ShortestMatch(string value) =>
            Patterns
                .Select(tuple => (pattern: tuple.pattern, match: tuple.pattern.Match(value), replace: tuple.replace))
                .Where(tuple => tuple.match.Success)
                .WithMinOrNone(tuple => tuple.match.Index + tuple.match.Length)
                .Map(tuple => (input: tuple.match.Value, output: tuple.pattern.Replace(tuple.match.Value, tuple.replace)))
                .Reduce((value, value));
    }
}
