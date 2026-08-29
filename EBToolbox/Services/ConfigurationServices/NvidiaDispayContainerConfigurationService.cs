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
    public class NvidiaDispayContainerConfigurationService : IConfigurationService
    {

        private const string EB_STORE_KEY_NAME = @"HKLM\SOFTWARE\EBOS\Services\NVidiaDisplayContainer";
        private const string STATE_VALUE_NAME = "state";

        private readonly ConfigurationStore _nvidiaDispayContainerConfigurationService;

        public NvidiaDispayContainerConfigurationService(
            [FromKeyedServices("NvidiaDispayContainer")]  ConfigurationStore nvidiaDispayContainerConfigurationService)
        {
            _nvidiaDispayContainerConfigurationService = nvidiaDispayContainerConfigurationService;
        }
        public void Disable()
        {
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 0);

            CommandPromptHelper.RunCommand(@$"{Environment.GetEnvironmentVariable("windir")}\EBModules\Toolbox\Scripts\NVidia\DisableNVIDIADisplayContainerLS.cmd", false);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\6. Advanced Configuration\Services\NVIDIA Display Container\Disable NVIDIA Display Container LS.cmd");

            _nvidiaDispayContainerConfigurationService.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            CommandPromptHelper.RunCommand(@$"{Environment.GetEnvironmentVariable("windir")}\EBModules\Toolbox\Scripts\NVidia\DisableNVIDIADisplayContainerLS.cmd", false);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\6. Advanced Configuration\Services\NVIDIA Display Container\Enable NVIDIA Display Container LS (default).cmd");

            _nvidiaDispayContainerConfigurationService.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
