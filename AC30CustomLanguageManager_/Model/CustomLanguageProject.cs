using AC30CustomLanguageInterfaces.Interfaces;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System;
using Microsoft.Win32;
using System.Xml.Serialization;

namespace AC30CustomLanguageManagerApp.Model
{
    public class CustomLanguageProject : ObservableObject, ICustomLanguageProject
    {
        string LanguageStringProjectFileFilter = "AC30 Language Strings (*.ac30languages.xml) |*.ac30languages.xml| All Files (*.*)|*.*";
        string LanguageStringProjectFileExtension = "ac30languages.xml";
        Dictionary<int, Dictionary<int, string>> originalStringTranslations;

        public string DeviceXmlVersionString { get; set; }
        [XmlIgnore]
        public Version DeviceXmlVersion { get; private set; }
        public uint DriveCustomizationSignature { get; set; }
        public ushort FixedStringCount { get; set; }
        public List<LanguageStringCollection> LanguageStringCollectionsInternal { get; set; }
        [XmlIgnore]
        public List<ILanguageStringCollection> LanguageStringCollections { get; set; }
        public ushort SoftStringCount { get; set; }

        public CustomLanguageProject()
        {
            DeviceXmlVersion = new Version(0, 0, 0, 0);
            InitializeLanguageStringCollections();
        }
        public void ImportDeviceXmlFile(IDeviceStringReader deviceStringReader, IDeviceXmlProvider deviceXmlProvider)
        {
            SaveOriginalTranslations(false);

            ClearAllFixedTranslations();
            AddLanguageStringCollection(deviceStringReader, deviceXmlProvider);

            RestoreOriginalTranslations();
            DeviceXmlVersion = deviceStringReader.DeviceXmlVersion;
        }
        public void ImportDriveCustomizationXmlFile(IDriveCustomizationStringReader driveCustomizationStringReader, IDriveCustomizationXmlProvider driveCustomizationXmlProvider)
        {
            SaveOriginalTranslations(true);

            ClearAllSoftTranslations();
            AddLanguageStringCollection(driveCustomizationStringReader, driveCustomizationXmlProvider);

            RestoreOriginalTranslations();
            DriveCustomizationSignature = driveCustomizationStringReader.DriveCustomizationSignature;
        }

        private void SaveOriginalTranslations(bool isSoftString)
        {
            //store the current base strings for DriveCustomization by StringId for use later to populate any matching strings once imported with their translations
            originalStringTranslations = new Dictionary<int, Dictionary<int, string>>();
            foreach (var l in LanguageStringCollections)
            {
                Dictionary<int, string> tmp = new Dictionary<int, string>();
                foreach (var t in l.Translations)
                {
                    if (t.IsSoftString == isSoftString)
                    {
                        tmp.Add(t.StringId, t.String);
                    }
                }
                originalStringTranslations.Add(l.Index, tmp);
            }
        }

        private void RestoreOriginalTranslations()
        {
            foreach (var t in LanguageStringCollections[0].Translations)
            {
                int newStringId = t.StringId;
                string newString = t.String;
                var search = originalStringTranslations[0].Where(st => st.Value == newString).FirstOrDefault();
                if (search.Key != 0)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        var originalTranslation = originalStringTranslations[i][search.Key];
                        if (!string.IsNullOrEmpty(originalTranslation))
                        {
                            ITranslation translation = LanguageStringCollections[i].Translations.Where(tr => tr.StringId == newStringId).FirstOrDefault();
                            if (translation != null)
                            {
                                translation.String = originalTranslation;
                            }
                        }
                    }
                }
            }
            originalStringTranslations.Clear();
        }

        public void SetReferenceStringProvider(IReferenceStringProvider referenceStringProvider)
        {
            if (referenceStringProvider != null)
            {
                foreach (var lsc in LanguageStringCollections)
                {
                    foreach (var t in lsc.Translations)
                    {
                        t.SetReferenceStringProvider(referenceStringProvider);
                    }
                }
            }
        }

        #region Serialization methods

        public void Deserialize()
        {
            string languageStringProjectFileName = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = LanguageStringProjectFileFilter;
            openFileDialog.DefaultExt = LanguageStringProjectFileExtension;

            if (openFileDialog.ShowDialog() == true)
            {
                languageStringProjectFileName = openFileDialog.FileName;
                XmlSerializer mySerializer = new XmlSerializer(typeof(CustomLanguageProject));
                try
                {
                    CustomLanguageProject deserializedProject;
                    using (FileStream myFileStream = new FileStream(languageStringProjectFileName, FileMode.Open))
                    {
                        deserializedProject = (CustomLanguageProject)mySerializer.Deserialize(myFileStream);
                        myFileStream.Close();
                    }
                    SetPropertiesAfterDeSerializing(deserializedProject);
                }
                catch { }
            }
        }
        internal void SetPropertiesAfterDeSerializing(CustomLanguageProject deserializedProject)
        {
            try { DeviceXmlVersion = Version.Parse(deserializedProject.DeviceXmlVersionString); } catch { }
            foreach (LanguageStringCollection languageStringCollection in deserializedProject.LanguageStringCollectionsInternal)
            {
                languageStringCollection.SetPropertiesAfterDeserializing();
            }
            LanguageStringCollections = deserializedProject.LanguageStringCollectionsInternal.AsEnumerable<ILanguageStringCollection>().ToList();
        }

        public void Serialize()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = LanguageStringProjectFileFilter;
            saveFileDialog.DefaultExt = LanguageStringProjectFileExtension;

            if (saveFileDialog.ShowDialog() == true)
            {
                string saveFileName = saveFileDialog.FileName;
                Serialize(saveFileName);
            }
        }
        internal void Serialize(string saveFileName)
        {
            SetPropertiesBeforeSerializing();
            XmlSerializer mySerializer = new XmlSerializer(typeof(CustomLanguageProject));
            using (StreamWriter myWriter = new StreamWriter(saveFileName))
            {
                mySerializer.Serialize(myWriter, this);
                myWriter.Close();
            }
            ClearPropertiesAfterSerializing();
        }
        void SetPropertiesBeforeSerializing()
        {
            DeviceXmlVersionString = DeviceXmlVersion.ToString();
            LanguageStringCollectionsInternal = new List<LanguageStringCollection>();
            foreach (LanguageStringCollection languageStringCollection in LanguageStringCollections)
            {
                languageStringCollection.SetPropertiesBeforeSerializing();
                LanguageStringCollectionsInternal.Add(languageStringCollection);
            }
        }
        void ClearPropertiesAfterSerializing()
        {
            foreach (LanguageStringCollection languageStringCollection in LanguageStringCollections)
            {
                languageStringCollection.ClearPropertiesAfterSerializing();
            }
            LanguageStringCollectionsInternal.Clear();
        }
        #endregion
        
        #region private methods
        private void AddLanguageStringCollection(IDeviceStringReader deviceStringReader, IDeviceXmlProvider deviceXmlProvider)
        {
            foreach (var languageStringCollection in deviceStringReader.GetStringsFromDeviceXml(deviceXmlProvider))
            {
                ILanguageStringCollection matchingCollection = LanguageStringCollections.Where(lsc => lsc.Index == languageStringCollection.Index).FirstOrDefault();
                if (matchingCollection != null)
                {
                    //matchingCollection.Translations = languageStringCollection.Translations;
                    matchingCollection.Translations.AddRange(languageStringCollection.Translations);
                }
                else
                {
                    LanguageStringCollections.Add(languageStringCollection);
                }
            }

            if (LanguageStringCollections[0].Translations.Count > 0)
            {
                FixedStringCount = Convert.ToUInt16(LanguageStringCollections[0].Translations.Where(s=>!s.IsSoftString).Max(s => s.StringId) + 1);
                //Find missing stringIds
                foreach (var als in LanguageStringCollections)
                {
                    var list = als.Translations.Select(s => s.StringId).OrderBy(i => i);
                    var result = Enumerable.Range(0, FixedStringCount).Except(list);
                    als.Translations.AddRange(
                        from missing in result
                        select new Translation()
                        {
                            StringId = missing,
                            String = String.Empty
                        }
                        );
                    als.Translations.Sort();
                }
            }
            LanguageStringCollections.Sort();
        }
        private void AddLanguageStringCollection(IDriveCustomizationStringReader driveCustomizationStringReader, IDriveCustomizationXmlProvider driveCustomizationXmlProvider)
        {
            List<ITranslation> softStrings = driveCustomizationStringReader.GetStringsFromDriveCustomization(driveCustomizationXmlProvider).ToList();
            int minId = softStrings.Min(t => t.StringId);
            int maxId = softStrings.Max(t => t.StringId);
            SoftStringCount = Convert.ToUInt16(maxId - minId + 1);// Convert.ToUInt16(softStrings.Count);

            LanguageStringCollections[0].Translations.AddRange(softStrings);
            //Find missing stringIds
            foreach (var als in LanguageStringCollections)
            {
                var list = als.Translations.Select(s => s.StringId).OrderBy(i => i);
                var result = Enumerable.Range(minId, SoftStringCount).Except(list);
                als.Translations.AddRange(
                    from missing in result
                    select new Translation()
                    {
                        IsSoftString = true,
                        StringId = missing,
                        String = String.Empty
                    }
                    );
                als.Translations.Sort();
            }
            LanguageStringCollections.Sort();
        }
        private void InitializeLanguageStringCollections()
        {
            LanguageStringCollections = new List<ILanguageStringCollection>();
            // Set the Language property according to the strings with string ids as follows
            for (int i = 0; i < 10; i++)
            {
                LanguageStringCollections.Add(new LanguageStringCollection(i));
            }
        }

        public void ClearAllTranslations()
        {
            foreach (var l in LanguageStringCollections)
            {
                l.Translations.Clear();
            }
            FixedStringCount = 0;
            SoftStringCount = 0;
        }
        public void ClearAllFixedTranslations()
        {
            foreach (var l in LanguageStringCollections)
            {
                l.Translations.RemoveAll(t => t.IsSoftString == false);
            }
            FixedStringCount = 0;
        }
        public void ClearAllSoftTranslations()
        {
            foreach (var l in LanguageStringCollections)
            {
                l.Translations.RemoveAll(t => t.IsSoftString == true);
            }
            SoftStringCount = 0;
        }

        #endregion

    }

}
