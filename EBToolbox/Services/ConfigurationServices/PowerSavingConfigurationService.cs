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
    internal class PowerSavingConfigurationService : IConfigurationService
    {
        private const string EB_STORE_KEY_NAME = @"HKLM\SOFTWARE\EBOS\Services\PowerSaving";
        private const string STATE_VALUE_NAME = "state";

        private const string DEFAULT_POWER_SAVING_SCRIPT_PATH_NAME = @"%windir%\EBModules\Scripts\ScriptWrappers\DefaultPowerSaving.ps1";
        private const string DISABLE_POWER_SAVING_SCRIPT_PATH_NAME = @"%windir%\EBModules\Scripts\ScriptWrappers\DisablePowerSaving.ps1";

        private readonly ConfigurationStore _powerSavingConfigurationStore;

        public PowerSavingConfigurationService(
            [FromKeyedServices("PowerSaving")] ConfigurationStore powerSavingConfigurationService) 
        {
            _powerSavingConfigurationStore = powerSavingConfigurationService;
        }
        public void Disable()
        {
            CommandPromptHelper.RunCommand($@"powershell -EP Bypass -NoP ^& """"""$env:{DISABLE_POWER_SAVING_SCRIPT_PATH_NAME}"""""" %*");
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\3. General Configuration\Power-saving\Default Power-saving (default).cmd");

            _powerSavingConfigurationStore.CurrentSetting = IsEnabled();

        }

        public void Enable()
        {
            CommandPromptHelper.RunCommand($@"powershell -EP Bypass -NoP ^& """"""$env:{DEFAULT_POWER_SAVING_SCRIPT_PATH_NAME}"""""" %*");
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\3. General Configuration\Power-saving\Disable Power-saving.cmd");

            _powerSavingConfigurationStore.CurrentSetting = IsEnabled();

        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
