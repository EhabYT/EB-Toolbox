using EBToolbox.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EBToolbox.ViewModels
{
    class ConfigPageViewModel : ObservableObject
    {
        private readonly List<IConfigurationItem> _allItems = new();
        private ObservableCollection<IConfigurationItem> _configurationItems = new();

        public ObservableCollection<IConfigurationItem> ConfigurationItems
        {
            get => _configurationItems;
            set => SetProperty(ref _configurationItems, value);
        }

        public ConfigPageViewModel(
            IEnumerable<ConfigurationItemViewModel> configurationItemViewModels,
            IEnumerable<ConfigurationSubMenuViewModel> configurationSubMenuViewModel,
            IEnumerable<MultiOptionConfigurationItemViewModel> multiOptionConfigurationItemViewModels,
            IEnumerable<LinksViewModel> linksViewModel,
            IEnumerable<ConfigurationButtonViewModel> configurationButtonViewModel)
        {
            configurationSubMenuViewModel.ToList().ForEach(_allItems.Add);
            multiOptionConfigurationItemViewModels.ToList().ForEach(_allItems.Add);
            configurationItemViewModels.ToList().ForEach(_allItems.Add);
            configurationButtonViewModel.ToList().ForEach(_allItems.Add);
            linksViewModel.ToList().ForEach(_allItems.Add);
            ConfigurationItems = new ObservableCollection<IConfigurationItem>(_allItems);
        }

        /// <summary>
        /// Gets the configuration services
        /// </summary>
        /// <param name="configurationType">Type to get</param>
        public void ShowForType(ConfigurationType configurationType)
        {
            ConfigurationItems = new ObservableCollection<IConfigurationItem>(_allItems.Where(item => item.Type == configurationType));
        }

        /// <summary>
        /// Loads the view model
        /// </summary>
        /// <param name="linksViewModels"></param>
        /// <param name="configurationItemViewModels"></param>
        /// <param name="multiOptionConfigurationItemViewModels"></param>
        /// <param name="configurationSubMenuViewModels"></param>
        /// <param name="configurationButtonViewModels"></param>
        /// <returns></returns>
        public static ConfigPageViewModel LoadViewModel(
            IEnumerable<LinksViewModel> linksViewModels,
            IEnumerable<ConfigurationItemViewModel> configurationItemViewModels,
            IEnumerable<MultiOptionConfigurationItemViewModel> multiOptionConfigurationItemViewModels,
            IEnumerable<ConfigurationSubMenuViewModel> configurationSubMenuViewModels,
            IEnumerable<ConfigurationButtonViewModel> configurationButtonViewModels)
        {
            ConfigPageViewModel viewModel = new(configurationItemViewModels, configurationSubMenuViewModels, multiOptionConfigurationItemViewModels, linksViewModels, configurationButtonViewModels);

            return viewModel;
        }
    }
}
