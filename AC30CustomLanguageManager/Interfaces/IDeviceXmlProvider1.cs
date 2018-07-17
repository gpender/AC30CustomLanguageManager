using System.Collections.Generic;

namespace Parker.AP.Common.CustomLanguages
{
    public interface IDeviceXmlProvider1 : IDeviceXmlProvider
    {
        List<IDeviceXmlFile> AvailableDeviceXmlFiles { get; }
    }
}