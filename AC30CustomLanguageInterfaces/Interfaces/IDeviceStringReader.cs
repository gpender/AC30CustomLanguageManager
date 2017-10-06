using System;
using System.Collections.Generic;

namespace AC30CustomLanguageInterfaces.Interfaces
{
    public interface IDeviceStringReader
    {
        Version DeviceXmlVersion { get; }

        IEnumerable<ILanguageStringCollection> GetStringsFromDeviceXml(IDeviceXmlProvider deviceXmlProvider);
    }
}