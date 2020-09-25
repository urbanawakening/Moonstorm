using BepInEx;
using BepInEx.Configuration;

using R2API.Utils;
using System;

namespace Starstorm
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(guid, modName, version)]
    [R2APISubmoduleDependency(new string[]
    {
        "ItemAPI",
        "ResourcesAPI",
        "PrefabAPI",
        "LanguageAPI",
        "BuffAPI",
        "EffectsAPI",
        "LoadoutAPI",
        "LanguageAPI",
        "ResourcesAPI",
        "UnlockablesAPI",
        "SurvivorAPI",
    })]

    public class Starstorm : BaseUnityPlugin
    {
        //be unique, though you're the same here.

        const string guid = "com.TeamMoonstorm.Starstorm2";
        const string modName = "Starstorm 2";
        const string version = "1.0.0";

        public static Starstorm instance;

        /// <summary>
        /// Called BEFORE the first frame of the game.
        /// </summary>
        public static event Action awake;
        /// <summary>
        /// Called on the first frame of the game.
        /// </summary>
        public static event Action start;
        /// <summary>
        /// Called when the mod is enabled
        /// </summary>
        public static event Action onEnable;
        /// <summary>
        /// Called when the mod is disabled
        /// </summary>
        public static event Action onDisable;
        /// <summary>
        /// Called on the mod's FixedUpdate
        /// </summary>
        public static event Action onFixedUpdate;

        #region Config
        public static ConfigEntry<bool> EnableExecutioner;
        public static ConfigEntry<bool> EnableCyborg;
        public static ConfigEntry<bool> EnableNucleator;
        public static ConfigEntry<bool> EnableItems;

        public static ConfigEntry<bool> EnableUnlockAll;
        public static ConfigEntry<bool> Enabled;
        #endregion
        public Starstorm()
        {
            LogCore.logger = Logger;

            DefineConfig();
            InitializeCores();

            LogCore.LogM("Starstorm 2 loaded!");
        }

        private void DefineConfig()
        {
            //learn a smile
            //as every good day goes on by

            EnableItems = Config.Bind("Starstorm 2 :: Items", "Enabled", true, "Enables Starstorm 2's items. Set to false to disable Starstorm's items.");
            EnableExecutioner = Config.Bind("Starstorm 2 :: Executioner", "Enabled", true, "Enables Starstorm 2's Executioner survivor. Set to false to disable Starstorm 2's Executioner survivor.");
            EnableNucleator = Config.Bind("Starstorm 2 :: Nucleator", "Enabled", true, "Enables Starstorm 2's Nucleator survivor. Set to false to disable Starstorm 2's Nucleator survivor.");
            EnableCyborg = Config.Bind("Starstorm 2 :: Cyborg", "Enabled", true, "Enables Starstorm 2's Cyborg survivor. Set to false to disable Starstorm 2's Cyborg survivor.");
        }

        private void InitializeCores()
        {
            if (Enabled.Value)
            {

            }
            else
            {
                LogCore.LogW("You have disabled ALL of Starstorm 2. If this was not desired, you can re-enable it in Starstorm 2's config file.");
            }
        }

        public void Awake()
        {
            Action awake = Starstorm.awake;
            if (awake == null)
            {
                return;
            }
            awake();
        }

        public void FixedUpdate()
        {
            Action fixedUpdate = Starstorm.onFixedUpdate;
            if (fixedUpdate == null)
            {
                return;
            }
            fixedUpdate();
        }

        public void Start()
        {
            Action awake = Starstorm.start;
            if (awake == null)
            {
                return;
            }
            awake();
        }

        public void OnEnable()
        {
            SingletonHelper.Assign<Starstorm>(Starstorm.instance, this);
            LogCore.LogI("Starstorm instance assigned.");
            Action awake = Starstorm.onEnable;
            if (awake == null)
            {
                return;
            }
            awake();
        }

        public void OnDisable()
        {
            SingletonHelper.Unassign<Starstorm>(Starstorm.instance, this);
            LogCore.LogI("Starstorm instance unassigned.");
            Action awake = Starstorm.onDisable;
            if (awake == null)
            {
                return;
            }
            awake();
        }
    }
}
