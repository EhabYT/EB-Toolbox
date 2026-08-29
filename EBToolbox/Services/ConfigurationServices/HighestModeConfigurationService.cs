using EBToolbox.Stores;
using EBToolbox.Utils;
using BcdSharp.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace EBToolbox.Services.ConfigurationServices
{
    public class HighestModeConfigurationService : IConfigurationService
    {
        private const string EB_STORE_KEY_NAME = @"HKLM\SOFTWARE\EBOS\Services\HightestMode";
        private const string STATE_VALUE_NAME = "state";

        private readonly ConfigurationStore _highestModeConfigurationStore;
        private readonly IBcdService _bcdService;

        public HighestModeConfigurationService(
            [FromKeyedServices("HighestMode")] ConfigurationStore highestModeConfigurationStore,
            IBcdService bcdService)
        {
            _highestModeConfigurationStore = highestModeConfigurationStore;
            _bcdService = bcdService;
        }

        public void Disable()
        {
            _bcdService.DeleteElement(WellKnownObjectIdentifiers.GlobalSettings, WellKnownElementTypes.HighestMode);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 0);

            _highestModeConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            _bcdService.SetBooleanElement(WellKnownObjectIdentifiers.GlobalSettings, WellKnownElementTypes.HighestMode, true);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);

            _highestModeConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
        }
    }
}
