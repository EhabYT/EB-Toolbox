using EBToolbox.Services.ConfigurationServices;
using System.Collections.Generic;


namespace EBToolbox.Stores
{
    public class ConfigurationStoreSubMenu
    {
        private List<string> _configurationServices;

        public List<string> ConfigurationStores
        {
            get
            {
                return _configurationServices;
            }
            set
            {
                _configurationServices = value;
            }
        }
    }
}
