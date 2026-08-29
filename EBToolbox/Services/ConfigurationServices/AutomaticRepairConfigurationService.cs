using EBToolbox.Services;
using EBToolbox.Services.ConfigurationServices;
using EBToolbox.Stores;
using EBToolbox.Utils;
using BcdSharp.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace EBToolbox.Services.ConfigurationServices
{
    public class AutomaticRepairConfigurationService : IConfigurationService
    {
        private const string EB_STORE_KEY_NAME = @"HKLM\SOFTWARE\EBOS\Services\AutomaticRepair";
        private const string STATE_VALUE_NAME = "state";


        private readonly ConfigurationStore _automaticRepairConfigurationStore;
        private readonly IBcdService _bcdService;

        public AutomaticRepairConfigurationService(
            [FromKeyedServices("AutomaticRepair")] ConfigurationStore automaticRepairConfigurationStore,
            IBcdService bcdService)
        {
            _automaticRepairConfigurationStore = automaticRepairConfigurationStore;
            _bcdService = bcdService;
        }

        public void Disable()
        {
            _bcdService.SetIntegerElement(WellKnownObjectIdentifiers.Current, WellKnownElementTypes.BootStatusPolicy, 1UL);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 0);


            _automaticRepairConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            _bcdService.SetIntegerElement(WellKnownObjectIdentifiers.Current, WellKnownElementTypes.BootStatusPolicy, 0UL);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);

            _automaticRepairConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
