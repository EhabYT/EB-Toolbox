using EBToolbox.Stores;
using EBToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;


namespace EBToolbox.Services.ConfigurationServices
{
    internal class SystemRestoreConfigurationService : IConfigurationService
    {
        private const string EB_STORE_KEY_NAME = @"HKLM\SOFTWARE\EBOS\Services\SystemRestore";
        private const string STATE_VALUE_NAME = "state";

        private const string SYSTEM_RESTORE_KEY_NAME = @"HKLM\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore";

        private const string DISABLE_SR_VALUE_NAME = "DisableSR";

        private readonly ConfigurationStore _systemRestoreConfigurationService;
        public SystemRestoreConfigurationService(
            [FromKeyedServices("SystemRestore")] ConfigurationStore systemRestoreConfigurationService)
        {
            _systemRestoreConfigurationService = systemRestoreConfigurationService;
        }

        public void Disable()
        {
            RegistryHelper.SetValue(SYSTEM_RESTORE_KEY_NAME, DISABLE_SR_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\3. General Configuration\System Restore\Disable System Restore.cmd");

            _systemRestoreConfigurationService.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.DeleteValue(SYSTEM_RESTORE_KEY_NAME, DISABLE_SR_VALUE_NAME);
            RegistryHelper.DeleteValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\3. General Configuration\System Restore\Enable System Restore (default).cmd");

            _systemRestoreConfigurationService.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
