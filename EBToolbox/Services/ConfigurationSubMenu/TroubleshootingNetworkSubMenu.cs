using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBToolbox.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace EBToolbox.Services.ConfigurationSubMenu
{
    public class TroubleshootingNetworkSubMenu : IConfigurationSubMenu
    {
        private readonly ConfigurationStore _troubleshootingNetworkSubMenu;
        public TroubleshootingNetworkSubMenu(
            [FromKeyedServices("TroubleshootingNetwork")] ConfigurationStore troubleshootingNetworkSubMenu)
        {
            _troubleshootingNetworkSubMenu = troubleshootingNetworkSubMenu;
        }
    }
}
