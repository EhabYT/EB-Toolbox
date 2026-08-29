using EBToolbox.Services.ConfigurationServices;
using EBToolbox.Stores;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace EBToolbox.Services.ConfigurationSubMenu
{
    public class AiSubMenu : IConfigurationSubMenu
    {

        private readonly ConfigurationStoreSubMenu _aiSubMenu;

        public AiSubMenu(
            [FromKeyedServices("AiSubMenu")] ConfigurationStoreSubMenu aiSubMenu)
        {
            _aiSubMenu = aiSubMenu;
        }
    }
}
