using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBToolbox.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace EBToolbox.Services.ConfigurationSubMenu
{
    public class NvidiaDisplayContainerSubMenu : IConfigurationSubMenu
    {
        private readonly ConfigurationStoreSubMenu _nvidiaDisplayContainerSubMenu;
        public NvidiaDisplayContainerSubMenu(
            [FromKeyedServices("NvidiaDisplayContainerSubMenu")] ConfigurationStoreSubMenu nvidiaDisplayContainerSubMenu)
        {
            _nvidiaDisplayContainerSubMenu = nvidiaDisplayContainerSubMenu;
        }
    }
}