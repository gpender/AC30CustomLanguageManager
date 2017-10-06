using System;
using System.IO;
using System.Xml.Linq;

namespace AC30CustomLanguageInterfaces.Interfaces
{
    public interface IDeviceXmlProvider
    {
        XNamespace NameSpace { get; set; }
        Stream XmlStream { get; set; }
    }
}