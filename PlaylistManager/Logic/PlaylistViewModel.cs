using MahApps.Metro.Controls.Dialogs;
using NAudio.Wave;
using PlaylistManager.Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PlaylistManager
{
    public class PlaylistViewModel
    {
        public delegate void MessageBoxRaiseHandler(object sender, MessageBoxRaiseEvent e);
        public event MessageBoxRaiseHandler OnMessageBoxRaise;

        private string playlistPath;
        private readonly List<string> AllowedFileTypes = new List<string> { ".aiff", ".flac", ".mp3", ".ogg", ".wav" };

        public PlaylistViewModel(string header, string infotext)
        {
            Header = header;
            InfoText = infotext;
            playlistPath = $"{PlaylistPaths.MainFolder}\\{Header}{PlaylistPaths.PlaylistEnding}";
            PlaylistEntries = new ObservableCollection<PlaylistItem>(LoadPlaylist());
            PlaylistEntries.CollectionChanged += PlaylistEntries_CollectionChanged;
        }

        public string Header { get; private set; }

        public string InfoText { get; private set; }

        public ObservableCollection<PlaylistItem> PlaylistEntries { get; private set; }

        public void DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is PlaylistItem item)
            {
                Process.Start(item.Path);
            }
        }

        public void Drop(object sender, DragEventArgs e)
        {
            object droppedData = e.Data.GetData(DataFormats.FileDrop);
            List<PlaylistItem> newlyAddedItems = new List<PlaylistItem>();
            List<string> errors = new List<string>();

            foreach (string filePath in (string[])droppedData)
            {
                string extension = Path.GetExtension(filePath);
                string fileName = Path.GetFileNameWithoutExtension(filePath);

                if (!PlaylistEntries.Any(x => x.Title == fileName)
                    && AllowedFileTypes.Any(x => x == extension))
                {
                    var duration = GetSongDurationInSeconds(filePath);
                    PlaylistItem item = new PlaylistItem(fileName, extension, duration, filePath);
                    newlyAddedItems.Add(item);
                    PlaylistEntries.Add(item);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(extension))
                    {
                        errors.Add(extension);
                    }
                }
            }

            if (errors.Count > 0)
            {
                OnMessageBoxRaise(this, new MessageBoxRaiseEvent("Ups! Something went wrong.", 
                    $"The following extensions are not supported:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}",
                    MessageDialogStyle.Affirmative));
            }

            AddToPlaylist(newlyAddedItems);
        }

        private double GetSongDurationInSeconds(string filePath)
        {
            Mp3FileReader reader = new Mp3FileReader(filePath);
            return reader.TotalTime.TotalSeconds;
        }

        private void PlaylistEntries_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                File.Delete(playlistPath);
                AddToPlaylist(PlaylistEntries.ToList());
            }
        }

        private void AddToPlaylist(List<PlaylistItem> newlyAddedItems)
        {
            CreatePlaylist();

            List<string> entries = new List<string>();
            foreach (var item in newlyAddedItems)
            {
                string fileName = Path.GetFileNameWithoutExtension(item.Path);

                entries.Add($"#EXTINF:{Math.Floor(item.Duration)},{fileName}");
                entries.Add(item.Path);
            }

            File.AppendAllLines(playlistPath, entries);
        }

        private List<PlaylistItem> LoadPlaylist()
        {
            List<PlaylistItem> items = new List<PlaylistItem>();
            if (File.Exists(playlistPath))
            {
                bool skipHeader = true;
                int count = 0;
                foreach (var item in File.ReadAllLines(playlistPath))
                {
                    if (skipHeader)
                    {
                        skipHeader = false;
                        continue;
                    }

                    if (count == 0)
                    {
                        count++;
                        continue;
                    }

                    items.Add(new PlaylistItem(Path.GetFileNameWithoutExtension(item), Path.GetExtension(item), GetSongDurationInSeconds(item), item));
                    count = 0;
                }
            }

            return items;
        }

        private void CreatePlaylist()
        {
            if (!File.Exists(playlistPath))
            {
                File.AppendAllText(playlistPath, $"#EXTM3U{Environment.NewLine}"); // Writes the File header
            }
        }
    }
}




