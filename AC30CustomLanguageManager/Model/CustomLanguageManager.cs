using Parker.AP.Common.CustomLanguages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AC30CustomLanguageManager.Model
{
    public class CustomLanguageManager : ICustomLanguageManager1, IDisposable
    {
        public event EventHandler SourceDataChanged;
        bool disableEventRaising = false;

        ICustomLanguageProject1 currentProject;
        ILanguageFileGenerator languageFileGenerator;
        ILanguageStringCollection selectedLanguageStringCollection;
        ILanguageStringCollection selectedReferenceStringCollection;
        IDeviceStringReader1 deviceStringReader;
        IDeviceXmlProvider1 deviceXmlProvider;
        IDriveCustomizationStringReader driveCustomizationStringReader;
        IDriveCustomizationXmlProvider driveCustomizationXmlProvider;
        IStringsChangedNotifier stringsChangedNotifier;
        ILanguage selectedLanguage;
        ILanguage selectedReferenceLanguage;
        bool editingLanguages;

        public List<IDeviceXmlFile> AvailableDeviceXmlFiles
        {
            get { return deviceXmlProvider.AvailableDeviceXmlFiles; }
        }
        public string CompilerWarning
        {
            get { return currentProject.CompilerWarning; }
        }
        public uint DriveCustomizationSignature
        {
            get { return currentProject.DriveCustomizationSignature; }
        }
        public uint DriveCustomizationSignaturePre3570
        {
            get { return currentProject.DriveCustomizationSignaturePre3570; }
        }
        public Version DeviceXmlVersion
        {
            get { return currentProject.DeviceXmlVersion; }
        }
        public bool EditingLanguages
        {
            get { return editingLanguages; }
            set
            {
                editingLanguages = value;
                UpdateBaseLanguages();
                if(editingLanguages) SetSelectedLanguageAndReferenceLanguage2();
            }
        }
        public List<ILanguage> Languages
        {
            get { return currentProject.Languages; }
        }
        public List<ILanguageStringCollection> LanguageStringCollections
        {
            get { return currentProject.LanguageStringCollections; }
        }
        public ILanguageStringCollection SelectedLanguageStringCollection
        {
            get { return selectedLanguageStringCollection; }
        }
        public ILanguage SelectedLanguage
        {
            get { return selectedLanguage; }
            set
            {
                selectedLanguage = value;
                if (selectedLanguage != null && currentProject != null)
                {
                    selectedLanguageStringCollection = currentProject.LanguageStringCollections[selectedLanguage.Index];
                }
                if (!disableEventRaising) SourceDataChanged?.Invoke(this, new EventArgs());
            }
        }
        public ILanguage SelectedReferenceLanguage
        {
            get { return selectedReferenceLanguage; }
            set
            {
                selectedReferenceLanguage = value;
                if (selectedReferenceLanguage != null)
                {
                    selectedReferenceStringCollection = currentProject.LanguageStringCollections[selectedReferenceLanguage.Index];
                    currentProject.SetReferenceStringProvider(new ReferenceStringProvider(selectedReferenceStringCollection));
                }
                if (!disableEventRaising) SourceDataChanged?.Invoke(this, new EventArgs());
            }
        }
        public ILanguageStringCollection SelectedReferenceStringCollection
        {
            get { return selectedReferenceStringCollection; }
        }
        public CustomLanguageManager(IDeviceStringReader deviceStringReader, IDeviceXmlProvider deviceXmlProvider, IDriveCustomizationStringReader driveCustomizationStringReader, IDriveCustomizationXmlProvider driveCustomizationXmlProvider, ILanguageFileGenerator languageFileGenerator, IStringsChangedNotifier stringsChangedNotifier)
        {
            this.deviceStringReader = deviceStringReader as IDeviceStringReader1;
            this.deviceXmlProvider = deviceXmlProvider as IDeviceXmlProvider1;
            this.driveCustomizationStringReader = driveCustomizationStringReader;
            this.driveCustomizationXmlProvider = driveCustomizationXmlProvider;
            this.languageFileGenerator = languageFileGenerator;
            this.stringsChangedNotifier = stringsChangedNotifier;

            currentProject = new CustomLanguageProject();
            SetSelectedLanguageAndReferenceLanguage();

            if (stringsChangedNotifier != null)
            {
                stringsChangedNotifier.DeviceChanged += StringsChangedNotifier_DeviceChanged;
                stringsChangedNotifier.DriveCustomizationChanged += StringsChangedNotifier_DriveCustomizationChanged;
            }
        }
        private void StringsChangedNotifier_DriveCustomizationChanged(object sender, EventArgs e)
        {
            var guy = "";
        }

        private void StringsChangedNotifier_DeviceChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
            //ImportDeviceXmlFile();
        }

        public void CreateLanguageFile(bool allLanguageFiles)
        {
            string outputFolder = @"c:\";
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (Directory.Exists(@"c:\temp"))
                    dialog.SelectedPath = @"c:\temp";
                else
                    dialog.SelectedPath = @"c:\";
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                outputFolder = dialog.SelectedPath;
            }
            if (allLanguageFiles)
            {
                foreach (var langStringCollection in LanguageStringCollections)
                {
                    byte[] bytes = languageFileGenerator.CreateLanguageFileBytes(langStringCollection, currentProject.FixedStringCount, currentProject.SoftStringCount, DeviceXmlVersion, DriveCustomizationSignature);
                    string fileName = String.Format(outputFolder + @"\language{0}.lang", langStringCollection.Index);
                    File.WriteAllBytes(fileName, bytes);
                }
            }
            else
            {
                byte[] bytes = languageFileGenerator.CreateLanguageFileBytes(SelectedLanguageStringCollection, currentProject.FixedStringCount, currentProject.SoftStringCount, DeviceXmlVersion, DriveCustomizationSignature);
                string fileName = String.Format(outputFolder + @"\language{0}.lang", SelectedLanguageStringCollection.Index);
                File.WriteAllBytes(fileName, bytes);
            }
        }
        public void ImportDeviceXmlFile()
        {
            throw new NotImplementedException();
        }
        public void ImportDeviceXmlFile(string fileLocation)
        {
            currentProject.ImportDeviceXmlFile(fileLocation, deviceStringReader, deviceXmlProvider);
            SetSelectedLanguageAndReferenceLanguage();
        }

        public void ImportDriveCustomizationXmlFile()
        {
            currentProject.ImportDriveCustomizationXmlFile(driveCustomizationStringReader, driveCustomizationXmlProvider);
            SetSelectedLanguageAndReferenceLanguage();
        }
        public void NewCustomLanguageProject()
        {
            currentProject.ClearAllTranslations();
            currentProject = new CustomLanguageProject();
            SetSelectedLanguageAndReferenceLanguage();
        }
        public void OpenCustomLanguageProject()
        {
            currentProject.Deserialize();
            SetSelectedLanguageAndReferenceLanguage();
        }
        public void SaveCustomLanguageProject()
        {
            currentProject.Serialize();
        }
        public void UpdateBaseLanguages()
        {
            foreach (var language in Languages)
            {
                ITranslation translation = LanguageStringCollections[0].Translations.Where(t => t.StringId == language.IdForLanguageString).FirstOrDefault();
                if (translation != null && !string.IsNullOrEmpty(translation.String))
                {
                    language.Name = translation.String;
                }
            }
        }
        public void Dispose()
        {
            if (stringsChangedNotifier != null)
            {
                stringsChangedNotifier.DeviceChanged -= StringsChangedNotifier_DeviceChanged;
                stringsChangedNotifier.DriveCustomizationChanged -= StringsChangedNotifier_DriveCustomizationChanged;
            }
        }
        private void SetSelectedLanguageAndReferenceLanguage()
        {
            if (currentProject.LanguageStringCollections.Count > 0)
            {
                disableEventRaising = true;
                SelectedLanguage = Languages[1];
                SelectedReferenceLanguage = Languages[0];
                disableEventRaising = false;
                if (!disableEventRaising) SourceDataChanged?.Invoke(this, new EventArgs());
            }
        }
        private void SetSelectedLanguageAndReferenceLanguage2()
        {
            if (currentProject.LanguageStringCollections.Count > 0)
            {
                disableEventRaising = true;
                SelectedLanguage = Languages[0];
                SelectedReferenceLanguage = Languages[0];
                disableEventRaising = false;
                if (!disableEventRaising) SourceDataChanged?.Invoke(this, new EventArgs());
            }
        }

    }
}
