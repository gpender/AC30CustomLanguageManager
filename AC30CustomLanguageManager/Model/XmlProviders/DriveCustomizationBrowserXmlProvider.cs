using Microsoft.Win32;
using Parker.AP.Common.CustomLanguages;
using System.IO;
using System.Xml.Linq;

namespace AC30CustomLanguageManager.Model
{
    public class DriveCustomizationBrowserXmlProvider : IDriveCustomizationXmlProvider
    {
        Stream xmlStream;
        XNamespace nameSpace = "";
        string fileLocation = @"C:\temp\LanguagesProjectRepository.xml";

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
                //openFileDialog.Filter = languageStringProjectFileFilter;
                //openFileDialog.DefaultExt = languageStringProjectFileExtension;

                if (openFileDialog.ShowDialog() == true)
                {
                    fileLocation = openFileDialog.FileName;
                }
                return new FileStream(fileLocation, FileMode.Open);

                string tmp = "<SoftParameterEnums>" +
                        "<SoftParameterEnum>" +
                            "<Id>1</Id>" +
                            "<CanUserDelete>false</CanUserDelete>" +
                            "<Name>Enum1</Name>" +
                            "<EnumList>One#Two</EnumList>" +
                            "<EnumGuid>Test String5</EnumGuid>" +
                        "</SoftParameterEnum>" +
                        "<SoftParameterEnum>" +
                            "<Id>2</Id>" +
                            "<CanUserDelete>false</CanUserDelete>" +
                            "<Name>Enum2</Name>" +
                            "<EnumList>Sdfjkfh#iluio#jkldflkjg#sdrgfg</EnumList>" +
                            "<EnumGuid>Test String5</EnumGuid>" +
                        "</SoftParameterEnum>" +
                        "<SoftParameterEnum>" +
                            "<Id>3</Id>" +
                            "<CanUserDelete>false</CanUserDelete>" +
                            "<Name>Enum2</Name>" +
                            "<EnumList>Sdfjkfh#fds#guy#fred</EnumList>" +
                            "<EnumGuid>Test String5</EnumGuid>" +
                        "</SoftParameterEnum>" +
                    "</SoftParameterEnums>";
                return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(tmp));
            }
            set { xmlStream = value; }
        }
    }
}
