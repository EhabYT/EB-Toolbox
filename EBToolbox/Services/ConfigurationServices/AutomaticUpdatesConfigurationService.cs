using EBToolbox.Services;
using EBToolbox.Services.ConfigurationServices;
using EBToolbox.Stores;
using EBToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace EBToolbox.Services.ConfigurationServices
{
    internal class AutomaticUpdatesConfigurationService : IConfigurationService
    {
        private const string EB_STORE_KEY_NAME = @"HKLM\SOFTWARE\EBOS\Services\AutomaticUpdates";
        private const string STATE_VALUE_NAME = "state";

        private const string AU_KEY_NAME = @"HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU";

        private const string AU_OPTIONS_VALUE_NAME = "AUOptions";

        private readonly ConfigurationStore _automaticRepairConfigurationStore;
        public AutomaticUpdatesConfigurationService(
            [FromKeyedServices("AutomaticUpdates")] ConfigurationStore automaticUpdatesConfigurationStore) 
        {
            _automaticRepairConfigurationStore = automaticUpdatesConfigurationStore;
        }
        public void Disable()
        {
            RegistryHelper.SetValue(AU_KEY_NAME, AU_OPTIONS_VALUE_NAME, 2, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\3. General Configuration\Automatic Updates\Disable Automatic Updates (default).cmd");

            _automaticRepairConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.DeleteValue(AU_KEY_NAME, AU_OPTIONS_VALUE_NAME);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\3. General Configuration\Automatic Updates\Enable Automatic Updates.cmd");

            _automaticRepairConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            bool[] checks =
            {
                RegistryHelper.IsMatch(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1)
            };

            return checks.All(x => x);
        }   
    }
}
