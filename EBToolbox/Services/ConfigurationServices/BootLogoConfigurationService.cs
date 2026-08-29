using EBToolbox.Services;
using EBToolbox.Services.ConfigurationServices;
using EBToolbox.Stores;
using EBToolbox.Utils;
using BcdSharp.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace EBToolbox.Services.ConfigurationServices
{
    public class BootLogoConfigurationService : IConfigurationService
    {

        private const string EB_STORE_KEY_NAME = @"HKLM\SOFTWARE\EBOS\Services\BootLogo";
        private const string STATE_VALUE_NAME = "state";

        private readonly ConfigurationStore _configurationStore;
        private readonly IBcdService _bcdService;

        public BootLogoConfigurationService(
            [FromKeyedServices("BootLogo")] ConfigurationStore configurationStore,
            IBcdService bcdService)
        {
            _configurationStore = configurationStore;
            _bcdService = bcdService;
        }

        public void Disable()
        {
            _bcdService.SetBooleanElement(WellKnownObjectIdentifiers.GlobalSettings, WellKnownElementTypes.NoBootUxLogo, true);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 0);

            _configurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            _bcdService.DeleteElement(WellKnownObjectIdentifiers.GlobalSettings, WellKnownElementTypes.NoBootUxLogo);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);

            _configurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
