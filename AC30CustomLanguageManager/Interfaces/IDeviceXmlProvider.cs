using System;
using System.IO;
using System.Xml.Linq;

namespace Parker.AP.Common.CustomLanguages
{
    public interface IDeviceXmlProvider
    {
        XNamespace NameSpace { get; set; }
        Stream XmlStream { get; set; }
    }
}