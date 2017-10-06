using AC30CustomLanguageInterfaces.Interfaces;
using System.IO;
using System.Xml.Linq;

namespace AC30CustomLanguageManagerApp.Model
{
    public class TestDeviceXmlProvider : IDeviceXmlProvider
    {
        Stream xmlStream;
        XNamespace nameSpace = "";

        public XNamespace NameSpace
        {
            get { return nameSpace; }
            set { nameSpace = value; }
        }

        public Stream XmlStream
        {
            get
            {
                string tmp = 
                    "<DeviceDescription>" +
                        "<Strings>" +
                            "<Language lang=\"en\">" +
                                "<String identifier=\"id1\">Test String1</String>" +
                                "<String identifier=\"id2\">Test String2</String>" +
                                "<String identifier=\"id3\">Test String3</String>" +
                                "<String identifier=\"id4\">Test String4</String>" +
                                "<String identifier=\"id5\">Test String5</String>" +
                                "<String identifier=\"id6\">Test String6</String>" +
                            "</Language>" +
                            "<Language lang=\"fr\">" +
                                "<String identifier=\"id1\">Test String1</String>" +
                                "<String identifier=\"id2\">Test String2</String>" +
                                "<String identifier=\"id3\">Test String3</String>" +
                                "<String identifier=\"id4\">Test Guy4</String>" +
                                "<String identifier=\"id5\">Test String5</String>" +
                                "<String identifier=\"id6\">Test String6</String>" +
                            "</Language>" +
                        "</Strings>" +
                        "<Device showParamsInDevDescOrder = \"true\">" +
                            "<DeviceIdentification>" +
                                "<Type>4099</Type>" +
                                "<Id>1629 0001</Id>" +
                                "<Version>1.14.1.1</Version>" +
                            "</DeviceIdentification>" +
                        "</Device>" +
                    "</DeviceDescription>";
                return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(tmp));
            }
            set { xmlStream = value; }
        }
    }
}
