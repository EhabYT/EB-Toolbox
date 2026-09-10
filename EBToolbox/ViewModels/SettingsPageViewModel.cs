using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using EBToolbox.Models;
using EBToolbox.Utils;
using EBToolbox.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;

namespace EBToolbox.ViewModels
{
    public partial class SettingsPageViewModel : INotifyPropertyChanged
    {
        public Language _currentLanguage { get; set; }
        public Language CurrentLanguage 
        {
            get => _currentLanguage;
            set
            {
                _currentLanguage = value;
                OnPropertyChanged(); // Notifies UI
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (this.CurrentLanguage is null) return;
            RegistryHelper.SetValue(@"HKLM\SOFTWARE\EBOS\Services\Toolbox", "lang", this.CurrentLanguage.Key);
            App.LoadLangString();
        }

        public ObservableCollection<Language> Languages { get; set; }

        public SettingsPageViewModel()
        {
            Languages = new();
            try
            {
                Dictionary<string, string> langs = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(@$"lang\index.json"));
                foreach (KeyValuePair<string, string> language in langs)
                {
                    Languages.Add(new (language.Value, language.Key));
                }
            }
            catch
            {
                Languages.Add(new Language("English (United States)", "en_us"));
            }
            string lang = (string)RegistryHelper.GetValue(@"HKLM\SOFTWARE\EBOS\Services\Toolbox", "lang");
            CurrentLanguage = Languages.Where(item => item.Key == lang).FirstOrDefault()
                ?? Languages.Where(item => item.Key == "en_us").FirstOrDefault()
                ?? Languages.FirstOrDefault();
        }

        public bool CheckUpdates()
        {
            if (ToolboxUpdateHelper.CheckUpdates())
            {
                App.ContentDialogCaller("newUpdate");
                return false;
            }else
            {
                return true;
            }
        }
    }
}
