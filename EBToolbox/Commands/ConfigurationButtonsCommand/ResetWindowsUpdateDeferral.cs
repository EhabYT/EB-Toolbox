using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using EBToolbox.Utils;
using MVVMEssentials.Commands;

namespace EBToolbox.Commands.ConfigurationButtonsCommand
{
    public class ResetWindowsUpdateDeferral : AsyncCommandBase
    {
        protected override async Task ExecuteAsync(object parameter)
        {
            await Task.Run(() =>
            {
                ResetUpdateDeferral();
            });
        }

        private void ResetUpdateDeferral()
        {
            const string WINDOWS_UPDATE_KEY = "HKLM\\SOFTWARE\\Policies\\Microsoft\\Windows\\WindowsUpdate";
            const string FEATURE_UDATE_Deferral_KEY = "HKLM\\SOFTWARE\\EBOS\\FeatureUpdateDeferrals";
            const string QUALITY_UPDATE_DeferralS_KEY = "HKLM\\SOFTWARE\\EBOS\\QualityUpdateDeferrals";

            RegistryHelper.DeleteValue(WINDOWS_UPDATE_KEY, "DeferFeatureUpdates");
            RegistryHelper.DeleteValue(WINDOWS_UPDATE_KEY, "DeferFeatureUpdatesPeriodInDays");

            RegistryHelper.DeleteValue(WINDOWS_UPDATE_KEY, "DeferQualityUpdates");
            RegistryHelper.DeleteValue(WINDOWS_UPDATE_KEY, "DeferQualityUpdatesPeriodInDays");

            RegistryHelper.SetValue(FEATURE_UDATE_Deferral_KEY, "state", 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(FEATURE_UDATE_Deferral_KEY, "path", "C:\\Windows\\EBDesktop\\3. General Configuration\\Windows Updates\\Set Windows Update Deferral.cmd", Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(FEATURE_UDATE_Deferral_KEY, "value", 0, Microsoft.Win32.RegistryValueKind.DWord);

            RegistryHelper.SetValue(QUALITY_UPDATE_DeferralS_KEY, "state", 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(QUALITY_UPDATE_DeferralS_KEY, "path", "C:\\Windows\\EBDesktop\\3. General Configuration\\Windows Updates\\Set Windows Update Deferral.cmd", Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(QUALITY_UPDATE_DeferralS_KEY, "value", 0, Microsoft.Win32.RegistryValueKind.DWord);
        }
    }
}
