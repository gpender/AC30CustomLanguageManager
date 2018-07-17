using AC30CustomLanguageManager.ViewModels;
using Parker.AP.Common.CustomLanguages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AC30CustomLanguageManager.Views
{
    /// <summary>
    /// Interaction logic for DeviceXmlCW.xaml
    /// </summary>
    public partial class DeviceXmlCW : Window
    {
        DeviceXmlVM vm;

        public DeviceXmlCW()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (vm == null)
            {
                vm = DataContext as DeviceXmlVM;
                if (vm is IRequestCloseViewModel)
                {
                    ((IRequestCloseViewModel)vm).RequestClose += ProjectTemplatesCW_RequestClose;
                }
            }
        }

        private void ProjectTemplatesCW_RequestClose(object sender, RoutedEventArgs e)
        {
            if (vm != null && vm is IRequestCloseViewModel)
            {
                ((IRequestCloseViewModel)vm).RequestClose -= ProjectTemplatesCW_RequestClose;
            }
            this.DialogResult = true;
            this.Close();
        }
    }
}
