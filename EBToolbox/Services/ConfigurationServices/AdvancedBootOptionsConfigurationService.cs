using EBToolbox.Services;
using EBToolbox.Services.ConfigurationServices;
using EBToolbox.Stores;
using EBToolbox.Utils;
using BcdSharp.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace EBToolbox.Services.ConfigurationServices
{
    public class AdvancedBootOptionsConfigurationService : IConfigurationService
    {
        private const string EB_STORE_KEY_NAME = @"HKLM\SOFTWARE\EBOS\Services\AdvancedBootOptions";
        private const string STATE_VALUE_NAME = "state";

        private readonly ConfigurationStore _advancedBootOptionsConfigurationStore;
        private readonly IBcdService _bcdService;

        public AdvancedBootOptionsConfigurationService(
            [FromKeyedServices("AdvancedBootOptions")] ConfigurationStore advancedBootOptionsConfigurationStore,
            IBcdService bcdService)
        {
            _advancedBootOptionsConfigurationStore = advancedBootOptionsConfigurationStore;
            _bcdService = bcdService;
        }

        public void Disable()
        {
            _bcdService.DeleteElement(WellKnownObjectIdentifiers.GlobalSettings, WellKnownElementTypes.AdvancedOptions);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 0);

            _advancedBootOptionsConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            _bcdService.SetBooleanElement(WellKnownObjectIdentifiers.GlobalSettings, WellKnownElementTypes.AdvancedOptions, true);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);

            _advancedBootOptionsConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
