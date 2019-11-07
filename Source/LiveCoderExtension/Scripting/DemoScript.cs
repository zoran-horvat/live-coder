using System.IO;
using Common.Optional;
using LiveCoderExtension.Interfaces;
using LiveCoderExtension.Scripting.Parsing;

namespace LiveCoderExtension.Scripting
{
    class DemoScript
    {
        public static Option<DemoScript> TryParse(FileInfo file, ILogger logger) =>
            Text.TryLoad(file).MapOptional(new ScriptTextParser(logger).TryParse);
    }
}
