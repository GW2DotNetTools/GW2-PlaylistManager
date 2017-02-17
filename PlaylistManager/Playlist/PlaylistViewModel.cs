using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace PlaylistManager
{
    public class PlaylistViewModel
    {
        private readonly List<string> AllowedFileTypes = new List<string>
        {
            ".aiff",
            ".flac",
            ".mp3",
            ".ogg",
            ".wav"
        };

        public PlaylistViewModel(string header, string infotext)
        {
            Header = header;
            InfoText = infotext;
            PlaylistEntries = new ObservableCollection<PlaylistItem>();
        }

        public string Header { get; private set; }

        public string InfoText { get; private set; }

        public ObservableCollection<PlaylistItem> PlaylistEntries { get; private set; }

        public void Drop(object sender, DragEventArgs e)
        {
            object droppedData = e.Data.GetData(DataFormats.FileDrop);

            foreach (string filePath in (string[])droppedData)
            {
                string extension = Path.GetExtension(filePath);
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                if (!PlaylistEntries.Any(x => x.Title == fileName) 
                    && AllowedFileTypes.Any(x => x == extension))
                {
                    PlaylistEntries.Add(new PlaylistItem(fileName, extension));
                }
            }

            CreateOrEditPlaylist();
        }

        private void CreateOrEditPlaylist()
        {
            string playlistName = $"{PlaylistPaths.MainFolder}\\{Header}{PlaylistPaths.PlaylistEnding}";

            if (!File.Exists(playlistName))
            {
                File.WriteAllText(playlistName, "#EXTM3U"); // Writes the File header
            }

            /*
             * #EXTM3U  File header. Must be the first line of the file.
             * #EXTINF	Track information, including runtime and title.
             * 
            #EXTM3U
            #EXTINF:%DURATIONSINSECONDS%,%TITLE%
            %FILEPATH%
            */
        }
    }
}
 
 
 
 
 