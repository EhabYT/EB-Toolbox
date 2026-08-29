using EBToolbox.Stores;
using EBToolbox.Utils;
using BcdSharp.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace EBToolbox.Services.ConfigurationServices
{
    public class KernelParametersConfigurationService : IConfigurationService
    {
        private const string EB_STORE_KEY_NAME = @"HKLM\SOFTWARE\EBOS\Services\KernelParameters";
        private const string STATE_VALUE_NAME = "state";

        private readonly ConfigurationStore _kernelParametersConfigurationStore;
        private readonly IBcdService _bcdService;

        public KernelParametersConfigurationService(
            [FromKeyedServices("KernelParameters")] ConfigurationStore kernelParametersConfigurationStore,
            IBcdService bcdService)
        {
            _kernelParametersConfigurationStore = kernelParametersConfigurationStore;
            _bcdService = bcdService;
        }

        public void Disable()
        {
            _bcdService.DeleteElement(WellKnownObjectIdentifiers.GlobalSettings, WellKnownElementTypes.OptionsEdit);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 0);

            _kernelParametersConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            _bcdService.SetBooleanElement(WellKnownObjectIdentifiers.GlobalSettings, WellKnownElementTypes.OptionsEdit, true);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);

            _kernelParametersConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
