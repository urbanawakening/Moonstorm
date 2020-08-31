using BepInEx;
using Cloudburst.Cores;
using Cloudburst.Cores.HAND;
using R2API.Utils;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        private AssetsCore assetCore;
        private OrbCore orbCore;
        private ItemCore itemCore;
        private BuffCore buffCore;
        private EquipmentCore equipCore;
        private EliteCore eliteCore;
        private DoTCore dotCore;
        private HAN_DCore han_dCore;
        private EngineerCore engineerCore;
        private QOLCore qol;
        private StatsCore stats;
        private AncientWispCore ancientWisp;
        private ClayMenCore clayCore;
        private SipSipCore sipsipCore;
        private ArchaicWispCore archwispCore;
        public Cloudburst()
        {
            LogCore.logger = Logger;

            assetCore = new AssetsCore();
            orbCore = new OrbCore();
            buffCore = new BuffCore();
            itemCore = new ItemCore();
            equipCore = new EquipmentCore();
            eliteCore = new EliteCore();
            dotCore = new DoTCore();
            han_dCore = new HAN_DCore();
            engineerCore = new EngineerCore();
            qol = new QOLCore();
            stats = new StatsCore();
            ancientWisp = new AncientWispCore();
            clayCore = new ClayMenCore();
            sipsipCore = new SipSipCore();
            archwispCore = new ArchaicWispCore();

            LogCore.LogM("Cloudburst loaded!");
        }

        /*public void FixedUpdate() { 
            bad experiment
            BaseDefenseRun.instance.nextStageScene = SceneCatalog.GetSceneDefFromSceneName("golemplains");
        }*/

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

        public void OnEnable()
        {
            Action awake = Cloudburst.onEnable;
            if (awake == null)
            {
                return;
            }
            awake();
        }

        public void OnDisable()
        {
            Action awake = Cloudburst.onDisable;
            if (awake == null)
            {
                return;
            }
            awake();
        }
    }
}
