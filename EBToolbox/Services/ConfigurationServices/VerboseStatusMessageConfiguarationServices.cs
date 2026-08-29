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
    public class VerboseStatusMessageConfiguarationServices : IConfigurationService
    {
        private const string EB_STORE_KEY_NAME = @"HKLM\SOFTWARE\EBOS\Services\VerboseStatusMessage";
        private const string STATE_VALUE_NAME = "state";

        private const string SYSTEM_POLICIES_KEY_NAME = @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";
        private const string VERBOSE_STATUS = "verbosestatus";

        private readonly ConfigurationStore _verboseStatusMessageConfigurationService;

        public VerboseStatusMessageConfiguarationServices(
            [FromKeyedServices("VerboseStatusMessage")] ConfigurationStore verboseStatusMessageConfigurationService)
        {
            _verboseStatusMessageConfigurationService = verboseStatusMessageConfigurationService;
        }
        public void Disable()
        {
            RegistryHelper.DeleteValue(SYSTEM_POLICIES_KEY_NAME, VERBOSE_STATUS);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\4. Interface Tweaks\Verbose Status Messages\Disable Verbose Messages (default).cmd");

            _verboseStatusMessageConfigurationService.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(SYSTEM_POLICIES_KEY_NAME, VERBOSE_STATUS, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\4. Interface Tweaks\Verbose Status Messages\Enable Verbose Messages.cmd");

            _verboseStatusMessageConfigurationService.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
