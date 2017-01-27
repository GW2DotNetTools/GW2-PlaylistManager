using System.Collections.Generic;
using System.Windows;

namespace PlaylistManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var ambientViewModel = new PlaylistViewModel("Default playlist when no other criteria are met");
            var battleViewModel = new PlaylistViewModel("\"Large\" fights involving many foes (~5 or more enemies); some personal story steps; PvP");
            var bossBattleViewModel = new PlaylistViewModel("World bosses and some Dungeon bosses; some personal story steps; some Activities");
            var cityViewModel = new PlaylistViewModel("When inside one of the main cities, such as The Black Citadel, but not always");
            var defeatedViewModel = new PlaylistViewModel("Upon being Defeated");
            var mainMenuViewModel = new PlaylistViewModel("At the character select screen");
            var nightTimeViewModel = new PlaylistViewModel("When in some explorable areas and it is nighttime (the moon is out)");
            var underwaterViewModel = new PlaylistViewModel("Any time the breathing apparatus is equipped. Overrides most other playlists");
            var victoryViewModel = new PlaylistViewModel("Music that plays after World bosses or Meta events");

            List<TabContainer> containers = new List<TabContainer>
            {
                new TabContainer("Ambient", new PlaylistView(ambientViewModel), ambientViewModel),
                new TabContainer("Battle", new PlaylistView(battleViewModel), battleViewModel),
                new TabContainer("BossBattle", new PlaylistView(bossBattleViewModel), bossBattleViewModel),
                new TabContainer("City", new PlaylistView(cityViewModel), cityViewModel),
                new TabContainer("Defeated", new PlaylistView(defeatedViewModel), defeatedViewModel),
                new TabContainer("MainMenu", new PlaylistView(mainMenuViewModel), mainMenuViewModel),
                new TabContainer("NightTime", new PlaylistView(nightTimeViewModel), nightTimeViewModel),
                new TabContainer("Underwater", new PlaylistView(underwaterViewModel), underwaterViewModel),
                new TabContainer("Victory", new PlaylistView(victoryViewModel), victoryViewModel)
            };

            ViewModel viewModel = new ViewModel(containers);
            View view = new View(viewModel);
            view.Show();
        }
    }
}
