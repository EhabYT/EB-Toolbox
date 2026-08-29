using EBToolbox.Services;
using EBToolbox.Services.ConfigurationServices;
using EBToolbox.Stores;
using EBToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace EBToolbox.Services.ConfigurationServices
{
    internal class BackgroundAppsConfigurationService : IConfigurationService
    {
        private const string EB_STORE_KEY_NAME = @"HKLM\SOFTWARE\EBOS\Services\BackgroundApps";
        private const string STATE_VALUE_NAME = "state";

        private const string BACKGROUND_ACCESS_APPLICATION_KEY_NAME = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications";
        private const string SEARCH_KEY_NAME = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Search";

        private const string GLOBAL_USER_DISABLED_VALUE_NAME = "GlobalUserDisabled";
        private const string BACKGROUND_APP_GLOBAL_TOGGLE_VALUE_NAME = "BackgroundAppGlobalToggle";

        private readonly ConfigurationStore _backgroundAppsConfigurationService;
        public BackgroundAppsConfigurationService(
            [FromKeyedServices("BackgroundApps")] ConfigurationStore backgroundAppsConfigurationService)
        {
            _backgroundAppsConfigurationService = backgroundAppsConfigurationService;
        }
        public void Disable()
        {
            RegistryHelper.SetValue(BACKGROUND_ACCESS_APPLICATION_KEY_NAME, GLOBAL_USER_DISABLED_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(SEARCH_KEY_NAME, BACKGROUND_APP_GLOBAL_TOGGLE_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\3. General Configuration\Background Apps\Disable Background Apps (default).cmd");

            _backgroundAppsConfigurationService.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.DeleteValue(BACKGROUND_ACCESS_APPLICATION_KEY_NAME, GLOBAL_USER_DISABLED_VALUE_NAME);
            RegistryHelper.DeleteValue(SEARCH_KEY_NAME, BACKGROUND_APP_GLOBAL_TOGGLE_VALUE_NAME);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\3. General Configuration\Background Apps\Enable Background Apps.cmd");

            _backgroundAppsConfigurationService.CurrentSetting = IsEnabled();
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
