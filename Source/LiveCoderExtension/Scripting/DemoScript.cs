using System.IO;
using Common.Optional;

namespace LiveCoderExtension.Scripting
{
    class DemoScript
    {
        public static Option<DemoScript> TryParse(FileInfo file) => 
            new ScriptTextParser().Parse(Text.Load(file));
    }
}
