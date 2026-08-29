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
    public class ToggleWindowsUpdateConfigurationService : IConfigurationService
    {
        private const string EB_STORE_KEY_NAME = @"HKLM\SOFTWARE\EBOS\Services\ToggleWindowsUpdates";
        private const string STATE_VALUE_NAME = "state";

        private readonly ConfigurationStore _configurationStore;
        public ToggleWindowsUpdateConfigurationService(
            [FromKeyedServices("ToggleWindowsUpdates")] ConfigurationStore configurationStore)
        {
            _configurationStore = configurationStore;
        }

        public void Disable()
        {
            CommandPromptHelper.RunCommand("sc stop wuauserv");
            CommandPromptHelper.RunCommand("sc config wuauserv start= disabled");
            CommandPromptHelper.RunCommand("schtasks /Change /TN \"Microsoft\\Windows\\WindowsUpdate\\sih\" /Disable");
            CommandPromptHelper.RunCommand("schtasks /Change /TN \"Microsoft\\Windows\\WindowsUpdate\\sihboot\" /Disable ");


            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\3. General Configuration\Windows Updates");
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "state", 0);

            _configurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            CommandPromptHelper.RunCommand("sc config wuauserv start= demand");
            CommandPromptHelper.RunCommand("sc start wuauserv");
            CommandPromptHelper.RunCommand("schtasks /Change /TN \"Microsoft\\Windows\\WindowsUpdate\\sih\" /Enable");
            CommandPromptHelper.RunCommand("schtasks /Change /TN \"Microsoft\\Windows\\WindowsUpdate\\sihboot\" /Enable");

            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\3. General Configuration\Windows Updates");
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "state", 1);

            _configurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
