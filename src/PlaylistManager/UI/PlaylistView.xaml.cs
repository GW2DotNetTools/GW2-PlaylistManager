using System.Windows.Controls;

namespace PlaylistManager.UI
{
    /// <summary>
    /// Interaction logic for Playlist.xaml
    /// </summary>
    public partial class PlaylistView : UserControl
    {
        private PlaylistViewModel viewModel;

        public PlaylistView(PlaylistViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            this.viewModel = viewModel;
        }

        private void DataGrid_Drop(object sender, System.Windows.DragEventArgs e)
        {
            viewModel.Drop(sender, e);
        }

        private void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            viewModel.DoubleClick(datagrid.SelectedItem, e);
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            viewModel.OnLoad();
        }
    }
}