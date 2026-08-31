using EBToolbox.Utils;
using EBToolbox.ViewModels;
using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;

namespace EBToolbox.Views
{
    public sealed partial class SettingsPage : Page
    {
        public bool KeepBackground_State = RegistryHelper.IsMatch("HKLM\\SOFTWARE\\EBOS\\Toolbox", "KeepInBackground", 1);

        public string Version
        {
            get
            {
                return App.Version;
            }
        }

        public SettingsPage()
        {
            this.InitializeComponent();
            this.DataContext = new SettingsPageViewModel();
            LoadText();
            ConfigSwitch.Loaded += (s, e) =>
            {
                ConfigSwitch.SelectionChanged += ConfigSwitch_SelectionChanged;
            };

            var savedTheme = ThemeHelper.GetSavedTheme();
            ThemeComboBox.SelectedIndex = ThemeHelper.ThemeToIndex(savedTheme);
            _isLoadingTheme = false;
            ThemeComboBox.SelectionChanged += ThemeComboBox_SelectionChanged;
        }

        public void LoadText()
        {
            // Default text loading
            TitleTxt.Text = App.GetValueFromItemList("Settings");
            AppearanceHeader.Text = "Appearance";
            ThemeDescription.Header = "Theme";
            BehaviorHeader.Text = App.GetValueFromItemList("Behavior");
            BackgroundDescription.Header = App.GetValueFromItemList("Settings_BackgroundDesc");
            AboutHeader.Text = App.GetValueFromItemList("About");
            toCloneRepoCard.Header = App.GetValueFromItemList("CloneRepoCard");
            bugRequestCard.Header = App.GetValueFromItemList("BugReportCard");
            WarningHeader.Header = App.GetValueFromItemList("WarningHeader");
            LanguageHeader.Header = App.GetValueFromItemList("Language");
            Update.Header = App.GetValueFromItemList("CheckUpdates");
            CheckUpdateButton.Content = App.GetValueFromItemList("CheckUpdatesBtn");
            NoUpdatesBar.Text = App.GetValueFromItemList("LatestVer");
            SystemInfo.Header = App.GetValueFromItemList("SystemInfo");
            WinVer.Text = App.GetValueFromItemList("Home_WinVer") + ": " + RegistryHelper.GetValue("HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "DisplayVersion").ToString();
            EBVer.Text = App.GetValueFromItemList("Home_PlaybookVer") + ": " + RegistryHelper.GetValue("HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "RegisteredOrganization").ToString();

            // Experiments
            ExperimentalHeader.Text = App.GetValueFromItemList("ExperimentsHeader");
            ExperimentsExpander.Header = App.GetValueFromItemList("ExperimentsCardHeader");
            ExperimentsExpander.Description = App.GetValueFromItemList("ExperimentsCardDescription");

            ///// Search Experiment
            //SearchExpCard.Header = App.GetValueFromItemList("SearchExperiment");
            //SearchExpCard.Description = App.GetValueFromItemList("SearchExperimentDescription");
        }

        private void KeepBackground_Toggled(object sender, RoutedEventArgs e)
        {
            SettingsBehaviorHelper.KeppBackground_Toggled(sender, e);
        }

        private bool _isLoadingTheme = true;

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isLoadingTheme) return;
            if (sender is ComboBox comboBox && comboBox.SelectedIndex >= 0)
            {
                var theme = ThemeHelper.IndexToTheme(comboBox.SelectedIndex);
                ThemeHelper.SetTheme(theme);
                App.ContentDialogCaller("restartApp");
            }
        }

        private void toCloneRepoCard_Click(object sender, RoutedEventArgs e)
        {
            DataPackage package = new DataPackage();
            package.SetText(gitCloneTextBlock.Text);
            Clipboard.SetContent(package);
        }

        private async void bugRequestCard_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://github.com/EhabYT/EB-Toolbox/issues/new?template=bug-report.md"));
        }

        private void ConfigSwitch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App.ContentDialogCaller("restartApp");
        }

        private async void CheckUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsPageViewModel vm = this.DataContext as SettingsPageViewModel;
            NoUpdatesBar.Visibility = Visibility.Collapsed;
            ProgressRing.Visibility = Visibility.Visible;
            bool update = await Task.Run(() => vm.CheckUpdates());
            if (update)
            {
                ProgressRing.Visibility = Visibility.Collapsed;
                NoUpdatesBar.Visibility = Visibility.Visible;
            }
        }
        #region experiments
        private void IsExperimentEnabled(object sender, RoutedEventArgs e)
        {
            var s = sender as ToggleSwitch;
            s.IsOn = RegistryHelper.IsMatch(@$"HKLM\SOFTWARE\EBOS\Toolbox\Experiments\{s.Tag.ToString()}", "enabled", 1);
            s.Toggled += ToggleState;
        }

        private void ToggleState(object sender, RoutedEventArgs e)
        {
            var s = sender as ToggleSwitch;
            RegistryHelper.SetValue(@$"HKLM\SOFTWARE\EBOS\Toolbox\Experiments\{s.Tag.ToString()}", "enabled", s.IsOn, Microsoft.Win32.RegistryValueKind.DWord);
            App.ContentDialogCaller("restartApp");
        }
        #endregion experiments

    }
}