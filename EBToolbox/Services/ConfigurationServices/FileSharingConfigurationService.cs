using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBToolbox.Stores;
using EBToolbox.Utils;
using CommunityToolkit.WinUI;
using Microsoft.Extensions.DependencyInjection;
using Windows.Devices.PointOfService;

namespace EBToolbox.Services.ConfigurationServices
{
    public class FileSharingConfigurationService : IConfigurationService
    {
        private const string EB_STORE_KEY_NAME = @"HKLM\SOFTWARE\EBOS\Services\FileSharing";
        private const string STATE_VALUE_NAME = "state";

        private readonly ConfigurationStore _configurationStore;
        public FileSharingConfigurationService(
            [FromKeyedServices("FileSharing")] ConfigurationStore configurationStore)
        {
            _configurationStore = configurationStore;
        }
        public void Disable()
        {
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            CommandPromptHelper.RunCommand(@$"{Environment.GetEnvironmentVariable("windir")}EBModules\Toolbox\ConfigurationServices\FileSharing\disable.cmd", false);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\3. General Configuration\File Sharing\Disable File Sharing (default).cmd");

            _configurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            CommandPromptHelper.RunCommand(@$"{Environment.GetEnvironmentVariable("windir")}EBModules\Toolbox\ConfigurationServices\FileSharing\enable.cmd", false);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\3. General Configuration\File Sharing\Enable File Sharing.cmd");

            _configurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
