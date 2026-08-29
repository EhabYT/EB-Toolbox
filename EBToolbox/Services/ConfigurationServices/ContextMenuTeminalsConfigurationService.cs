using System.Collections.Generic;
using EBToolbox.Stores;
using EBToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace EBToolbox.Services.ConfigurationServices
{
    public class ContextMenuTeminalsConfigurationService : IMultiOptionConfigurationServices
    {

        private const string EB_STORE_KEY_NAME = @"HKLM\SOFTWARE\EBOS\Services\ContextMenuTerminals";
        private const string STATE_VALUE_NAME = "state";

        private readonly MultiOptionConfigurationStore _contextMenuTeminalsConfigurationService;

        private const string CONTEXT_MENU_REG_FILE_PATH = "C:\\Windows\\EBModues\\Scripts\\ConfigurationServices\\ContextMenuTerminals\\ContextMenuTerminals_";

        private List<string> options = new List<string>()
        {
            "Add terminals",
            "Add terminals (no Windows Terminal)",
            "Remove terminals from the context menu",
        };

        public ContextMenuTeminalsConfigurationService(
            [FromKeyedServices("ContextMenuTerminals")] MultiOptionConfigurationStore contextMenuTeminalsConfigurationService)
        {
            _contextMenuTeminalsConfigurationService = contextMenuTeminalsConfigurationService;
            _contextMenuTeminalsConfigurationService.Options = options;
        }

        public void ChangeStatus(int status)
        {
            RegistryHelper.MergeRegFile(CONTEXT_MENU_REG_FILE_PATH + status.ToString() + ".reg");
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, status);

            _contextMenuTeminalsConfigurationService.CurrentSetting = Status();
        }

        public string Status()
        {
            try
            {
                return options[((int)RegistryHelper.GetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME))];
            }
            catch
            {
                ChangeStatus(0);
                return options[((int)RegistryHelper.GetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME))];

            }
        }
    }
}
