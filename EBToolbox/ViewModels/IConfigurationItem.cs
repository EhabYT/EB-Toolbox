using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBToolbox.Enums;

namespace EBToolbox.ViewModels
{
    public interface IConfigurationItem
    {
        string Name { get; }
        string Key { get; }
        ConfigurationType Type { get; }
    }
}
