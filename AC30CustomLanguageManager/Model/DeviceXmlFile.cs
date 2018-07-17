using Parker.AP.Common.CustomLanguages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace AC30CustomLanguageManager.Model
{
    public class DeviceXmlFile : IDeviceXmlFile
    {
        private Dictionary<string, string> ac30ProductTypes = new Dictionary<string, string>()
        {
            { "1","AC30V" },
            { "2","AC30PD" },
            { "3","AC30EIPS (EthernetIP)" },
            { "4","AC30PNIO (ProfinetIO)" }
        };
        public Version Version { get; set; }
        public string ProductType { get; set; }
        public string FileLocation { get; set; }
        public DeviceXmlFile(string version, string fileLocation)
        {
            Version = Version.Parse(version);
            FileLocation = fileLocation;
            ProductType = ac30ProductTypes[Version.Major.ToString()];
        }

        public override string ToString()
        {
            return ProductType + " : " + Version.ToString();
        }
    }
}
