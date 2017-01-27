namespace PlaylistManager
{
    public class PlaylistItem
    {
        public PlaylistItem(string title, string duration)
        {
            Title = title;
            Duration = duration;
        }

        public string Title { get; }

        public string Duration { get; }
    }
}
