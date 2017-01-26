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
        }

        public ICommand OpenWikiLinkCmd { get; set; }

        public ICommand ShowMoreInformationsCmd { get; set; }

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
