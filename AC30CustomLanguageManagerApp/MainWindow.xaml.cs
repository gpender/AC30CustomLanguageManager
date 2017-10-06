using System.Windows;

namespace AC30CustomLanguageManagerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                this.Title = "AC30 Custom Language Manager(Desktop App)  "  + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            catch { }
        }
    }
}
