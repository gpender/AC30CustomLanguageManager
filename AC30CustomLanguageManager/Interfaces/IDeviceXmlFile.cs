using System;
using System.IO;
using System.Xml.Linq;

namespace Parker.AP.Common.CustomLanguages
{
    public interface IDeviceXmlFile
    {
        Version Version { get; set; }
        string ProductType { get; set; }
        string FileLocation { get; set; }
    }
}