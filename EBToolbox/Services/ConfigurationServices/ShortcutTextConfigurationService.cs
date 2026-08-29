using EBToolbox.Stores;
using EBToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EBToolbox.Services.ConfigurationServices
{
    public class ShortcutTextConfigurationService : IConfigurationService
    {
        private const string EB_STORE_KEY_NAME = @"HKLM\SOFTWARE\EBOS\Services\ShortcutText";
        private const string STATE_VALUE_NAME = "state";

        private const string EXPLORER_KEY_NAME = @"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\NamingTemplates";

        private const string SHORTCUT_NAME_TEMPLATE_VALUE_NAME = "ShortcutNameTemplate";

        private readonly ConfigurationStore _shortcutTextConfigurationStore;

        public ShortcutTextConfigurationService(
            [FromKeyedServices("ShortcutText")] ConfigurationStore shortcutTextConfigurationStore)
        {
            _shortcutTextConfigurationStore = shortcutTextConfigurationStore;
        }

        public void Disable()
        {
            RegistryHelper.SetValue(EXPLORER_KEY_NAME, SHORTCUT_NAME_TEMPLATE_VALUE_NAME, "\"%s.lnk\"");
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\4. Interface Tweaks\Shortcut Text\Disable Shortcut Text (default).cmd");

            _shortcutTextConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.DeleteValue(EXPLORER_KEY_NAME, SHORTCUT_NAME_TEMPLATE_VALUE_NAME);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\4. Interface Tweaks\Shortcut Text\Enable Shortcut Text.cmd");

            _shortcutTextConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
