/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:AC30CustomLanguageManagerApp"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using Parker.AP.Common.CustomLanguages;
using AC30CustomLanguageManager.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace AC30CustomLanguageManager.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<ILanguageFileGenerator, LanguageFileGenerator>();
                SimpleIoc.Default.Register<IDeviceStringReader, DeviceStringReader>();
                //SimpleIoc.Default.Register<IDeviceXmlProvider, CodesysDeviceXmlProvider>();
                SimpleIoc.Default.Register<IDriveCustomizationStringReader, DriveCustomizationStringReader>();
                SimpleIoc.Default.Register<IDriveCustomizationXmlProvider, DriveCustomizationBrowserXmlProvider>();
                SimpleIoc.Default.Register<IDeviceXmlProvider, TestDeviceXmlProvider>();
                SimpleIoc.Default.Register<ICustomLanguageProject, CustomLanguageProject>();
                SimpleIoc.Default.Register<ICustomLanguageManager, CustomLanguageManager>();
            }
            else
            {
                // Create run time view services and models
                SimpleIoc.Default.Register<ILanguageFileGenerator, LanguageFileGenerator>();
                SimpleIoc.Default.Register<IDeviceStringReader, DeviceStringReader>();
                SimpleIoc.Default.Register<IDriveCustomizationStringReader, DriveCustomizationStringReader>();
                SimpleIoc.Default.Register<IDriveCustomizationXmlProvider, DriveCustomizationBrowserXmlProvider>();
                SimpleIoc.Default.Register<ICustomLanguageProject, CustomLanguageProject>();
                SimpleIoc.Default.Register<ICustomLanguageManager, CustomLanguageManager>();

                SimpleIoc.Default.Register<IStringsChangedNotifier, StringsChangedNotifier>();
                SimpleIoc.Default.Register<IDeviceXmlProvider, CodesysDeviceBrowserXmlProvider>();
                //SimpleIoc.Default.Register<IDeviceXmlProvider, TestDeviceXmlProvider>();
            }

            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}