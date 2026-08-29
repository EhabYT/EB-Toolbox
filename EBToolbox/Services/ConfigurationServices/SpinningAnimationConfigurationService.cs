using EBToolbox.Stores;
using EBToolbox.Utils;
using BcdSharp.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace EBToolbox.Services.ConfigurationServices
{
    public class SpinningAnimationConfigurationService : IConfigurationService
    {

        private const string EB_STORE_KEY_NAME = @"HKLM\SOFTWARE\EBOS\Services\SpinningAnimations";
        private const string STATE_VALUE_NAME = "state";

        private readonly ConfigurationStore _spinningAnimationConfigurationStore;
        private readonly IBcdService _bcdService;

        public SpinningAnimationConfigurationService(
            [FromKeyedServices("SpinningAnimation")] ConfigurationStore spinningAnimationConfigurationStore,
            IBcdService bcdService)
        {
            _spinningAnimationConfigurationStore = spinningAnimationConfigurationStore;
            _bcdService = bcdService;
        }

        public void Disable()
        {
            _bcdService.SetBooleanElement(WellKnownObjectIdentifiers.GlobalSettings, WellKnownElementTypes.NoBootUxProgress, true);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 0);

            _spinningAnimationConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            _bcdService.DeleteElement(WellKnownObjectIdentifiers.GlobalSettings, WellKnownElementTypes.NoBootUxProgress);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);

            _spinningAnimationConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
