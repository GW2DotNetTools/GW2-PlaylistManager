using MahApps.Metro.Controls;
using System.Reflection;

namespace PlaylistManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class View : MetroWindow
    {
        public View(ViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Title += $" - {version.Major}.{version.Minor}.{version.Build}";
        }
    }
}