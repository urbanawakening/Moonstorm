using R2API;
using System.Reflection;
using UnityEngine;

namespace Moonstorm.Cores
{
    class AssetsCore
    {
        //yeah no instance 
        public static AssetBundle mainAssetBundle = null;
        public static AssetBundleResourcesProvider provider;

        public static AssetBundle HANDAssetBundle = null;
        public static AssetBundleResourcesProvider HANDProvider;

        // icons
        public static GameObject grinderModel;

        #region HAN-D
        public static Sprite winchIcon = null;
        public static Texture2D portraitIcon = null;
        public static Sprite droneIcon = null;
        public static Sprite forcedReassemblyIcon = null;
        public static Sprite overclockBuffIcon = null;
        public static Sprite passiveIcon = null;
        public static Sprite hurtIcon = null;
        public static Sprite overclockIcon = null;
        public static Sprite explungeIcon = null;
        public static Sprite passiveDroneBuffIcon = null;
        public static Sprite unethicalReassemblyIcon = null;
        public static Sprite passive = null;
        #endregion

        public static Sprite rexHarvester;
        public AssetsCore()
        {
            LogCore.LogI("Initializing Core: " + base.ToString());

            // populate ASSETS
            if (mainAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Moonstorm.assetburst"))
                {
                    mainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                    provider = new AssetBundleResourcesProvider("@Moonstorm", mainAssetBundle);
                    ResourcesAPI.AddProvider(provider);
                }
            }
            if (HANDAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Moonstorm.hand_assets"))
                {
                    HANDAssetBundle = AssetBundle.LoadFromStream(assetStream);
                    HANDProvider = new AssetBundleResourcesProvider("@EngimaHANDREBOOTED", HANDAssetBundle);
                    ResourcesAPI.AddProvider(HANDProvider);
                }
            }
            // gather assets
            grinderModel = mainAssetBundle.LoadAsset<GameObject>("Assets/Items/Grinder/MDLGrinder.prefab");
            rexHarvester = mainAssetBundle.LoadAsset<Sprite>("Assets/MiscIcons/texTreebotHarvester.png");
            
            //HAN-D
            winchIcon = HANDAssetBundle.LoadAsset<Sprite>("Assets/Import/HAND_ICONS/Winch.png");
            portraitIcon = HANDAssetBundle.LoadAsset<Texture2D>("Assets/Import/HAND_ICONS/Portrait.png");
            droneIcon = HANDAssetBundle.LoadAsset<Sprite>("Assets/Import/HAND_ICONS/Drone.png");
            forcedReassemblyIcon = HANDAssetBundle.LoadAsset<Sprite>("Assets/Import/HAND_ICONS/Forced_Reassembly.png");
            overclockBuffIcon = HANDAssetBundle.LoadAsset<Sprite>("Assets/Import/HAND_ICONS/OverclockBuff.png");
            passiveIcon = HANDAssetBundle.LoadAsset<Sprite>("Assets/Import/HAND_ICONS/Passive.png");
            hurtIcon = HANDAssetBundle.LoadAsset<Sprite>("Assets/Import/HAND_ICONS/Hurt.png");
            overclockIcon = HANDAssetBundle.LoadAsset<Sprite>("Assets/Import/HAND_ICONS/Overclock.png");
            explungeIcon = HANDAssetBundle.LoadAsset<Sprite>("Assets/Import/HAND_ICONS/Explunge.png");
            passiveDroneBuffIcon = HANDAssetBundle.LoadAsset<Sprite>("Assets/Import/HAND_ICONS/PassiveDrone.png");
            unethicalReassemblyIcon = HANDAssetBundle.LoadAsset<Sprite>("Assets/Import/HAND_ICONS/Unethical_Reassembly.png");
            passive = HANDAssetBundle.LoadAsset<Sprite>("Assets/Import/HAND_ICONS/Passive.png");
        }
    }
}
