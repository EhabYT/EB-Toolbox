using System;
using System.Configuration;
using System.Linq;

namespace EBToolbox.Utils
{
    public class CompatibilityHelper
    {
        /// <summary>
        /// Check compatibility with app's version
        /// </summary>
        /// <returns></returns>
        public static bool IsCompatible()
        {
            // The allowlist lives in app.config under the "EBVersion" key.
            // It supports a "*" wildcard to accept every EBOS version.
            string compatibleConfig = ConfigurationManager.AppSettings.Get("EBVersion") ?? "*";
            string[] compatibleVersions = compatibleConfig
                .Split(',')
                .Select(v => v.Trim())
                .Where(v => !string.IsNullOrEmpty(v))
                .ToArray();

            string ebVersion = (string)RegistryHelper.GetValue(
                "HKLM\\SOFTWARE\\AME\\Playbooks\\Applied\\{00000000-0000-4000-6174-6C6173203A33}", "version");

            // Wildcard => accept any (or missing) EBOS version.
            if (compatibleVersions.Contains("*"))
                return true;

            // No EBOS version detected (e.g. unmanaged Windows or very old
            // EB builds) => don't block the app.
            if (string.IsNullOrWhiteSpace(ebVersion))
                return true;

            foreach (string version in compatibleVersions)
            {
                if (string.Equals(ebVersion.Trim(), version, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}
