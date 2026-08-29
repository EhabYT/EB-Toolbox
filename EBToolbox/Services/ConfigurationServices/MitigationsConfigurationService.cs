using System.Collections.Generic;
using EBToolbox.Stores;
using EBToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace EBToolbox.Services.ConfigurationServices
{
    public class MitigationsConfigurationService : IMultiOptionConfigurationServices
    {
        private const string EB_STORE_KEY_NAME = @"HKLM\SOFTWARE\EBOS\Services\Mitigations";
        private const string STATE_VALUE_NAME = "state";

        private readonly MultiOptionConfigurationStore _mitigationsConfigurationService;

        private const string CONTEXT_MENU_REG_FILE_PATH = "C:\\Windows\\EBModues\\Scripts\\ConfigurationServices\\Mitigations\\Mitigations_";

        private List<string> options = new List<string>()
        {
            "Disable mitigations",
            "Enable all mitigations",
            "Default Windows mitigations",
        };

        public MitigationsConfigurationService(
            [FromKeyedServices("Mitigations")] MultiOptionConfigurationStore mitigationsConfigurationService)
        {
            _mitigationsConfigurationService = mitigationsConfigurationService;
            _mitigationsConfigurationService.Options = options;
        }

        public void ChangeStatus(int status)
        {
            RegistryHelper.MergeRegFile(CONTEXT_MENU_REG_FILE_PATH + status.ToString() + ".reg");
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, status);

            _mitigationsConfigurationService.CurrentSetting = Status();
        }

        public string Status()
        {
            try
            {
                return options[((int)RegistryHelper.GetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME))];
            }
            catch
            {
                ChangeStatus(2);
                return options[((int)RegistryHelper.GetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME))];
            }
        }
    }
}
