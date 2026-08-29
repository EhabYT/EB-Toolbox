using EBToolbox.Stores;
using EBToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.ServiceProcess;

namespace EBToolbox.Services.ConfigurationServices
{
    public class SearchIndexingConfigurationService : IConfigurationService
    {
        private const string EB_STORE_KEY_NAME = @"HKLM\SOFTWARE\EBOS\Services\SearchIndexing";
        private const string STATE_VALUE_NAME = "state";

        private const string WSEARCH_SERVICE_NAME = "WSearch";


        private readonly ConfigurationStore _searchIndexingConfigurationStore;

        public SearchIndexingConfigurationService(
            [FromKeyedServices("SearchIndexing")] ConfigurationStore searchIndexingConfigurationStore)
        {
            _searchIndexingConfigurationStore = searchIndexingConfigurationStore;
        }

        public void Disable()
        {
            ServiceHelper.SetStartupType(WSEARCH_SERVICE_NAME, ServiceStartMode.Disabled);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 0);

            _searchIndexingConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            ServiceHelper.SetStartupType(WSEARCH_SERVICE_NAME, ServiceStartMode.Automatic);
            RegistryHelper.SetValue(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);

            _searchIndexingConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(EB_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
