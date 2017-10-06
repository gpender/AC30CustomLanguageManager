using Parker.AP.Common.CustomLanguages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace AC30CustomLanguageManager.Model
{
    public class DeviceStringReader : IDeviceStringReader
    {
        public Version DeviceXmlVersion { get; private set; }
        public DeviceStringReader()
        {}

        public IEnumerable<ILanguageStringCollection> GetStringsFromDeviceXml(IDeviceXmlProvider deviceXmlProvider)
        {
            XDocument doc = null;
            using (Stream stream = deviceXmlProvider.XmlStream)
            {
                doc = XDocument.Load(stream);
            }
            return GetLanguagesFromStringsSection(deviceXmlProvider.NameSpace,doc);
        }
        private IEnumerable<ILanguageStringCollection> GetLanguagesFromStringsSection(XNamespace nameSpace, XDocument doc)
        {
            bool englishStringsPresentInDeviceXml = doc.Descendants(nameSpace + "Strings").Descendants(nameSpace + "Language").Where(a => a.Attribute("lang").Value == "en").FirstOrDefault() != null;
            int index = englishStringsPresentInDeviceXml ? 0 : 1;

            List<ILanguageStringCollection> availableLanguageStrings = (from c in doc.Descendants(nameSpace + "Strings").Descendants(nameSpace + "Language")
                                                                               select new LanguageStringCollection(index++)
                                                                               {
                                                                                   Translations = (from t in c.Descendants(nameSpace + "String")
                                                                                                   select new Translation()
                                                                                                   {
                                                                                                       StringId = int.Parse(t.Attribute("identifier").Value.Replace("id", "")),
                                                                                                       String = t.Value
                                                                                                   }).ToList<ITranslation>().Where(t=>t.StringId < 10000).ToList()
                                                                               }).ToList<ILanguageStringCollection>();

            // if english not present in the xml file as a language extract english strings from all the parameter and menu objects
            if(!englishStringsPresentInDeviceXml)
            {
                availableLanguageStrings.Insert(0, GetEnglishStringsFromXmlElements(nameSpace,doc));
            }
            GetDeviceVersion(nameSpace,doc);
            return availableLanguageStrings;
        }

        private void GetDeviceVersion(XNamespace nameSpace, XDocument doc)
        {
            try
            {
                DeviceXmlVersion = (from c in doc.Descendants(nameSpace + "Device").Descendants(nameSpace + "DeviceIdentification").Descendants(nameSpace + "Version")
                                    select Version.Parse(c.Value)).FirstOrDefault();
            }
            catch
            {
                DeviceXmlVersion = new Version(0, 0, 0, 0);
            }
        }

        /// <summary>
        /// If the device.xml file does not have en-GB in the Strings section then scan through xml and collate any local:idxxx strings
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="translationMaster"></param>
        private ILanguageStringCollection GetEnglishStringsFromXmlElements(XNamespace nameSpace,XDocument doc)//, ITranslationMaster translationMaster)
        {
            ILanguageStringCollection englishLanguageStringCollection = new LanguageStringCollection(0) { Translations = new List<ITranslation>() };
            List<ITranslation> withDupes = (from c in doc.Descendants(nameSpace + "DeviceDescription").Descendants().Elements().Where(c => c.Attribute("name") != null && c.Attribute("name").Value.StartsWith("local:id")) select new Translation { StringId = int.Parse(c.Attribute("name").Value.Replace("local:id", "")), String = c.Value }).ToList<ITranslation>();
            withDupes.Sort();
            IEnumerable<ITranslation> noDupes = withDupes.GroupBy(x => x.StringId).Select(x => x.First());// because withDupes.Distinct() DOES NOT WORK!!!!!!;
            IEnumerable<ITranslation> result = noDupes.Where(x => x.StringId > 0 && x.StringId < 10000); // Only return strings > 0  and less than 10000
            foreach (var r in result)
            {

                if (!englishLanguageStringCollection.Translations.Contains(r))
                {
                    englishLanguageStringCollection.Translations.Add(r);
                }
            }
            return englishLanguageStringCollection;
        }
    }

}
