using EBToolbox.Services.ConfigurationServices;
using EBToolbox.Models;
using EBToolbox.Stores;
using System.Windows.Input;
using EBToolbox.Commands;
using EBToolbox.Enums;
using Windows.UI;
using Microsoft.UI.Xaml.Media;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
//using System.Drawing;

namespace EBToolbox.ViewModels
{
    public class ConfigurationItemViewModel : IConfigurationItem
    {
        private readonly ConfigurationStore _configurationStore;
        private readonly IConfigurationService _configurationService;

        public Configuration Configuration { get; set; }
        public string Name => Configuration.Name;
        public string Key => Configuration.Key;
        public string Description => Configuration.Description;
        public ConfigurationType Type => Configuration.Type;
        public FontIcon Icon => Configuration.Icon;

        private bool _currentSetting;

        public bool CurrentSetting
        {
            get => _currentSetting;
            set
            {
                _currentSetting = value;
                _configurationStore.CurrentSetting = CurrentSetting;
                this.SaveConfigurationCommand.Execute(this);
            }
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
            }
        }


        public ICommand SaveConfigurationCommand { get; }

        public ConfigurationItemViewModel(
            Configuration configuration,
            ConfigurationStore configurationStore,
            IConfigurationService configurationService)
        {
            _configurationStore = configurationStore;
            _configurationService = configurationService;
            Configuration = configuration;

            _currentSetting = FetchCurrentSetting();
            SaveConfigurationCommand = new SaveConfigurationCommand(this, configurationStore, configurationService);
            
        }

        public bool FetchCurrentSetting()
        {
            IsBusy = true;

            try
            {
                bool currentSetting = _configurationService.IsEnabled();
                _configurationStore.CurrentSetting = currentSetting;
                return currentSetting;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
