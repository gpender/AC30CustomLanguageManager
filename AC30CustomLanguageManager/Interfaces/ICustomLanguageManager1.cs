using System.Collections.Generic;

namespace Parker.AP.Common.CustomLanguages
{
    public interface ICustomLanguageManager1 : ICustomLanguageManager
    {
        List<IDeviceXmlFile> AvailableDeviceXmlFiles { get; }
        string ProjectFileName { get; }
        void ImportDeviceXmlFile(string fileLocation);
    }
}