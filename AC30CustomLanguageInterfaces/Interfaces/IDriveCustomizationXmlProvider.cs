using System.IO;
using System.Xml.Linq;

namespace AC30CustomLanguageInterfaces.Interfaces
{
    public interface IDriveCustomizationXmlProvider
    {
        XNamespace NameSpace { get; set; }
        Stream XmlStream { get; set; }
    }
}