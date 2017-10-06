using System;
using System.Collections.Generic;
using System.Globalization;

namespace Parker.AP.Common.CustomLanguages
{
    public interface ICustomLanguageProject
    {
        /// <summary>
        /// AC30 allows for 10 languages with the file format
        /// language0.lang - langauge9.lang
        /// </summary>

        string CompilerWarning { get; set; }
        Version DeviceXmlVersion { get; }
        uint DriveCustomizationSignature { get; set; }
        uint DriveCustomizationSignaturePre3570 { get; set; }
        List<ILanguage> Languages { get; }
        List<ILanguageStringCollection> LanguageStringCollections { get; set; }
        void SetReferenceStringProvider(IReferenceStringProvider referenceStringProvider);
        ushort FixedStringCount { get; set; }
        ushort SoftStringCount { get; set; }


        void ImportDeviceXmlFile(IDeviceStringReader deviceStringReader, IDeviceXmlProvider deviceXmlProvider);
        void ImportDriveCustomizationXmlFile(IDriveCustomizationStringReader driveCustomizationStringReader, IDriveCustomizationXmlProvider driveCustomizationXmlProvider);
        void ClearAllTranslations();

        void Deserialize();
        void Serialize();
    }
}