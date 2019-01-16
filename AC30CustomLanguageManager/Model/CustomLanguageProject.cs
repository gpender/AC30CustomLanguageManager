using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using Microsoft.Win32;
using System.Xml.Serialization;
using Parker.AP.Common.CustomLanguages;
using System.Windows;

namespace AC30CustomLanguageManager.Model
{
    public class CustomLanguageProject : ICustomLanguageProject1
    {
        string LanguageStringProjectFileFilter = "AC30 Language Strings (*.ac30languages.xml) |*.ac30languages.xml| All Files (*.*)|*.*";
        string LanguageStringProjectFileExtension = "ac30languages.xml";
        List<int> languageNameStringIds = new List<int>() { 848, 849, 850, 851, 852, 1399, 1400, 1401, 1402, 1697 };
        List<string> languageNameStrings = new List<string>() { "ENGLISH", "FRANCAIS", "DEUTSCH", "ESPANOL", "ITALIANO", "CHINESE", "L6", "L7", "L8", "L9" };
        Dictionary<int, Dictionary<int, string>> originalStringTranslations;
        List<ILanguage> languages = new List<ILanguage>();

        public string CompilerWarning { get; set; }
        public string DeviceXmlVersionString { get; set; }
        [XmlIgnore]
        public Version DeviceXmlVersion { get; private set; }
        public uint DriveCustomizationSignature { get; set; }
        public uint DriveCustomizationSignaturePre3570 { get; set; }
        public string ProjectFileName { get; set; }
        public ushort FixedStringCount { get; set; }
        [XmlIgnore]
        public List<ILanguage> Languages
        {
            get { return languages; }
        }
        public List<LanguageStringCollection> LanguageStringCollectionsInternal { get; set; }
        [XmlIgnore]
        public List<ILanguageStringCollection> LanguageStringCollections { get; set; }
        public ushort SoftStringCount { get; set; }
        public CustomLanguageProject()
        {
            DeviceXmlVersion = new Version(0, 0, 0, 0);
            InitializeLanguageStringCollections();
            ProjectFileName = "MyProject";
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
        public void ImportDeviceXmlFile(IDeviceStringReader deviceStringReader, IDeviceXmlProvider deviceXmlProvider)
        {
            throw new NotImplementedException();
        }
        public void ImportDeviceXmlFile(string fileLocation, IDeviceStringReader deviceStringReader, IDeviceXmlProvider deviceXmlProvider)
        {
            SaveOriginalTranslations(false);

            ClearAllFixedTranslations();
            AddLanguageStringCollection(fileLocation, deviceStringReader, deviceXmlProvider);

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
            DriveCustomizationSignaturePre3570 = driveCustomizationStringReader.DriveCustomizationSignaturePre3570;
            CompilerWarning = driveCustomizationStringReader.CompilerWarning;
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
                ProjectFileName = new FileInfo(languageStringProjectFileName).Name;
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
            DriveCustomizationSignature = deserializedProject.DriveCustomizationSignature;
            FixedStringCount = deserializedProject.FixedStringCount;
            SoftStringCount = deserializedProject.SoftStringCount;
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
            saveFileDialog.FileName = ProjectFileName;
            saveFileDialog.DefaultExt = LanguageStringProjectFileExtension;

            if (saveFileDialog.ShowDialog() == true)
            {
                string saveFileName = saveFileDialog.FileName;
                Serialize(saveFileName);
                ProjectFileName = new FileInfo(saveFileName).Name;
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
            try
            {
                FixedStringCount = Convert.ToUInt16(LanguageStringCollections[0].Translations.Where(s => !s.IsSoftString).Max(s => s.StringId) + 1);
            }
            catch { }
            try
            {
                SoftStringCount = Convert.ToUInt16(LanguageStringCollections[0].Translations.Where(s => s.IsSoftString).Count());
            }
            catch { }
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
        private void AddLanguageStringCollection(string fileLocation, IDeviceStringReader deviceStringReader, IDeviceXmlProvider deviceXmlProvider)
        {
            try
            {
                foreach (var languageStringCollection in ((IDeviceStringReader1)deviceStringReader).GetStringsFromDeviceXml(fileLocation,deviceXmlProvider))
                {
                    ILanguageStringCollection matchingCollection = LanguageStringCollections.Where(lsc => lsc.Index == languageStringCollection.Index).FirstOrDefault();
                    if (matchingCollection != null)
                    {
                        matchingCollection.Translations.AddRange(languageStringCollection.Translations);
                    }
                    else
                    {
                        LanguageStringCollections.Add(languageStringCollection);
                    }
                }

                if (LanguageStringCollections[0].Translations.Count > 0)
                {
                    FixedStringCount = Convert.ToUInt16(LanguageStringCollections[0].Translations.Where(s => !s.IsSoftString).Max(s => s.StringId) + 1);
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void AddLanguageStringCollection(IDriveCustomizationStringReader driveCustomizationStringReader, IDriveCustomizationXmlProvider driveCustomizationXmlProvider)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ClearAllFixedTranslations()
        {
            foreach (var l in LanguageStringCollections)
            {
                l.Translations.RemoveAll(t => t.IsSoftString == false);
            }
            FixedStringCount = 0;
        }
        private void ClearAllSoftTranslations()
        {
            foreach (var l in LanguageStringCollections)
            {
                l.Translations.RemoveAll(t => t.IsSoftString == true);
            }
            SoftStringCount = 0;
        }
        private void InitializeLanguageStringCollections()
        {
            LanguageStringCollections = new List<ILanguageStringCollection>();
            // Set the Language property according to the strings with string ids as follows
            for (int i = 0; i < 10; i++)
            {
                LanguageStringCollections.Add(new LanguageStringCollection(i));
            }
            if (this.languages.Count == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    this.languages.Add(new Language(i, languageNameStringIds[i], languageNameStrings[i]));
                }
            }
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
            //Restore Any Changes to The English version of the Defined Languages
            foreach (var lsc in LanguageStringCollections)
            {
                int languageStringId = Languages[lsc.Index].IdForLanguageString;
                var baseEnglishVersionOfDefinedLanguage = LanguageStringCollections[0].Translations.Where(t => t.StringId == languageStringId).FirstOrDefault();
                var originalLanguage = originalStringTranslations[0].ContainsKey(languageStringId) ? originalStringTranslations[0][languageStringId] : string.Empty;
                if (baseEnglishVersionOfDefinedLanguage != null && !string.IsNullOrEmpty(originalLanguage))
                {
                    baseEnglishVersionOfDefinedLanguage.String = originalLanguage;
                }
            }
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
        #endregion
    }

}
