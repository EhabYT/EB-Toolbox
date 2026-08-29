using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EBToolbox.Stores;
using EBToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace EBToolbox.Services.ConfigurationServices
{
    public class SecurityHealthTrayConfigurationService : IConfigurationService
    {
        private const string EB_STORE_KEY_NAME = @"HKLM\SOFTWARE\EBOS\Services\SecurityHealthTray";
        private const string STATE_VALUE_NAME = "state";

        private readonly ConfigurationStore _securityHealthTrayConfigurationService;

        public SecurityHealthTrayConfigurationService(
            [FromKeyedServices("SecurityHealthTray")] ConfigurationStore securityHealthTrayConfigurationService)
        {
            _securityHealthTrayConfigurationService = securityHealthTrayConfigurationService;
        }

        public void Disable()
        {
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.MergeRegFile(@$"{Environment.GetEnvironmentVariable("windir")}\EBModules\Toolbox\Scripts\SecurityHealthTray\RemoveTray.reg");
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\7. Security\Defender\Security Health Tray\Remove Security Tray from Startup (default).cmd");

            _securityHealthTrayConfigurationService.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.MergeRegFile(@$"{Environment.GetEnvironmentVariable("windir")}\EBModules\Toolbox\Scripts\SecurityHealthTray\AddTray.reg");
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\7. Security\Defender\Security Health Tray\Add Security Tray from Startup.cmd");

            _securityHealthTrayConfigurationService.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
