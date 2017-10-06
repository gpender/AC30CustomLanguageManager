using System;
using System.Collections.Generic;

namespace Parker.AP.Common.CustomLanguages
{
    public interface IDeviceStringReader
    {
        Version DeviceXmlVersion { get; }

        IEnumerable<ILanguageStringCollection> GetStringsFromDeviceXml(IDeviceXmlProvider deviceXmlProvider);
    }
}