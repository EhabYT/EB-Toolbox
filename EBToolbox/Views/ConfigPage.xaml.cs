using System;
using System.Collections.ObjectModel;
using System.Linq;
using EBToolbox.Enums;
using EBToolbox.Utils;
using EBToolbox.ViewModels;
using CommunityToolkit.WinUI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using NLog.Filters;

namespace EBToolbox.Views;

public sealed partial class ConfigPage : Page
{
    private readonly ConfigPageViewModel _viewModel;

    public ConfigPage()
    {
        this.InitializeComponent();

        _viewModel = App._host.Services.GetRequiredService<ConfigPageViewModel>();
        // Gets all the items for the choosen category
        Enum.TryParse(App.CurrentCategory, out ConfigurationType configType);
        _viewModel.ShowForType(configType);

        this.DataContext = _viewModel;

        BreadcrumbBar.ItemsSource = new ObservableCollection<Folder> {
            new Folder {Name = configType.GetDescription()}
        };
        BreadcrumbBar.ItemClicked += BreadcrumbBar_ItemClicked;

        this.Loaded += ConfigPage_Loaded;
    }

        private void ConfigPage_Loaded(object sender, RoutedEventArgs e)
        {
            SearchHighlightHelper.RunPendingHighlightAsync(FindConfigurationItem, ConfigScrollViewer);
        }

        private (ItemsControl itemsControl, int index)? FindConfigurationItem(string itemKey)
        {
            for (int i = 0; i < _viewModel.ConfigurationItems.Count; i++)
            {
                if (_viewModel.ConfigurationItems[i].Key == itemKey) return (ConfigItemsControl, i);
            }
            return null;
        }

        private void BreadcrumbBar_ItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
        {
            var items = BreadcrumbBar.ItemsSource as ObservableCollection<Folder>;
            if (items is null) return;
            for (int i = items.Count - 1; i >= args.Index + 1; i--)
            {
                items.RemoveAt(i);
            }
        }

        private void OnCardClicked(object sender, RoutedEventArgs e)
        {
            if (sender is not SettingsCard settingCard || settingCard.DataContext is not ConfigurationSubMenuViewModel item) return;

        DataTemplate template = (DataTemplate)MainGrid.Resources["ConfigurationSubMenuTemplate"];

        try
        {
            Frame.Navigate(typeof(SubSection), new Tuple<ConfigurationSubMenuViewModel, DataTemplate, object>(item, template, this.BreadcrumbBar.ItemsSource), new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }
        catch (Exception ex)
        {
            App.logger.Error($"Exception when attempting to navigate to {item.Type}: \n\t{ex.Message}\n\n{ex.InnerException}");
        }
    }

        private void ToggleSwitch_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is not ToggleSwitch toggleSwitch) return;
            toggleSwitch.Toggled -= ToggleSwitchBehavior.OnToggled;
            toggleSwitch.Toggled += ToggleSwitchBehavior.OnToggled;
        }

        private async void LinkCard_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not SettingsCard linkCard || linkCard.DataContext is not LinksViewModel linkVM || string.IsNullOrEmpty(linkVM.Link)) return;
            if (Uri.TryCreate(linkVM.Link, UriKind.Absolute, out Uri uri)) await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuFlyoutItem menuFlyoutItem || menuFlyoutItem.Tag is null) return;
            RegistryHelper.SetValue(@"HKLM\SOFTWARE\\EBOS\\Toolbox\\Favorites", menuFlyoutItem.Tag.ToString(), true);
        }
}
