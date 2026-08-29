using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBToolbox.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace EBToolbox.Services.ConfigurationSubMenu
{
    public class CoreIsolationSubMenu : IConfigurationSubMenu
    {
        private readonly ConfigurationStore _coreIsolationSubMenu;
        public CoreIsolationSubMenu(
            [FromKeyedServices("CoreIsolationSubMenu")] ConfigurationStore coreIsolationSubMenu)
        {
            _coreIsolationSubMenu = coreIsolationSubMenu;
        }
    }
}
