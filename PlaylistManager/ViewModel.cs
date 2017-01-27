using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace PlaylistManager
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool areInformationsVisible;

        public ViewModel()
        {
            OpenWikiLinkCmd = new ActionCommand(x => Process.Start("https://wiki.guildwars2.com/wiki/Customized_soundtrack"));
            ShowMoreInformationsCmd = new ActionCommand(x => AreInformationsVisible = true);
            Containers = new List<TabContainer>();
            Containers.Add(new TabContainer("blubb", new List<PlaylistItem> { new PlaylistItem("a", "3:45") }));
            Containers.Add(new TabContainer("blubb2", new List<PlaylistItem> { new PlaylistItem("a2", "3:45") }));
        }

        public ICommand OpenWikiLinkCmd { get; set; }

        public ICommand ShowMoreInformationsCmd { get; set; }

        public List<TabContainer> Containers { get; private set; }

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
