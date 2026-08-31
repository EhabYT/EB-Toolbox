using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace EBToolbox.Utils
{
    public static class ThemeHelper
    {
        private const string ThemeKey = "ThemeMode";
        private const string RegistryPath = @"HKLM\SOFTWARE\EBOS\Services\Toolbox";

        public static ElementTheme GetSavedTheme()
        {
            try
            {
                var value = RegistryHelper.GetValue(RegistryPath, ThemeKey);
                if (value is int intValue)
                {
                    return intValue switch
                    {
                        0 => ElementTheme.Light,
                        1 => ElementTheme.Dark,
                        _ => ElementTheme.Light
                    };
                }
            }
            catch { }
            return ElementTheme.Light;
        }

        public static void SetTheme(ElementTheme theme)
        {
            int value = theme switch
            {
                ElementTheme.Light => 0,
                ElementTheme.Dark => 1,
                _ => 0
            };
            RegistryHelper.SetValue(RegistryPath, ThemeKey, value, Microsoft.Win32.RegistryValueKind.DWord);
        }

        public static void ApplyTheme(ElementTheme theme)
        {
            if (App.Current is Application app)
            {
                app.RequestedTheme = theme switch
                {
                    ElementTheme.Light => ApplicationTheme.Light,
                    ElementTheme.Dark => ApplicationTheme.Dark,
                    _ => ApplicationTheme.Light
                };
            }
        }

        public static void ApplySavedTheme()
        {
            var theme = GetSavedTheme();
            ApplyTheme(theme);
        }

        public static int ThemeToIndex(ElementTheme theme)
        {
            return theme switch
            {
                ElementTheme.Light => 0,
                ElementTheme.Dark => 1,
                _ => 0
            };
        }

        public static ElementTheme IndexToTheme(int index)
        {
            return index switch
            {
                0 => ElementTheme.Light,
                1 => ElementTheme.Dark,
                _ => ElementTheme.Light
            };
        }
    }
}
