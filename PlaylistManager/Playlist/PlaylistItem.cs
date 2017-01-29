namespace PlaylistManager
{
    public class PlaylistItem
    {
        public PlaylistItem(string title, string type)
        {
            Title = title;
            Type = type;
        }

        public string Title { get; }

        public string Type { get; }
    }
}
