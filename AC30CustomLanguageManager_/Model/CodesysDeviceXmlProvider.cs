using AC30CustomLanguageInterfaces.Interfaces;
using Microsoft.Win32;
using System.IO;
using System.Xml.Linq;

namespace AC30CustomLanguageManagerApp.Model
{
    public class CodesysDeviceXmlProvider : IDeviceXmlProvider
    {
        Stream xmlStream;
        XNamespace nameSpace = "http://www.3s-software.com/schemas/DeviceDescription-1.0.xsd";
        //string fileLocation = @"C:\ProgramData\PDQ\Devices\4099\1629 0003\3.13.1.1\device.xml";
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
               // return new FileStream(fileLocation, FileMode.Open);
                OpenFileDialog openFileDialog = new OpenFileDialog();
                //openFileDialog.Filter = languageStringProjectFileFilter;
                //openFileDialog.DefaultExt = languageStringProjectFileExtension;

                if (openFileDialog.ShowDialog() == true)
                {
                    fileLocation = openFileDialog.FileName;
                }
                return new FileStream(fileLocation, FileMode.Open); }
            set { xmlStream = value; }
        }
    }
}
