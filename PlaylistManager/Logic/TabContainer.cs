using PlaylistManager.UI;

namespace PlaylistManager
{
    public class TabContainer
    {
        public TabContainer(string header, PlaylistView view, PlaylistViewModel playlistViewModel)
        {
            Header = header;
            View = view;
            PlaylistViewModel = playlistViewModel;
        }

        public string Header { get; private set; }

        public PlaylistView View { get; private set; }

        public PlaylistViewModel PlaylistViewModel { get; private set; }
    }
}
