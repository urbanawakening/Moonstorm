using R2API;
using System.Reflection;
using UnityEngine;

namespace Cloudburst
{
    public static class Assets
    {
        public static AssetBundle mainAssetBundle = null;
        public static AssetBundleResourcesProvider provider;

        // icons
        public static GameObject grinderModel;
        public static void PopulateAssets()
        {
            // populate ASSETS
            if (mainAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Cloudburst.assetburst"))
                {
                    mainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                    provider = new AssetBundleResourcesProvider("@Cloudburst", mainAssetBundle);
                    ResourcesAPI.AddProvider(provider);
                }
            }
            // gather assets
            grinderModel = mainAssetBundle.LoadAsset<GameObject>("Assets/Import/belt/belt.prefab");
        }
        public static void Resize() {
            grinderModel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }
}