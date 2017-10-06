using AC30CustomLanguageInterfaces.Interfaces;
using System.Collections.Generic;
using System.Globalization;
using System;
using System.Linq;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;

namespace AC30CustomLanguageManagerApp.Model
{
    public class LanguageStringCollection : ObservableObject, ILanguageStringCollection, IComparable
    {
        List<int> languageNameStringIds = new List<int>() { 848, 849, 850, 851, 852, 1399, 1400, 1401, 1402, 1180 };
        List<string> languageNameString = new List<string>() { "ENGLISH", "FRANCAIS", "DEUTSCH", "ITALIANO", "ESPANOL", "CHINESE", "L6", "L7", "L8", "L9" };
        
        public int IdForLanguageString { get; set; }
        public string Language { get; set; }
        public int Index { get; set; }

        [XmlIgnore]
        public List<ITranslation> Translations { get; set; }
        public List<Translation> TranslationsInternal { get; set; }
        public LanguageStringCollection()
        {
            Translations = new List<ITranslation>();
        }
        public LanguageStringCollection(int index)
        {
            Translations = new List<ITranslation>();
            Index = index;
            Initialize();
        }
        private void Initialize()
        {
            IdForLanguageString = languageNameStringIds[Index];
            Language = languageNameString[Index];
            if (Index == 0) Translations.Add(new Translation() { StringId = 0 });
        }
        public void SetPropertiesBeforeSerializing()
        {
            try
            {
                TranslationsInternal = new List<Translation>();
                foreach (var t in Translations)
                {
                    TranslationsInternal.Add((Translation)t);
                }
            }
            catch { }
        }
        public void ClearPropertiesAfterSerializing()
        {
            try
            {
                TranslationsInternal.Clear();
            }
            catch { }
        }
        public void SetPropertiesAfterDeserializing()
        {
            try
            {
                Translations = TranslationsInternal.AsEnumerable<ITranslation>().ToList();
                TranslationsInternal.Clear();
            }
            catch { }
        }
        public int CompareTo(object other)
        {
            if (other == null) return 1;
            return this.Index.CompareTo(((ILanguageStringCollection)other).Index);
        }
    }
}
