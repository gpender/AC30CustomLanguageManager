using GalaSoft.MvvmLight;
//using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Collections.ObjectModel;
using Telerik.Windows.Data;
using System.ComponentModel;
using System.Windows.Data;
using GalaSoft.MvvmLight.Command;
using Parker.AP.Common.CustomLanguages;
using AC30CustomLanguageManager.Views;

namespace AC30CustomLanguageManager.ViewModels
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    /// 
    public class MainViewModel : ViewModelBase
    {
        ICommand deleteTranslationCommand;
        ICommand importDeviceXmlCommand;
        ICommand importDriveCustomizationXmlCommand;
        RelayCommand<string> createLanguageFileCommand;
        ICommand newLanguageStringProjectCommand;
        ICommand openLanguageStringProjectCommand;
        ICommand saveLanguageStringProjectCommand;

        ICustomLanguageManager1 customLanguageManager;
        bool? stringTypeFilter;
        ObservableCollection<ITranslation> selectedItems = new ObservableCollection<ITranslation>();
        public string CompilerWarning
        {
            get { return customLanguageManager.CompilerWarning; }
        }
        public Version DeviceXmlVersion
        {
            get { return customLanguageManager.DeviceXmlVersion; }
        }
        public uint DriveCustomizationSignature
        {
            get { return customLanguageManager.DriveCustomizationSignature; }
        }
        public uint DriveCustomizationSignaturePre3570
        {
            get { return customLanguageManager.DriveCustomizationSignaturePre3570; }
        }
        public bool EditingLanguages
        {
            get { return customLanguageManager.EditingLanguages; }
            set
            {
                customLanguageManager.EditingLanguages = value;
                RaisePropertyChanged(() => EditingLanguages);
                stringTypeFilter = null;
                RaisePropertyChanged(() => StringTypeFilter);
                SetFilter();
            }
        }
        public List<ILanguage> Languages
        {
            get { return customLanguageManager.Languages; }
        }
        public ILanguage SelectedLanguage
        {
            get { return customLanguageManager.SelectedLanguage; }
            set
            {
                customLanguageManager.SelectedLanguage = value;
                RaisePropertyChanged(() => SelectedLanguage);
            }
        }
        public ILanguage SelectedReferenceLanguage
        {
            get { return customLanguageManager.SelectedReferenceLanguage; }
            set
            {
                customLanguageManager.SelectedReferenceLanguage = value;
                RaisePropertyChanged(() => SelectedReferenceLanguage);
            }
        }
        public ObservableCollection<ITranslation> SelectedItems
        {
            get{ return selectedItems; }
        }
        public ICollectionView SelectedTranslationsView
        {
            get { return customLanguageManager.SelectedLanguageStringCollection!=null ? CollectionViewSource.GetDefaultView(customLanguageManager.SelectedLanguageStringCollection.Translations):null;}
        }
        public bool ShowStringFilterButton
        {
            get { return DriveCustomizationSignature > 0 && !DeviceXmlVersion.Equals(new Version(0,0,0,0)); }
        }
        public bool? StringTypeFilter
        {
            get { return stringTypeFilter; }
            set
            {
                Set<bool?>(() => StringTypeFilter, ref stringTypeFilter, value);
                customLanguageManager.EditingLanguages = false;
                RaisePropertyChanged(() => EditingLanguages);
                SetFilter();
            }
        }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(ICustomLanguageManager customLanguageManager)
        {
            if (IsInDesignMode)
            {
                this.customLanguageManager = customLanguageManager as ICustomLanguageManager1;
            }
            else
            {
                this.customLanguageManager = customLanguageManager as ICustomLanguageManager1;
                this.customLanguageManager.SourceDataChanged += CustomLanguageManager_SourceDataChanged;
            }
        }

        private void CustomLanguageManager_SourceDataChanged(object sender, EventArgs e)
        {
            RefreshDropDownsAndTranslations();
        }

        #region Commands

        #region DeleteTranslationCommand

        public ICommand DeleteTranslationCommand
        {
            get
            {
                if (deleteTranslationCommand == null)
                {
                    deleteTranslationCommand = new RelayCommand(DeleteTranslation, CanDeleteTranslation);
                }
                return deleteTranslationCommand;
            }
        }
        private bool CanDeleteTranslation()
        {
            return true;
        }
        private void DeleteTranslation()
        {
            foreach(var t in SelectedItems)
            {
                t.String = string.Empty;
            }
        }
        #endregion

        #region ImportDeviceXmlCommand

        public ICommand ImportDeviceXmlCommand
        {
            get
            {
                if (importDeviceXmlCommand == null)
                {
                    importDeviceXmlCommand = new RelayCommand(ImportDeviceXml, CanImportDeviceXml);
                }
                return importDeviceXmlCommand;
            }
        }
        private bool CanImportDeviceXml()
        {
            return true;
        }

        private void ImportDeviceXml()
        {
            EditingLanguages = false;
            DeviceXmlCW cw = new DeviceXmlCW();
            DeviceXmlVM vm = new DeviceXmlVM(customLanguageManager);
            cw.DataContext = vm;
            cw.ShowDialog();
        }
        #endregion

        #region ImportDriveCustomizationXmlCommand

        public ICommand ImportDriveCustomizationXmlCommand
        {
            get
            {
                if (importDriveCustomizationXmlCommand == null)
                {
                    importDriveCustomizationXmlCommand = new RelayCommand(ImportDriveCustomizationXml, CanImportDriveCustomizationXml);
                }
                return importDriveCustomizationXmlCommand;
            }
        }
        private bool CanImportDriveCustomizationXml()
        {
            return true;
        }

        private void ImportDriveCustomizationXml()
        {
            EditingLanguages = false;
            customLanguageManager.ImportDriveCustomizationXmlFile();
        }
        #endregion

        #region CreateLanguageFileCommand

        public ICommand CreateLanguageFileCommand
        {
            get
            {
                if (createLanguageFileCommand == null)
                {
                    createLanguageFileCommand = new RelayCommand<string>((createAllLanguageFiles) => CreateLanguageFile(createAllLanguageFiles));
                }
                return createLanguageFileCommand;
            }
        }
        private bool CanCreateLanguageFile()
        {
            return true;
        }

        private void CreateLanguageFile(string createAllLanguageFiles)
        {
            customLanguageManager.CreateLanguageFile(!string.IsNullOrEmpty(createAllLanguageFiles));
        }
        #endregion

        #region NewLanguageStringProjectCommand

        public ICommand NewLanguageStringProjectCommand
        {
            get
            {
                if (newLanguageStringProjectCommand == null)
                {
                    newLanguageStringProjectCommand = new RelayCommand(NewLanguageStringProject, CanNewLanguageStringProject);
                }
                return newLanguageStringProjectCommand;
            }
        }
        private bool CanNewLanguageStringProject()
        {
            return true;
        }

        private void NewLanguageStringProject()
        {
            EditingLanguages = false;
            customLanguageManager.NewCustomLanguageProject();
        }

        #endregion

        #region OpenLanguageStringProjectCommand

        public ICommand OpenLanguageStringProjectCommand
        {
            get
            {
                if (openLanguageStringProjectCommand == null)
                {
                    openLanguageStringProjectCommand = new RelayCommand(OpenLanguageStringProject, CanOpenLanguageStringProject);
                }
                return openLanguageStringProjectCommand;
            }
        }
        private bool CanOpenLanguageStringProject()
        {
            return true;
        }

        private void OpenLanguageStringProject()
        {
            EditingLanguages = false;
            customLanguageManager.OpenCustomLanguageProject();
        }
        #endregion

        #region SaveLanguageStringProjectCommand

        public ICommand SaveLanguageStringProjectCommand
        {
            get
            {
                if (saveLanguageStringProjectCommand == null)
                {
                    saveLanguageStringProjectCommand = new RelayCommand(SaveLanguageStringProject, CanSaveLanguageStringProject);
                }
                return saveLanguageStringProjectCommand;
            }
        }
        private bool CanSaveLanguageStringProject()
        {
            return true;
        }

        private void SaveLanguageStringProject()
        {
            customLanguageManager.SaveCustomLanguageProject();
        }

        #endregion

        #endregion

        private void RefreshDropDownsAndTranslations()
        {
            RaisePropertyChanged(() => SelectedLanguage);
            RaisePropertyChanged(() => SelectedReferenceLanguage);
            RaisePropertyChanged(() => Languages);
            SelectedTranslationsView.Refresh();
            RaisePropertyChanged(() => SelectedTranslationsView);
            RaisePropertyChanged(() => DeviceXmlVersion);
            RaisePropertyChanged(() => DriveCustomizationSignature);
            RaisePropertyChanged(() => DriveCustomizationSignaturePre3570);
            RaisePropertyChanged(() => CompilerWarning);
            RaisePropertyChanged(() => ShowStringFilterButton);
            SetFilter();
        }
        private void SetFilter()
        {
            if (SelectedTranslationsView != null)
            {
                if (EditingLanguages)
                {
                    SelectedTranslationsView.Filter = new Predicate<object>(FilterParameters);
                }
                else
                {
                    SelectedTranslationsView.Filter = new Predicate<object>(FilterParameterByType);
                }
            }
        }

        private bool FilterParameters(object translation)
        {
            ITranslation t = translation as ITranslation;
            if (t != null)
            {
                List<int> langNameIds = new List<int>() { 848, 849, 850, 851, 852, 1399, 1400, 1401, 1402, 1697 };
                return langNameIds.Contains(t.StringId);
            }
            return true;
        }
        private bool FilterParameterByType(object translation)
        {
            ITranslation t = translation as ITranslation;
            if (t != null)
            {
                if (StringTypeFilter == null) return true;
                return t.IsSoftString == StringTypeFilter.Value;
            }
            return true;
        }
    }
}