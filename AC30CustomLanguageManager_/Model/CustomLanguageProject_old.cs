using AC30CustomLanguageInterfaces.Interfaces;
using AC30CustomLanguageManagerApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AC30CustomLanguageManager.Model
{
    //public class CustomLanguageProject : ICustomLanguageProject
    //{
    //    public static string LanguageStringProjectFileFilter = "AC30 Language Strings (*.ac30languages.xml) |*.ac30languages.xml| All Files (*.*)|*.*";
    //    public static string LanguageStringProjectFileExtension = "ac30languages.xml";

    //    public string DeviceXmlVersionString { get; set; }
    //    [XmlIgnore]
    //    public Version DeviceXmlVersion { get; set; }
    //    public List<LanguageStringCollection> LanguageStringCollections { get; set; }

    //    public CustomLanguageProject()
    //    {
    //        DeviceXmlVersion = new Version(0, 0, 0, 0);
    //        LanguageStringCollections = new List<LanguageStringCollection>();
    //    }
    //    internal void SetPropertiesAfterDeSerializing()
    //    {
    //        try { DeviceXmlVersion = Version.Parse(DeviceXmlVersionString); } catch { }
    //        foreach (LanguageStringCollection languageStringCollection in LanguageStringCollections)
    //        {
    //            languageStringCollection.SetPropertiesAfterDeserializing();
    //        }
    //        LanguageStringCollections.Clear();
    //    }

    //    internal void Serialize(string saveFileName, List<ILanguageStringCollection> languageStringCollections)
    //    {
    //        DeviceXmlVersionString = DeviceXmlVersion.ToString();
    //        SetPropertiesBeforeSerializing(languageStringCollections);
    //        XmlSerializer mySerializer = new XmlSerializer(typeof(CustomLanguageProject));
    //        using (StreamWriter myWriter = new StreamWriter(saveFileName))
    //        {
    //            mySerializer.Serialize(myWriter, this);
    //            myWriter.Close();
    //        }
    //        ClearPropertiesAfterSerializing(languageStringCollections);
    //    }

    //    void SetPropertiesBeforeSerializing(List<ILanguageStringCollection> languageStringCollections)
    //    {
    //        foreach (LanguageStringCollection languageStringCollection in languageStringCollections)
    //        {
    //            languageStringCollection.SetPropertiesBeforeSerializing();
    //            LanguageStringCollections.Add(languageStringCollection);
    //        }
    //    }

    //    void ClearPropertiesAfterSerializing(List<ILanguageStringCollection> languageStringCollections)
    //    {
    //        foreach (LanguageStringCollection languageStringCollection in languageStringCollections)
    //        {
    //            languageStringCollection.ClearPropertiesAfterSerializing();
    //        }
    //        LanguageStringCollections.Clear();
    //    }
    //}
}
