using BepInEx;
using BepInEx.Configuration;
using Moonstorm.Cores;
using Moonstorm.Cores.Components;
using Moonstorm.Cores.HAND;
using R2API.Utils;
using System;

namespace Moonstorm
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
        "DirectorAPI",
        "LanguageAPI",
        "ResourcesAPI",
        "UnlockablesAPI",
        "DotAPI",
        "EliteAPI",
        //gotta stop using this one, orbCore should be able to support itself.
        "OrbAPI"
    })]

    public class Moonstorm : BaseUnityPlugin
    {
        //be unique, though you're the same here.

        const string guid = "com.TeamMoonstorm.Moonstorm";
        const string modName = "Moonstorm";
        const string version = "1.0.0";

        public static Moonstorm instance;

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


        #region Cores
        //Cupport cores; Cannot be disabled.
        private AssetsCore assetCore;
        private OrbCore orbCore;
        private BuffCore buffCore;
        private DoTCore dotCore;
        private EffectCore effectCore;
        private DebuggingCore debugCore;

        //Content Cores; Can be disabled
        private ItemCore itemCore;
        private EquipmentCore equipCore;
        private EliteCore eliteCore;
        private HAN_DCore han_dCore;
        private EngineerCore engineerCore;
        private QOLCore qol;
        private CommandoCore commandoCore;
        private AncientWispCore ancientWisp;
        private ClayMenCore clayCore;
        private SipSipCore sipsipCore;
        private ArchaicWispCore archwispCore;
        private RexCore rexCore;
        private HuntressCore wipCore;
        private BombardierCore bombManCore;
        private AchievementCore achiveCore;
        private ProjectileCore projectileCore;
        private DamageTypeCore damageTypeCore;
        //shut up vs
        //private ArtifactsCore artifactCore;
        #endregion

        #region Config
        public static ConfigEntry<bool> EnableHAND;
        public static ConfigEntry<bool> EnableClaymen;
        public static ConfigEntry<bool> EnableSipSip;
        public static ConfigEntry<bool> EnableArchaicWisps;
        public static ConfigEntry<bool> EnableElites;
        public static ConfigEntry<bool> EnableCommando;
        public static ConfigEntry<bool> EnableEngineer;
        public static ConfigEntry<bool> EnableRex;
        public static ConfigEntry<bool> EnableItems;
        public static ConfigEntry<bool> EnableEquipment;

        public static ConfigEntry<bool> EnableWIP;
        public static ConfigEntry<bool> EnableUnlockAll;
        public static ConfigEntry<bool> Enabled;
        #endregion
        public Moonstorm()
        {
            LogCore.logger = Logger;

            DefineConfig();
            InitializeCores();

            if (!gameObject.HasComponent<ProgressTracker>() || !ProgressTracker.instance) {
                LogCore.LogI("Bepinex object does not have a progress tracker or there is no instance of the progress tracker. Attaching now.");
                gameObject.AddComponent<ProgressTracker>();
            }
            

            LogCore.LogM("Moonstorm loaded!");
        }

        private void DefineConfig() {        
            //learn a smile
            //as every good day goes on by
            EnableHAND = Config.Bind("Moonstorm :: HAN-D", "Enabled", true, "Enables HAN-D as a survivor. Set to false to disable HAN-D.");
            EnableClaymen = Config.Bind("Moonstorm :: Clay Men", "Enabled", true, "Enables Clay Men as enemies. Set to false to disable Clay Men.");
            EnableSipSip = Config.Bind("Moonstorm :: SipSip", "Enabled", true, "Enables SipSip as a potential 5th lunar scavenger. Set to false to disable SipSip.");
            EnableArchaicWisps = Config.Bind("Moonstorm :: Archaic Wisps", "Enabled", true, "Enables Archaic Wisps as enemies. Set to false to disable Arcahic Wisps.");
            EnableElites = Config.Bind("Moonstorm :: Elites", "Enabled", true, "Enables custom elites. Set to false to disable Cloudburst's elites.");
            EnableRex = Config.Bind("Moonstorm :: REX", "Enabled", true, "Enables Cloudburst's modifications to REX. Set to false to disable Cloudburst's modifications to REX.");
            EnableCommando = Config.Bind("Moonstorm :: Commando", "Enabled", true, "Enables Cloudburst's modifications to Commando. Set to false to disable Cloudburst's modifications to Commando.");
            EnableEngineer = Config.Bind("Moonstorm :: Engineer", "Enabled", true, "Enables Cloudburst's modifications to Engineer. Set to false to disable Cloudburst's modifications to Engineer.");
            EnableItems = Config.Bind("Moonstorm :: Items", "Enabled", true, "Enables Cloudburst's items. Set to false to disable Cloudburst's items.");
            EnableEquipment = Config.Bind("Moonstorm :: Equipment", "Enabled", true, "Enables Moonstorm's equipment. Set to false to disable Moonstorm's equipment.");
            EnableWIP = Config.Bind("Moonstorm :: WIP", "Enabled", false, "WARNING: CONTENT ADDED BY THIS MODULE MAY NOT BE STABLE. ENABLE AT YOUR OWN RISK! Enables Moonstorm's WIP (work in progress) content. Set to false to disable Moonstorm's WIP content.");
            EnableUnlockAll = Config.Bind("Moonstorm :: Achivements", "Enabled", true, "Enables Moonstorm's unlocks for unlockable content. Set to false to unlock all of Moonstorm's unlockable content.");
            Enabled = Config.Bind("Moonstorm", "Enabled", true, "Enables the mod. Set to false to disable the mod entirely.");
        }

        private void InitializeCores() {
            if (Enabled.Value)
            {
                //if i allow users to disable these cores they're gonna be stupid
                //so i can't do that
                //plus most cores rely on these
                assetCore = new AssetsCore();
                CommonAssets.Load();

                damageTypeCore = new DamageTypeCore();
                orbCore = new OrbCore();
                buffCore = new BuffCore();
                effectCore = new EffectCore();
                dotCore = new DoTCore();
                projectileCore = new ProjectileCore();
                achiveCore = new AchievementCore();

                //got this feeling
                //i'm losing you
                //it's got me reeling
                //i need a clue
                if (EnableItems.Value)
                {
                    itemCore = new ItemCore();
                }
                if (EnableEquipment.Value)
                {
                    equipCore = new EquipmentCore();
                }
                if (EnableElites.Value)
                {
                    eliteCore = new EliteCore();
                }
                if (EnableHAND.Value)
                {
                    han_dCore = new HAN_DCore();
                }
                if (EnableEngineer.Value)
                {
                    engineerCore = new EngineerCore();
                }
                if (EnableCommando.Value)
                {
                    commandoCore = new CommandoCore();
                }
                if (EnableSipSip.Value)
                {
                    sipsipCore = new SipSipCore();
                }
                if (EnableClaymen.Value)
                {
                    clayCore = new ClayMenCore();
                }
                if (EnableArchaicWisps.Value)
                {
                    archwispCore = new ArchaicWispCore();
                }
                if (EnableRex.Value)
                {
                    rexCore = new RexCore();
                }
                if (EnableWIP.Value) {
                    wipCore = new HuntressCore();
                }

                //and thus
                //we reach the end
                qol = new QOLCore();
                ancientWisp = new AncientWispCore();
                bombManCore = new BombardierCore();
                //artifactCore = new ArtifactsCore();


#if DEBUG
                //unlock!
                debugCore = new DebuggingCore();
#endif
            }
            else {
                LogCore.LogW("You have disabled ALL of Moonstorm. If this was not desired, you can re-enable it in Moonstorm's config file.");
            }
        }

        public void Awake()
        {
            Action awake = Moonstorm.awake;
            if (awake == null)
            {
                return;
            }
            awake();
        }

        public void FixedUpdate()
        {
            Action fixedUpdate = Moonstorm.onFixedUpdate;
            if (fixedUpdate == null)
            {
                return;
            }
            fixedUpdate();
        }

        public void Start()
        {
            Action awake = Moonstorm.start;
            if (awake == null)
            {
                return;
            }
            awake();
        }

        public void OnEnable() {
            SingletonHelper.Assign<Moonstorm>(Moonstorm.instance, this);
            LogCore.LogI("Moonstorm instance assigned.");
            Action awake = Moonstorm.onEnable;
            if (awake == null)
            {
                return;
            }
            awake();
        }

        public void OnDisable()
        {
            SingletonHelper.Unassign<Moonstorm>(Moonstorm.instance, this);
            LogCore.LogI("Moonstorm instance unassigned.");
            Action awake = Moonstorm.onDisable;
            if (awake == null)
            {
                return;
            }
            awake();
        }
    }
}
