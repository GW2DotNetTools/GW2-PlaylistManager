using System;

namespace PlaylistManager
{
    public static class PlaylistPaths
    {
        public static string MainFolder { get { return $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\Guild Wars 2\\Music"; } }

        public static string PlaylistEnding { get { return ".m3u"; } }
    }
}
