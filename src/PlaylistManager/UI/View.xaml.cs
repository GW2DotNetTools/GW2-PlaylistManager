using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using PlaylistManager.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PlaylistManager.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class View : MetroWindow
    {
        public View(ViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            viewModel.OnMessageBoxRaise += ViewModel_OnMessageBoxRaiseAsync;

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Title += $" - {version.Major}.{version.Minor}.{version.Build}";

            LoadThemeIfSet();
        }

        private async void ViewModel_OnMessageBoxRaiseAsync(object sender, MessageBoxRaiseEvent e)
        {
            await this.ShowMessageAsync(e.Title, e.Message, e.DialogStyle);
        }

        #region ThemePicker
        private void AccentSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AccentSelector.SelectedItem is Accent)
            {
                Accent selectedAccent = (Accent)AccentSelector.SelectedItem;
                if (selectedAccent != null)
                {
                    var theme = ThemeManager.DetectAppStyle(Application.Current);
                    ThemeManager.ChangeAppStyle(Application.Current, selectedAccent, theme.Item1);
                    Application.Current.MainWindow.Activate();
                }
            }
        }

        private void LoadThemeIfSet()
        {
            //todo
        }
        #endregion
    }
}