using System.Windows.Controls;

namespace PlaylistManager
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
    }
}