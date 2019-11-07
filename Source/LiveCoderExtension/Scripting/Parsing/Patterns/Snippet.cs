using System.Text.RegularExpressions;
using Common.Optional;

namespace LiveCoderExtension.Scripting.Parsing.Patterns
{
    class Snippet : IPattern
    {
        public Regex StartsWith => new Regex("^snippet ");

        public (Option<Text> remaining, Option<DemoScript> script) Apply(Text current, DemoScript script)
        {
            return (current, None.Value);
        }
    }
}
