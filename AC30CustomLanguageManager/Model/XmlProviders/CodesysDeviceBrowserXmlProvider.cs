using Parker.AP.Common.CustomLanguages;
using Microsoft.Win32;
using System.IO;
using System.Xml.Linq;

namespace AC30CustomLanguageManager.Model
{
    public class CodesysDeviceBrowserXmlProvider : IDeviceXmlProvider
    {
        Stream xmlStream;
        XNamespace nameSpace = "http://www.3s-software.com/schemas/DeviceDescription-1.0.xsd";
        string defaultFileLocation = @"C:\ProgramData\PDQ\Devices\4099";
        //string fileLocation = @"C:\temp\AC30P.devdesc.xml";
        string fileLocation = @"C:\temp\device.xml";

        public XNamespace NameSpace
        {
            get { return nameSpace; }
            set { nameSpace = value; }
        }

        public Stream XmlStream
        {
            get
            {
                //return new FileStream(fileLocation, FileMode.Open);
                OpenFileDialog openFileDialog = new OpenFileDialog();
               // openFileDialog.InitialDirectory = defaultFileLocation;
                //openFileDialog.Filter = languageStringProjectFileFilter;
                //openFileDialog.DefaultExt = languageStringProjectFileExtension;

                if (openFileDialog.ShowDialog() == true)
                {
                    fileLocation = openFileDialog.FileName;
                }
                return new FileStream(fileLocation, FileMode.Open,FileAccess.Read); }
            set { xmlStream = value; }
        }
    }
}
