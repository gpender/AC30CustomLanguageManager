using AC30CustomLanguageInterfaces.Interfaces;
using System.Collections.Generic;
using System.Globalization;
using System;
using System.Linq;
using Microsoft.Win32;
using System.Xml.Serialization;
using System.IO;

namespace AC30CustomLanguageManagerApp.Model
{
    public class CustomLanguageManager : ICustomLanguageManager
    {
        ICustomLanguageProject currentProject;
        ILanguageFileGenerator languageFileGenerator;
        ILanguageStringCollection selectedLanguageStringCollection;
        ILanguageStringCollection selectedReferenceStringCollection;
        IDeviceStringReader deviceStringReader;
        IDeviceXmlProvider deviceXmlProvider;
        IDriveCustomizationStringReader driveCustomizationStringReader;
        IDriveCustomizationXmlProvider driveCustomizationXmlProvider;
        //ICustomLanguageProject currentProject;
        public uint DriveCustomizationSignature
        {
            get { return currentProject.DriveCustomizationSignature; }
        }
        public Version DeviceXmlVersion
        {
            get { return currentProject.DeviceXmlVersion; }
        }
        public ILanguageStringCollection SelectedLanguageStringCollection
        {
            get { return selectedLanguageStringCollection; }
            set { selectedLanguageStringCollection = value; }
        }
        public ILanguageStringCollection SelectedReferenceStringCollection
        {
            get { return selectedReferenceStringCollection; }
            set
            {
                selectedReferenceStringCollection = value;
                IReferenceStringProvider referenceStringProvider = new ReferenceStringProvider(selectedReferenceStringCollection);
                currentProject.SetReferenceStringProvider(referenceStringProvider);
            }
        }
        public List<ILanguageStringCollection> LanguageStringCollections
        {
            get { return currentProject.LanguageStringCollections; }
        }

        public CustomLanguageManager(IDeviceStringReader deviceStringReader, IDeviceXmlProvider deviceXmlProvider, IDriveCustomizationStringReader driveCustomizationStringReader, IDriveCustomizationXmlProvider driveCustomizationXmlProvider, ILanguageFileGenerator languageFileGenerator)
        {
            this.deviceStringReader = deviceStringReader;
            this.deviceXmlProvider = deviceXmlProvider;
            this.driveCustomizationStringReader = driveCustomizationStringReader;
            this.driveCustomizationXmlProvider = driveCustomizationXmlProvider;
            this.languageFileGenerator = languageFileGenerator;
            currentProject = new CustomLanguageProject();
            SetSelectedLanguageStringCollection();
        }

        public void CreateLanguageFile(bool allLanguageFiles)
        {
            if (allLanguageFiles)
            {
                foreach (var langStringCollection in LanguageStringCollections)
                {
                    byte[] bytes = languageFileGenerator.CreateLanguageFileBytes(langStringCollection, currentProject.FixedStringCount, currentProject.SoftStringCount, DeviceXmlVersion, DriveCustomizationSignature);
                    string fileName = String.Format(@"c:\temp\language{0}.lang", langStringCollection.Index);
                    File.WriteAllBytes(fileName, bytes);
                }
            }
            else
            {
                byte[] bytes = languageFileGenerator.CreateLanguageFileBytes(SelectedLanguageStringCollection, currentProject.FixedStringCount, currentProject.SoftStringCount, DeviceXmlVersion, DriveCustomizationSignature);
                string fileName = String.Format(@"c:\temp\language{0}.lang", SelectedLanguageStringCollection.Index);
                File.WriteAllBytes(fileName, bytes);
            }
        }
        public void ImportDeviceXmlFile()
        {
            currentProject.ImportDeviceXmlFile(deviceStringReader, deviceXmlProvider);
            SetSelectedLanguageStringCollection();
        }

        public void ImportDriveCustomizationXmlFile()
        {
            currentProject.ImportDriveCustomizationXmlFile(driveCustomizationStringReader, driveCustomizationXmlProvider);
            SetSelectedLanguageStringCollection();
        }
        public void NewCustomLanguageProject()
        {
            currentProject.ClearAllTranslations();
            currentProject = new CustomLanguageProject();
            SetSelectedLanguageStringCollection();
        }
        public void OpenCustomLanguageProject()
        {
            currentProject.Deserialize();
            SetSelectedLanguageStringCollection();
        }
        public void SaveCustomLanguageProject()
        {
            currentProject.Serialize();
        }

        private void SetSelectedLanguageStringCollection()
        {
            if (currentProject.LanguageStringCollections.Count > 0)
            {
                SelectedLanguageStringCollection = currentProject.LanguageStringCollections[1];
                SelectedReferenceStringCollection = currentProject.LanguageStringCollections[0];// selectedLanguageStringCollection;
            }
        }
    }
}
