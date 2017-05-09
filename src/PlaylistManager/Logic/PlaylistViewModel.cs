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
using System.Text.RegularExpressions;
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
                string error = string.Empty;

                if (IsVaildEntry(extension, filePath, fileName, out error))
                {
                    var duration = GetSongDurationInSeconds(filePath);
                    PlaylistItem item = new PlaylistItem(fileName, extension, duration, filePath);
                    newlyAddedItems.Add(item);
                    PlaylistEntries.Add(item);
                }

                if (!string.IsNullOrWhiteSpace(error))
                {
                    errors.Add(error);
                }
            }

            if (errors.Count > 0)
            {
                OnMessageBoxRaise(this, new MessageBoxRaiseEvent("Ups! Something went wrong.", 
                    $"{string.Join(Environment.NewLine, errors)}",
                    MessageDialogStyle.Affirmative));
            }

            AddToPlaylist(newlyAddedItems);
        }

        private bool IsVaildEntry(string extension, string filePath, string fileName, out string error)
        {
            error = string.Empty;
            if (!AllowedFileTypes.Any(x => x == extension))
            {
                error = $"The extension '{extension}' is not supported.";
                return false;
            }

            if (fileName.ToArray().Any(x => x > 175)) // Everything after 175 is not relevant for us
            {
                error = $"The file '{fileName}{extension}' contains invalid characters.";
                return false;
            }

            return !PlaylistEntries.Any(x => x.Title == fileName);
        }

        private double GetSongDurationInSeconds(string filePath)
        {
            Mp3FileReader reader = new Mp3FileReader(filePath);
            return reader.TotalTime.TotalSeconds;
        }

        private void PlaylistEntries_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Reset)
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
                int currentLine = -1;
                List<string> errors = new List<string>();
                List<int> linesToRemove = new List<int>();
                foreach (var item in File.ReadAllLines(playlistPath))
                {
                    currentLine++;
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

                    try
                    {
                        items.Add(new PlaylistItem(Path.GetFileNameWithoutExtension(item), Path.GetExtension(item), GetSongDurationInSeconds(item), item));
                    }
                    catch (FileNotFoundException)
                    {
                        linesToRemove.Add(currentLine -1); // We also have to remove the header of the missing file.
                        linesToRemove.Add(currentLine);
                        errors.Add(item);
                    }

                    count = 0;
                }

                if (errors.Count > 0)
                {
                    string oneOrMoreMissingFiles = errors.Count > 1 ? "file was" : "files were";
                    OnMessageBoxRaise(this, new MessageBoxRaiseEvent($"The following {oneOrMoreMissingFiles} not found:",
                        $"{string.Join(Environment.NewLine, errors)}",
                        MessageDialogStyle.Affirmative));
                    RemoveMissingFiles(linesToRemove.OrderByDescending(x => x).ToList());
                }
            }

            return items;
        }

        private void RemoveMissingFiles(List<int> linesToRemove)
        {
            var allLines = File.ReadAllLines(playlistPath).ToList();

            for (int i = 0; i < linesToRemove.Count; i++)
            {
                allLines.RemoveAt(linesToRemove[i]);
            }

            File.WriteAllLines(playlistPath, allLines);
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