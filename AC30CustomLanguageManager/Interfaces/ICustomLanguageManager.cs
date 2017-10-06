using System;
using System.Collections.Generic;

namespace Parker.AP.Common.CustomLanguages
{
    public interface ICustomLanguageManager
    {
        event EventHandler SourceDataChanged;
        //event EventHandler StringsChanged;
        //event EventHandler SelectedLanguageChanged;
        //event EventHandler SelectedReferenceLanguageChanged;
        string CompilerWarning { get; }
        bool EditingLanguages { get; set; }
        List<ILanguage> Languages { get; }
        List<ILanguageStringCollection> LanguageStringCollections { get; }
        Version DeviceXmlVersion { get; }
        uint DriveCustomizationSignature { get; }
        uint DriveCustomizationSignaturePre3570 { get; }
        ILanguage SelectedLanguage { get; set; }
        ILanguage SelectedReferenceLanguage { get; set; }
        ILanguageStringCollection SelectedLanguageStringCollection { get; }
        ILanguageStringCollection SelectedReferenceStringCollection { get; }

        //ITranslationManager TranslationManager { get; set; }
        void CreateLanguageFile(bool allLanguageFiles);
        void NewCustomLanguageProject();
        void OpenCustomLanguageProject();
        void SaveCustomLanguageProject();
        void ImportDeviceXmlFile();
        void ImportDriveCustomizationXmlFile();

    }
}