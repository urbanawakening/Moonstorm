using R2API;
using System.Reflection;
using UnityEngine;

namespace Starstorm2.Cores
{
    class AssetsCore
    {
        //yeah no instance 
        public static AssetBundle mainAssetBundle = null;
        public static AssetBundleResourcesProvider provider = null;

        public AssetsCore()
        {
            LogCore.LogI("Initializing Core: " + base.ToString());

            // populate ASSETS
            if (mainAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Starstorm2.assetstorm"))
                {
                    mainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                    provider = new AssetBundleResourcesProvider("@Starstorm2", mainAssetBundle);
                    ResourcesAPI.AddProvider(provider);
                }
            }
        }
    }
}
