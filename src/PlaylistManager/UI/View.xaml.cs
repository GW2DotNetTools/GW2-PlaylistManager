using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using PlaylistManager.Logic;
using System.Reflection;

namespace PlaylistManager.UI
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
            viewModel.OnMessageBoxRaise += ViewModel_OnMessageBoxRaiseAsync;

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Title += $" - {version.Major}.{version.Minor}.{version.Build}";
        }

        private async void ViewModel_OnMessageBoxRaiseAsync(object sender, MessageBoxRaiseEvent e)
        {
            await this.ShowMessageAsync(e.Title, e.Message, e.DialogStyle);
        }
    }
}