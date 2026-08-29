using EBToolbox.Stores;
using EBToolbox.Utils;
using BcdSharp.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace EBToolbox.Services.ConfigurationServices
{
    public class NewBootMenuConfigurationService : IConfigurationService
    {

        private const string EB_STORE_KEY_NAME = @"HKLM\SOFTWARE\EBOS\Services\NewBootMenu";
        private const string STATE_VALUE_NAME = "state";

        private readonly ConfigurationStore _newBootMenuConfigurationStore;
        private readonly IBcdService _bcdService;

        public NewBootMenuConfigurationService(
            [FromKeyedServices("NewBootMenu")] ConfigurationStore newBootMenuConfigurationStore,
            IBcdService bcdService)
        {
            _newBootMenuConfigurationStore = newBootMenuConfigurationStore;
            _bcdService = bcdService;
        }

        public void Disable()
        {
            _bcdService.SetIntegerElement(WellKnownObjectIdentifiers.Default, WellKnownElementTypes.BootMenuPolicyWinload, 0UL);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 0);

            _newBootMenuConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            _bcdService.SetIntegerElement(WellKnownObjectIdentifiers.Default, WellKnownElementTypes.BootMenuPolicyWinload, 1UL);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);

            _newBootMenuConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
