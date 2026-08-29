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
    class RecallSupportConfigurationService : IConfigurationService
    {
        private const string EB_STORE_KEY_NAME = @"HKLM\SOFTWARE\EBOS\Services\recall";
        private const string STATE_VALUE_NAME = "state";

        private const string WINDOWS_AI_KEY_NAME = @"HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsAI";

        private readonly ConfigurationStore _recallConfigurationStore;

        public RecallSupportConfigurationService([FromKeyedServices("Recall")] ConfigurationStore RecallConfigurationStore)
        {
            _recallConfigurationStore = RecallConfigurationStore;
        }

        public void Disable()
        {
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\3. General Configuration\AI Features\Microsoft Copilot\Disable Recall Support (default).cmd");

            RegistryHelper.SetValue(WINDOWS_AI_KEY_NAME, "DisableAIDataAnalysis", 01, Microsoft.Win32.RegistryValueKind.DWord);
            _recallConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\3. General Configuration\AI Features\Microsoft Copilot\Enable Recall Support.cmd");

            RegistryHelper.DeleteValue(WINDOWS_AI_KEY_NAME, "DisableAIDataAnalysis");

            _recallConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
