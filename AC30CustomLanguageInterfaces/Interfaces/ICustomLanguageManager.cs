using System;
using System.Collections.Generic;

namespace AC30CustomLanguageInterfaces.Interfaces
{
    public interface ICustomLanguageManager
    {
        List<ILanguageStringCollection> LanguageStringCollections { get; }
        Version DeviceXmlVersion { get; }
        uint DriveCustomizationSignature { get; }
        ILanguageStringCollection SelectedLanguageStringCollection { get; set; }
        ILanguageStringCollection SelectedReferenceStringCollection { get; set; }
        //ITranslationManager TranslationManager { get; set; }
        void CreateLanguageFile(bool allLanguageFiles);
        void NewCustomLanguageProject();
        void OpenCustomLanguageProject();
        void SaveCustomLanguageProject();
        void ImportDeviceXmlFile();
        void ImportDriveCustomizationXmlFile();

    }
}