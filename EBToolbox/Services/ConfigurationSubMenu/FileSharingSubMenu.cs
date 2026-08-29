using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBToolbox.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace EBToolbox.Services.ConfigurationSubMenu
{
    public class FileSharingSubMenu : IConfigurationSubMenu
    {
        private readonly ConfigurationStoreSubMenu _configurationStoreSubMenu;
        public FileSharingSubMenu(
            [FromKeyedServices("FileSharingSubMenu")] ConfigurationStoreSubMenu configurationStoreSubMenu)
        {
            _configurationStoreSubMenu = configurationStoreSubMenu;
        }
    }
}
