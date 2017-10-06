using AC30CustomLanguageInterfaces.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AC30CustomLanguageManagerApp.Model
{
    public class DriveCustomizationStringReader : IDriveCustomizationStringReader
    {
        /// <summary>
        /// String Id order as defined by DriveCustomization object in Codesys 
        /// Enums --> Tags --> Menus (sorted by their own object id)
        /// </summary>
        /// 
        public uint DriveCustomizationSignature { get; private set; }

        public DriveCustomizationStringReader()
        {
        }

        public IEnumerable<ITranslation> GetStringsFromDriveCustomization(IDriveCustomizationXmlProvider driveCustomizationXmlProvider)
        {
            XDocument doc = null;
            using (Stream stream = driveCustomizationXmlProvider.XmlStream)
            {
                doc = XDocument.Load(stream);
            }
            return GetStringsFromXml(driveCustomizationXmlProvider.NameSpace,doc);
        }

        private IEnumerable<ITranslation> GetStringsFromXml(XNamespace nameSpace, XDocument doc)
        {
            int i = 5000;
            List<ITranslation> allTranslations = new List<ITranslation>();
            GetDriveCustomizationSignature(nameSpace,doc);

            List<ITranslation> softEnumTranslations = (from t in doc.Descendants(nameSpace + "SoftParameterEnums")
                                                .Descendants(nameSpace + "SoftParameterEnum").OrderBy(e => int.Parse(e.Descendants("Id").FirstOrDefault().Value))
                                                .Descendants(nameSpace + "EnumList")
                                                       from x in t.Value.Split('#')
                                                       select new Translation()
                                                       {
                                                           IsSoftString = true,
                                                           StringId = i++,
                                                           String = x
                                                       }).ToList<ITranslation>();

            List<ITranslation> softParameterTranslations = (from t in doc.Descendants(nameSpace + "SoftParameters")
                                                .Descendants(nameSpace + "SoftParameter").OrderBy(e => int.Parse(e.Descendants("Tag").FirstOrDefault().Value))
                                                .Descendants(nameSpace + "Name")
                                                            select new Translation()
                                                            {
                                                                IsSoftString = true,
                                                                StringId = i++,
                                                                String = t.Value
                                                            }).ToList<ITranslation>();

            List<int> usedVariableUnits = (from t in doc.Descendants(nameSpace + "SoftParameters")
                                                .Descendants(nameSpace + "SoftParameter")
                                                .Descendants(nameSpace + "SoftParameterUnitSelectorId").Where(e => !string.IsNullOrEmpty(e.Value))
                                                            select int.Parse(t.Value)).ToList();


            List<ITranslation> softParameterUnitsTranslations = (from t in doc.Descendants(nameSpace + "SoftParameters")
                                                .Descendants(nameSpace + "SoftParameter").Where(p => p.Descendants("IsVariableUnits").FirstOrDefault().Value.ToUpperInvariant() != "TRUE")
                                                .OrderBy(e => int.Parse(e.Descendants("Tag").FirstOrDefault().Value))
                                                .Descendants(nameSpace + "Units")
                                                        select new Translation()
                                                        {
                                                            IsSoftString = true,
                                                            StringId = i++,
                                                            String = t.Value
                                                        }).ToList<ITranslation>();

            List<ITranslation> softParameterVariableUnitTranslations = (from t in doc.Descendants(nameSpace + "SoftParameterUnitSelectorsInCurrentUse")
                                                 .Descendants(nameSpace + "SoftParameterUnitSelector").OrderBy(e => int.Parse(e.Descendants("Id").FirstOrDefault().Value))
                                                 .Descendants()
                                                    where t.Name.ToString().EndsWith("Units")
                                                        //.Descendants(nameSpace + "BaseUnits")
                                                        select new Translation()
                                                        {
                                                            IsSoftString = true,
                                                            StringId = i++,
                                                            String = t.Value
                                                        }).ToList<ITranslation>();

            List<ITranslation> topMenuTranslations = (from t in doc.Descendants(nameSpace + "TopMenusInCurrentUse")
                                                .Descendants(nameSpace + "SoftMenuNew").OrderBy(e => int.Parse(e.Descendants("Id").FirstOrDefault().Value))
                                                .Descendants(nameSpace + "Name")
                                                       select new Translation()
                                                       {
                                                           IsSoftString = true,
                                                           StringId = i++,
                                                           String = t.Value
                                                       }).ToList<ITranslation>();

            List<ITranslation> softMenuTranslations = (from t in doc.Descendants(nameSpace + "SoftMenusInCurrentUse")
                                                .Descendants(nameSpace + "SoftMenuNew").OrderBy(e => int.Parse(e.Descendants("Id").FirstOrDefault().Value))
                                                .Descendants(nameSpace + "Name")
                                                       select new Translation()
                                                       {
                                                           IsSoftString = true,
                                                           StringId = i++,
                                                           String = t.Value
                                                       }).ToList<ITranslation>();

            i = 5000;
            i = AddTranslations(i, allTranslations, softEnumTranslations);
            i = AddTranslations(i, allTranslations, softParameterTranslations);
            i = AddTranslations(i, allTranslations, softParameterUnitsTranslations);
            i = AddTranslations(i, allTranslations, softParameterVariableUnitTranslations);
            i = AddTranslations(i, allTranslations, topMenuTranslations);
            i = AddTranslations(i, allTranslations, softMenuTranslations);
            //allTranslations.AddRange(softEnumTranslations);
            //allTranslations.AddRange(softParameterUnitsTranslations);
            //allTranslations.AddRange(softParameterVariableUnitTranslations);
            //allTranslations.AddRange(softParameterTranslations);
            //allTranslations.AddRange(softMenuTranslations);
            return allTranslations;
        }

        private int AddTranslations (int i, List<ITranslation> allTranslations, List<ITranslation> translations)
        {
            foreach (var t in translations)
            {
                if (!string.IsNullOrEmpty(t.String))
                {
                    if (!allTranslations.Contains(t, new TranslationBaseStringEqualityComparer()))
                    {
                        t.StringId = i++;
                        allTranslations.Add(t);
                    }
                }
            }
            return i;
        }

        private void GetDriveCustomizationSignature(XNamespace nameSpace, XDocument doc)
        {
            try
            {
                DriveCustomizationSignature = (from c in doc.Descendants(nameSpace + "Signature")
                                    select uint.Parse(c.Value)).FirstOrDefault();
            }
            catch
            {
                DriveCustomizationSignature = 0;
            }
        }
    }

}
