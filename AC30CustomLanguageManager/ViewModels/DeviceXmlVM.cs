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
using System.Windows;

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
    public class DeviceXmlVM : ViewModelBase, IRequestCloseViewModel
    {
        public event RoutedEventHandler RequestClose;
        ICommand importDeviceXmlCommand;

        ICollectionView availableXmlFiles;
        IDeviceXmlFile selectedXmlFile;
        ICustomLanguageManager1 customLanguageManager;
        bool showAllVersions;
        double fileLocationColumnWidth;
        public double FileLocationColumnWidth
        {
            get { return fileLocationColumnWidth; }
            set
            {
                fileLocationColumnWidth = value;
                RaisePropertyChanged(() => FileLocationColumnWidth);
            }
        }
        public IDeviceXmlFile SelectedXmlFile
        {
            get { return selectedXmlFile; }
            set
            {
                selectedXmlFile = value;
                RaisePropertyChanged(() => SelectedXmlFile);
            }
        }
        public bool ShowAllVersions
        {
            get { return showAllVersions; }
            set
            {
                showAllVersions = value;
                FileLocationColumnWidth = (showAllVersions ? 400 : 0);
                ApplyVersionFilter();
                RaisePropertyChanged(() => ShowAllVersions);
            }
        }
        public ICollectionView AvailableXmlFiles
        {
            get { return availableXmlFiles; }
        }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public DeviceXmlVM(ICustomLanguageManager1 customLanguageManager)
        {
            this.customLanguageManager = customLanguageManager;
            this.availableXmlFiles = CollectionViewSource.GetDefaultView(customLanguageManager.AvailableDeviceXmlFiles.OrderByDescending(d=>d.Version));
            AddGrouping();
            ApplyVersionFilter();
        }
        
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
            return SelectedXmlFile !=null;
        }

        private void ImportDeviceXml()
        {
            customLanguageManager.ImportDeviceXmlFile(SelectedXmlFile.FileLocation);
            if (RequestClose != null)
            {
                RequestClose(this, new RoutedEventArgs());
            }
        }
        #endregion

        private void AddGrouping()
        {
            if (availableXmlFiles.CanGroup == true)
            {
                PropertyGroupDescription groupDescription
                    = new PropertyGroupDescription("ProductType");
                availableXmlFiles.GroupDescriptions.Add(groupDescription);
                availableXmlFiles.SortDescriptions.Add(new SortDescription("Version", ListSortDirection.Descending));
            }
            else
                return;
        }
        private void ApplyVersionFilter()
        {
            List<int> tmp = new List<int>();
            
            availableXmlFiles.Filter = delegate (object item) {
                IDeviceXmlFile user = item as IDeviceXmlFile;
                if (!showAllVersions)
                {
                    if(!tmp.Contains(user.Version.Major))
                    {
                        tmp.Add(user.Version.Major);
                        return true;
                    }
                    return false;
                }
                return true;
            };
        }

    }
}