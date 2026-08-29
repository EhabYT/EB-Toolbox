using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBToolbox.Stores;
using EBToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using WinRT;

namespace EBToolbox.Services.ConfigurationServices
{
    public class AddNvidiaDisplayContainerContextMenuConfigurationService : IConfigurationService
    {

        private const string EB_STORE_KEY_NAME = @"HKLM\SOFTWARE\EBOS\Services\NVidiaDisplayContainerContextMenu";
        private const string STATE_VALUE_NAME = "state";

        private const string NVIDIA_CONTAINER_KEY_NAME = @"HKCR\DesktopBackground\Shell\NVIDIAContainer";
        private const string NVIDIA_CONTAINER_001_KEY_NAME = @"HKCR\DesktopBackground\shell\NVIDIAContainer\shell\NVIDIAContainer001";
        private const string NVIDIA_CONTAINER_001_COMMAND_KEY_NAME = @"HKCR\DesktopBackground\shell\NVIDIAContainer\shell\NVIDIAContainer001\command";
        private const string NVIDIA_CONTAINER_002_KEY_NAME = @"HKCR\DesktopBackground\shell\NVIDIAContainer\shell\NVIDIAContainer002";
        private const string NVIDIA_CONTAINER_002_COMMAND_KEY_NAME = @"HKCR\DesktopBackground\shell\NVIDIAContainer\shell\NVIDIAContainer002\command";

        private readonly ConfigurationStore _addNvidiaDisplayContainerContextMenuConfigurationService;

        public AddNvidiaDisplayContainerContextMenuConfigurationService(
            [FromKeyedServices("AddNvidiaDisplayContainerContextMenu")]  ConfigurationStore addNvidiaDisplayContainerContextMenuConfigurationService)
        {
            _addNvidiaDisplayContainerContextMenuConfigurationService = addNvidiaDisplayContainerContextMenuConfigurationService;
        }
        public void Disable()
        {
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.DeleteKey(NVIDIA_CONTAINER_KEY_NAME);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\6. Advanced Configuration\Services\NVIDIA Display Container\Context Menu\Remove Container Context Menu (default).cmd");

            _addNvidiaDisplayContainerContextMenuConfigurationService.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);

            RegistryHelper.SetValue(NVIDIA_CONTAINER_KEY_NAME, "Icon", "NVIDIA.ico,0", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(NVIDIA_CONTAINER_KEY_NAME, "MUIVerb", "NVIDIA Container", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(NVIDIA_CONTAINER_KEY_NAME, "Position", "Bottom", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(NVIDIA_CONTAINER_KEY_NAME, "SubCommands", "", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(NVIDIA_CONTAINER_001_KEY_NAME, "HasLUAShield", "", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(NVIDIA_CONTAINER_001_KEY_NAME, "MUIVerb", "Enable NVIDIA Display Container LS", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(NVIDIA_CONTAINER_001_COMMAND_KEY_NAME, "", @$"{Environment.GetEnvironmentVariable("windir")}\EBModules\Toolbox\Scripts\NVidia\EnableNVIDIADisplayContainerLS.cmd", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(NVIDIA_CONTAINER_002_KEY_NAME, "HasLUAShield", "", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(NVIDIA_CONTAINER_002_KEY_NAME, "MUIVerb", "Disable NVIDIA Display Container LS", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(NVIDIA_CONTAINER_002_COMMAND_KEY_NAME, "", @$"{Environment.GetEnvironmentVariable("windir")}\EBModules\Toolbox\Scripts\NVidia\DisableNVIDIADisplayContainerLS.cmd", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\6. Advanced Configuration\Services\NVIDIA Display Container\Context Menu\Add Container Context Menu.cmd");

            CommandPromptHelper.RestartExplorer();

            _addNvidiaDisplayContainerContextMenuConfigurationService.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
