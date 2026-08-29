using EBToolbox.Services.ConfigurationServices;
using EBToolbox.Stores;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace EBToolbox.Services.ConfigurationSubMenu
{
    internal class ServicesSubMenu : IConfigurationSubMenu
    {

        private readonly ConfigurationStoreSubMenu _servicesSubMenu;

        public ServicesSubMenu(
            [FromKeyedServices("ServicesSubMenu")] ConfigurationStoreSubMenu servicesSubMenu)
        {
            _servicesSubMenu = servicesSubMenu;
        }
    }
}
