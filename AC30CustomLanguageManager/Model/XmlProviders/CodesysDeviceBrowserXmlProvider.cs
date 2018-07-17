using Parker.AP.Common.CustomLanguages;
using Microsoft.Win32;
using System.IO;
using System.Xml.Linq;
using System;
using AC30CustomLanguageManager.Views;
using System.Collections.Generic;

namespace AC30CustomLanguageManager.Model
{
    public class CodesysDeviceBrowserXmlProvider : IDeviceXmlProvider1
    {
        Stream xmlStream;
        XNamespace nameSpace = "http://www.3s-software.com/schemas/DeviceDescription-1.0.xsd";
        Dictionary<string, string> tmpAc30DeviceXmlFiles = new Dictionary<string, string>();
        string fileLocation = @"c:\temp\devce.xml";
        List<IDeviceXmlFile> availableDeviceXmlFiles = new List<IDeviceXmlFile>();

        List<string> xmlFileLocations = new List<string>()
        {
            @"C:\ProgramData\PDQ\Devices\4099",
            @"C:\ProgramData\Codesys\Devices\4099",
            @"C:\ProgramData\ParkerAutomationManager\Devices\4099"
        };

        public List<IDeviceXmlFile> AvailableDeviceXmlFiles
        {
            get
            {
                if(availableDeviceXmlFiles.Count == 0)
                {
                    GetInstalledDeviceXmlFiles();
                }
                return availableDeviceXmlFiles;
            }
        }
        public XNamespace NameSpace
        {
            get { return nameSpace; }
            set { nameSpace = value; }
        }



        public Stream XmlStream
        {
            get
            {
                return new FileStream(fileLocation, FileMode.Open, FileAccess.Read);
                //return new FileStream(fileLocation, FileMode.Open);
                OpenFileDialog openFileDialog = new OpenFileDialog();
                // openFileDialog.InitialDirectory = defaultFileLocation;
                //openFileDialog.Filter = languageStringProjectFileFilter;
                //openFileDialog.DefaultExt = languageStringProjectFileExtension;

                if (openFileDialog.ShowDialog() == true)
                {
                    fileLocation = openFileDialog.FileName;
                }
                return new FileStream(fileLocation, FileMode.Open, FileAccess.Read); }
            set { xmlStream = value; }
        }

        private void GetInstalledDeviceXmlFiles()
        {
            foreach (var f in xmlFileLocations)
            {
                if (Directory.Exists(f))
                {
                    GetAC30Devices(f);
                }
            }
            foreach(var s in tmpAc30DeviceXmlFiles)
            {
                IDeviceXmlFile device = null;
                try
                {
                    device = new DeviceXmlFile(s.Key, s.Value);
                }
                catch { }
                if (device != null)
                {
                    availableDeviceXmlFiles.Add(device);
                }
            }
        }

        private void GetAC30Devices(string folder)
        {
            foreach (var f in Directory.EnumerateDirectories(folder))
            {
                foreach (var sf in Directory.EnumerateDirectories(f))
                {
                    DirectoryInfo di = new DirectoryInfo(sf);
                    string deviceLocation = di.FullName + @"\device.xml";
                    if(File.Exists(deviceLocation))
                    {
                        if (!tmpAc30DeviceXmlFiles.ContainsKey(di.Name))
                        {
                            tmpAc30DeviceXmlFiles.Add(di.Name, deviceLocation);
                        }
                    }
                }
            }
        }
    }
}
