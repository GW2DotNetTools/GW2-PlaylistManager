using System.Windows;

namespace PlaylistManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ViewModel viewModel = new ViewModel();
            View view = new View(viewModel);
            view.Show();
        }
    }
}
