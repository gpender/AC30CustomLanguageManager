using System.Collections.Generic;

namespace Parker.AP.Common.CustomLanguages
{
    public interface ICustomLanguageManager1 : ICustomLanguageManager
    {
        List<IDeviceXmlFile> AvailableDeviceXmlFiles { get; }
        void ImportDeviceXmlFile(string fileLocation);
    }
}