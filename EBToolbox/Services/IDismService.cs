using Microsoft.Dism;

namespace EBToolbox.Services
{
    public interface IDismService
    {
        DismPackageFeatureState GetFeatureState(string featureName);
        bool IsFeatureStateMatch(string featureName, DismPackageFeatureState featureState);
        void EnableFeature(string featureName);
        void DisableFeature(string featureName);
        void RemoveProvisionedAppxPackage(string packageName);
    }
}
