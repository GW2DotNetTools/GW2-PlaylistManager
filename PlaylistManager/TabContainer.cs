using System.Collections.Generic;

namespace PlaylistManager
{
    public class TabContainer
    {
        public TabContainer(string header, List<PlaylistItem> playlistEntries)
        {
            Header = header;
            PlaylistEntries = playlistEntries;
        }

        public string Header { get; }

        public List<PlaylistItem> PlaylistEntries { get; }
    }
}
