using System.Collections.Generic;

namespace PlaylistManager
{
    public class PlaylistViewModel
    {
        public PlaylistViewModel(string header)
        {
            Header = header;
            PlaylistEntries = new List<PlaylistItem>();
        }

        public string Header { get; private set; }

        public List<PlaylistItem> PlaylistEntries { get; private set; }
    }
}