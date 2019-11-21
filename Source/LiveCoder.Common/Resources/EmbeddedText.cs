using System.IO;
using System.Reflection;
using LiveCoder.Common.Optional;

namespace LiveCoder.Common.Resources
{
    public static class EmbeddedText
    {
        public static Option<string> TryReadEmbeddedResourceText(this Assembly assembly, string resourceName) =>
            Disposable.Using(() => TryGetResourceStream(assembly, resourceName))
                .TryMap(ReadText);

        private static Option<Stream> TryGetResourceStream(this Assembly assembly, string resourceName) =>
            assembly.GetManifestResourceNames()
                .FirstOrNone(resource => resource == resourceName)
                .Map(assembly.GetManifestResourceStream);

        private static string ReadText(Stream stream) =>
            Disposable.Using(() => new StreamReader(stream)).Map(ReadText);

        private static string ReadText(TextReader reader) =>
            reader.ReadToEnd();

    }
}
