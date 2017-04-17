using System;

namespace PlaylistManager
{
    public class PlaylistItem
    {
        public PlaylistItem(string title, string type, double duration, string path)
        {
            Title = title;
            Type = type;
            Duration = duration;
            Path = path;

            TitleDisplay = $"{Title}{Type}";

            var timespan = TimeSpan.FromSeconds(duration);
            DurationDisplay = timespan.ToString(@"mm\:ss");
        }

        public string Title { get; }

        public string Type { get; }

        public double Duration { get; }
       
        public string TitleDisplay { get; }

        public string DurationDisplay { get; }

        public string Path { get; }
    }
}
