using System.Text.RegularExpressions;
using Common.Optional;

namespace LiveCoderExtension.Scripting.Parsing
{
    interface IPattern
    {
        Regex StartsWith { get; }
        (Option<Text> remaining, Option<DemoScript> script) Apply(Text current, DemoScript script);
    }
}
