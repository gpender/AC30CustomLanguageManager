using System;
using System.Collections.Generic;

namespace Parker.AP.Common.CustomLanguages
{
    public interface IDeviceStringReader1 : IDeviceStringReader
    {
        IEnumerable<ILanguageStringCollection> GetStringsFromDeviceXml(string fileLocation, IDeviceXmlProvider deviceXmlProvider);
    }
}