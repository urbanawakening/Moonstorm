using BepInEx;
using BepInEx.Configuration;
using Cloudburst.Cores;
using Cloudburst.Cores.HAND;
using R2API.Utils;
using System;
using System.Reflection;

namespace Cloudburst
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
        "OrbAPI"

    })]

    public class Cloudburst : BaseUnityPlugin
    {
        const string guid = "com.AwokeinanEngima.Cloudburst";
        const string modName = "Cloudburst";
        const string version = "1.0.0";

        public static Cloudburst instance;

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
        public static ConfigEntry<bool> Enabled;
        #endregion
        public Cloudburst()
        {
            LogCore.logger = Logger;

            DefineConfig();
            InitializeCores();

            LogCore.LogM("Cloudburst loaded!");
        }

        private void DefineConfig() {            
            EnableHAND = Config.Bind("Cloudburst :: HAN-D", "Enabled", true, "Enables HAN-D as a survivor. Set to false to disable HAN-D.");
            EnableClaymen = Config.Bind("Cloudburst :: Clay Men", "Enabled", true, "Enables Clay Men as enemies. Set to false to disable Clay Men.");
            EnableSipSip = Config.Bind("Cloudburst :: SipSip", "Enabled", true, "Enables SipSip as a potential 5th lunar scavenger. Set to false to disable SipSip.");
            EnableArchaicWisps = Config.Bind("Cloudburst :: Archaic Wisps", "Enabled", true, "Enables Archaic Wisps as enemies. Set to false to disable Arcahic Wisps.");
            EnableElites = Config.Bind("Cloudburst :: Elites", "Enabled", true, "Enables custom elites. Set to false to disable Cloudburst's elites.");
            EnableRex = Config.Bind("Cloudburst :: REX", "Enabled", true, "Enables Cloudburst's modifications to REX. Set to false to disable Cloudburst's modifications to REX.");
            EnableCommando = Config.Bind("Cloudburst :: Commando", "Enabled", true, "Enables Cloudburst's modifications to Commando. Set to false to disable Cloudburst's modifications to Commando.");
            EnableEngineer = Config.Bind("Cloudburst :: Engineer", "Enabled", true, "Enables Cloudburst's modifications to Engineer. Set to false to disable Cloudburst's modifications to Engineer.");
            EnableItems = Config.Bind("Cloudburst :: Items", "Enabled", true, "Enables Cloudburst's items. Set to false to disable Cloudburst's items.");
            EnableEquipment = Config.Bind("Cloudburst :: Equipment", "Enabled", true, "Enables Cloudburst's equipment. Set to false to disable Cloudburst's equipment.");
            EnableWIP = Config.Bind("Cloudburst :: WIP", "Enabled", true, "WARNING: CONTENT ADDED BY THIS MODULE MAY NOT BE STABLE. ENABLE AT YOUR OWN RISK! Enables Cloudburst's WIP (work in progress) content. Set to false to disable Cloudburst's WIP content.");
            Enabled = Config.Bind("Cloudburst", "Enabled", true, "Enables the mod. Set to false to disable the mod entirely.");
        }

        private void InitializeCores() {
            if (Enabled.Value)
            {
                //if i allow users to disable these cores they're gonna be stupid
                //so i can't do that
                //plus most cores rely on these
                assetCore = new AssetsCore();
                orbCore = new OrbCore();
                buffCore = new BuffCore();
                effectCore = new EffectCore();
                dotCore = new DoTCore();

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
                ancientWisp = new AncientWispCore(); ;

                //artifactCore = new ArtifactsCore();


#if DEBUG
                //unlock!
                debugCore = new DebuggingCore();
#endif
            }
            else {
                LogCore.LogW("You have disabled ALL of Cloudburst. If this was not desired, you can re-enable it in Cloudburst's config file.");
            }
        }

        public void Awake()
        {
            Action awake = Cloudburst.awake;
            if (awake == null)
            {
                return;
            }
            awake();
        }

        public void Start()
        {
            Action awake = Cloudburst.start;
            if (awake == null)
            {
                return;
            }
            awake();
        }

        public void OnEnable() {
            SingletonHelper.Assign<Cloudburst>(Cloudburst.instance, this);
            Action awake = Cloudburst.onEnable;
            if (awake == null)
            {
                return;
            }
            awake();
        }

        public void OnDisable()
        {
            SingletonHelper.Unassign<Cloudburst>(Cloudburst.instance, this);
            Action awake = Cloudburst.onDisable;
            if (awake == null)
            {
                return;
            }
            awake();
        }
    }
}
