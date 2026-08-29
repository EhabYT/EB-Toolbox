using System;
using System.Threading;
using System.Threading.Tasks;
using EBToolbox.Utils;
using EBToolbox.Views;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MVVMEssentials.Commands;

namespace EBToolbox.Commands.ConfigurationButtonsCommand
{
    public class SetUpdateDeferralConfigurationButton : AsyncCommandBase
    {
        protected override async Task ExecuteAsync(object parameter)
        {
            UpdateDeferralPage page = new UpdateDeferralPage();
            await Task.Run(() =>
            {
                // stupid hack idk why microsoft doesn't allow us to do a DispatcherQueue.EnqueueAsync
                // in things other than pages and whatnot
                page.ShowUpdateDeferralPrompt();
            });
        }

        public static void SetUpdateDeferral(int featureDays, int qualityDays)
        {
            const string WINDOWS_UPDATE_KEY = "HKLM\\SOFTWARE\\Policies\\Microsoft\\Windows\\WindowsUpdate";
            
            RegistryHelper.SetValue(WINDOWS_UPDATE_KEY, "DeferFeatureUpdates", 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(WINDOWS_UPDATE_KEY, "DeferFeatureUpdatesPeriodInDays", featureDays, Microsoft.Win32.RegistryValueKind.DWord);

            RegistryHelper.SetValue(WINDOWS_UPDATE_KEY, "DeferQualityUpdates", 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(WINDOWS_UPDATE_KEY, "DeferQualityUpdatesPeriodInDays", qualityDays, Microsoft.Win32.RegistryValueKind.DWord);

            RegistryHelper.SetValue("HKLM\\SOFTWARE\\EBOS\\FeatureUpdateDeferrals", "state", 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue("HKLM\\SOFTWARE\\EBOS\\FeatureUpdateDeferrals", "path", "C:\\Windows\\EBDesktop\\3. General Configuration\\Windows Updates\\Set Windows Update Deferral.cmd", Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue("HKLM\\SOFTWARE\\EBOS\\FeatureUpdateDeferrals", "value", featureDays, Microsoft.Win32.RegistryValueKind.DWord);
                                                                                        
            RegistryHelper.SetValue("HKLM\\SOFTWARE\\EBOS\\QualityUpdateDeferrals", "state", 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue("HKLM\\SOFTWARE\\EBOS\\QualityUpdateDeferrals", "path", "C:\\Windows\\EBDesktop\\3. General Configuration\\Windows Updates\\Set Windows Update Deferral.cmd", Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue("HKLM\\SOFTWARE\\EBOS\\QualityUpdateDeferrals", "value", qualityDays, Microsoft.Win32.RegistryValueKind.DWord);
        }
    }
}
