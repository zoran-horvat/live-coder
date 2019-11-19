using System.IO;
using System.Xml.Linq;
using LiveCoder.Common.IO;
using LiveCoder.Common.Optional;

namespace LiveCoder.Common.Xml
{
    public static class XmlFileLoad
    {
        public static XDocument LoadXml(this FileInfo file) =>
            Disposable.Using(file.OpenRead).Map(XDocument.Load);

        public static Option<XDocument> TryLoadConcurrent(this FileInfo file) =>
            Disposable.Using(file.TryOpenReadConcurrent).TryMap(XDocument.Load);
    }
}
