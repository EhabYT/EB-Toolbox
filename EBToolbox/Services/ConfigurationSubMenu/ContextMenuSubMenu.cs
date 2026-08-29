using EBToolbox.Services.ConfigurationServices;
using EBToolbox.Stores;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace EBToolbox.Services.ConfigurationSubMenu
{
    public class ContextMenuSubMenu : IConfigurationSubMenu
    {
        private readonly ConfigurationStoreSubMenu _contextMenuConfigurationSubMenu;

        public ContextMenuSubMenu(
            [FromKeyedServices("ContextMenuSubMenu")] ConfigurationStoreSubMenu contextMenuSubMenu)
        {
            _contextMenuConfigurationSubMenu = contextMenuSubMenu;
        }
    }
}
