using EBToolbox.Stores;
using EBToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace EBToolbox.Services.ConfigurationServices
{
    public class QuickAccessConfigurationService : IConfigurationService
    {
        private const string EB_STORE_KEY_NAME = @"HKLM\SOFTWARE\EBOS\Services\QuickAccess";
        private const string STATE_VALUE_NAME = "state";

        private const string EXPLORER_KEY_NAME = @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer";

        private const string HUB_MODE_VALUE_NAME = "HubMode";

        private readonly ConfigurationStore _quickAccessConfigurationStore;

        public QuickAccessConfigurationService(
            [FromKeyedServices("QuickAccess")] ConfigurationStore quickAccessConfigurationStore)
        {
            _quickAccessConfigurationStore = quickAccessConfigurationStore;
        }

        public void Disable()
        {
            RegistryHelper.SetValue(EXPLORER_KEY_NAME, HUB_MODE_VALUE_NAME, 1);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 0);


            _quickAccessConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.DeleteValue(EXPLORER_KEY_NAME, HUB_MODE_VALUE_NAME);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);

            _quickAccessConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
