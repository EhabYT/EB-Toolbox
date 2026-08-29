using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EBToolbox.Enums;
using EBToolbox.Models;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;

namespace EBToolbox.ViewModels
{
    public partial class ConfigurationButtonViewModel : IConfigurationItem
    {
        private ConfigurationButton ConfigButton { get; set; }
        public ICommand Command => ConfigButton.Command;
        public string Name => ConfigButton.Name;
        public string Description => ConfigButton.Description;
        public string Key => ConfigButton.Name.Replace(" ", "");
        public FontIcon Icon => ConfigButton.Icon;
        public ConfigurationType Type => ConfigButton.Type;

        public ConfigurationButtonViewModel(ConfigurationButton configurationButton)
        {
            ConfigButton = configurationButton;
        }

        [RelayCommand]
        public void ExecuteCommand()
        {
            Command.Execute(this);
        }
    }
}
