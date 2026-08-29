using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBToolbox.Stores;
using EBToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace EBToolbox.Services.ConfigurationServices
{
    public class SuperFetchConfigurationService : IConfigurationService
    {
        private const string EB_STORE_KEY_NAME = @"HKLM\SOFTWARE\EBOS\Services\SuperFetch";
        private const string STATE_VALUE_NAME = "state";

        private readonly ConfigurationStore _superFetchConfigurationService;

        public SuperFetchConfigurationService(
            [FromKeyedServices("SuperFetch")] ConfigurationStore superFetchConfigurationService)
        {
            _superFetchConfigurationService = superFetchConfigurationService;
        }
        public void Disable()
        {
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            CommandPromptHelper.RunCommand(@$"{Environment.GetEnvironmentVariable("windir")}\EBModules\Toolbox\Scripts\SuperFetch\DisableSuperFetch.cmd");
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\6. Advanced Configuration\Services\Superfetch\Disable SuperFetch.cmd");

            _superFetchConfigurationService.CurrentSetting = IsEnabled();
            App.ContentDialogCaller("restart");
        }

        public void Enable()
        {
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            CommandPromptHelper.RunCommand(@$"{Environment.GetEnvironmentVariable("windir")}\EBModules\Toolbox\Scripts\SuperFetch\DisableSuperFetch.cmd");
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\6. Advanced Configuration\Services\Superfetch\Enable SuperFetch (default).cmd");

            _superFetchConfigurationService.CurrentSetting = IsEnabled();
            App.ContentDialogCaller("restart");
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
