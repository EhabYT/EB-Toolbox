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
    internal class SafeModeConfigurationService : IMultiOptionConfigurationServices
    {
        private const string EB_STORE_KEY_NAME = @"HKLM\SOFTWARE\EBOS\Services\SafeMode";
        private const string STATE_VALUE_NAME = "state";

        private readonly MultiOptionConfigurationStore _safeModeConfigurationService;

        private const string CONTEXT_MENU_REG_FILE_PATH = "C:\\Windows\\EBModues\\Scripts\\ConfigurationServices\\SafeMode\\SafeMode_";

        private List<string> options = new List<string>()
        {
            "Exit Safe Mode",
            "Safe Mode with Command Prompt",
            "Safe Mode with Networking",
            "Safe Mode",
        };

        public SafeModeConfigurationService(
            [FromKeyedServices("SafeMode")] MultiOptionConfigurationStore safeModeConfigurationService)
        {
            _safeModeConfigurationService = safeModeConfigurationService;
            _safeModeConfigurationService.Options = options;
        }

        public void ChangeStatus(int status)
        {
            RegistryHelper.MergeRegFile(CONTEXT_MENU_REG_FILE_PATH + status.ToString() + ".reg");
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, status);

            _safeModeConfigurationService.CurrentSetting = Status();
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
