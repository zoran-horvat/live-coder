using System.IO;
using System.Xml.Linq;

namespace LiveCoder.Common.Xml
{
    public static class XmlFileLoad
    {
        public static XDocument LoadXml(this FileInfo file) =>
            Disposable.Using(() => File.OpenRead(file.FullName))
                .Map(XDocument.Load);
    }
}
