using PlaylistManager.Logic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace PlaylistManager
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public delegate void MessageBoxRaiseHandler(object sender, MessageBoxRaiseEvent e);
        public event MessageBoxRaiseHandler OnMessageBoxRaise;

        private bool areInformationsVisible;
        private int selectedContainer;

        public ViewModel(List<TabContainer> containers)
        {
            Containers = new ObservableCollection<TabContainer>(containers);
            OpenWikiLinkCmd = new ActionCommand(x => Process.Start("https://wiki.guildwars2.com/wiki/Customized_soundtrack"));
            ShowMoreInformationsCmd = new ActionCommand(x => AreInformationsVisible = true);
            Directory.CreateDirectory(PlaylistPaths.MainFolder);

            foreach (var container in containers)
            {
                container.PlaylistViewModel.OnMessageBoxRaise += (o, e) => OnMessageBoxRaise(o, e);
            }
        }

        public ICommand OpenWikiLinkCmd { get; set; }

        public ICommand ShowMoreInformationsCmd { get; set; }

        public ObservableCollection<TabContainer> Containers { get; private set; }

        public int SelectedContainer
        {
            get { return selectedContainer; }
            set
            {
                selectedContainer = value;
                NotifyPropertyChanged(nameof(SelectedContainer));
            }
        }

        public bool AreInformationsVisible
        {
            get { return areInformationsVisible; }
            set
            {
                areInformationsVisible = value;
                NotifyPropertyChanged(nameof(AreInformationsVisible));
            }
        }

        protected void NotifyPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
