using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LiveCoder.Common.Optional;

namespace LiveCoder.Common.Xml
{
    public static class ElementsExtensions
    {
        public static IEnumerable<XElement> RootChildren(this XDocument document) =>
            document.Root?.Elements() ?? new XElement[0];

        public static Option<XElement> Child(this XElement element, string localName) =>
            element.Elements()
                .Where(child => child.Name.LocalName == localName)
                .FirstOrNone();

        public static Option<XElement> Child(this Option<XElement> element, string localName) =>
            element.MapOptional(el => el.Child(localName));

        public static Option<string> Value(this Option<XElement> element) =>
            element.Map(el => el.Value);

        public static Option<string> ValueOf(this XElement element, params string[] localNamesPath) =>
            localNamesPath.Aggregate(Option.Of(element), (acc, localName) => acc.Child(localName)).Value();
    }
}
