using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using BepInEx.Logging;
//using Cloudburst.Achievements;
using Cloudburst.Enemies;
using Cloudburst.Equipment;
using Cloudburst.Items;
using Cloudburst.Misc;
using R2API;
using R2API.Utils;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cloudburst
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInIncompatibility("")]
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
        "UnlockablesAPI"
    })]
    [BepInPlugin(guid, modName, version)]
    public class Main : BaseUnityPlugin
    {
        const string guid = "com.AwokeinanEngima.CloudBurst";
        const string modName = "Cloudburst";
        const string version = "1.0.0";

        public BuffIndex olLoppin;

        public Item items;
        public Equip equipment;
        public SipSip sipSip;
        public ClayMan clayMan;
        public MegaMushrumMonster megaMushrum;
        public Hooks hooks;

        public static Main instance;
        public static ManualLogSource logger;

        private List<string> replacedFuncs;
        private List<PluginInfo> loadedPlugins;

        private bool archaicWispMod;
        private bool deathMarkMod;
        private bool solidIceMod;

        public DifficultyIndex cloudburst;
        public DifficultyDef cloudburstDef;

        public static event Action awake;
        //internal event Action afterLoad;
        public static event Action start;

        //public static ConfigEntry<bool> unlockAll;
        public Main() {
            awake += Load;
            items = new Item();
            equipment = new Equip();
            sipSip = new SipSip();
            clayMan = new ClayMan();
            megaMushrum = new MegaMushrumMonster();
            hooks = new Hooks();
        }
        #region Basic
        public void Awake()
        {
            Action awake = Main.awake;
            if (awake == null)
            {
                return;
            }
            awake();
        }
        public void Start() {
            Action start = Main.start;
            if (start == null) {
                return;
            }
            start();
        }
        #endregion
        private void Load() {
            if (instance == null || logger == null)
            {
                instance = this;
                logger = this.Logger;
            }
            //CreateConfig();

            Assets.PopulateAssets();
            Assets.Resize();
            CheckLoadedPlugins();
            CheckCompat();
            AddContent();
            ModifyContent();
            MiscModifications.Modify();
            Hook();

            /*var allCSC = Resources.LoadAll<CharacterSpawnCard>("SpawnCards/CharacterSpawnCards");
            for (int i = 0; i < allCSC.Length; i++) {
                BaseLogger.Log(allCSC[i].name + ": " + allCSC[i].directorCreditCost.ToString());
            }*/
        }
        /*private void CreateConfig() {
            unlockAll = Config.Bind<bool>(new ConfigDefinition("General", "UnlockAll"), false, new ConfigDescription("While Cloudburst is much more fun with unlockables, I understand that many might not want to spend time unlocking items and such. Therefore, I have created this config option so those who do not want to spend time unlocking content can just unlock everything right away.", null, Array.Empty<object>()));
        }*/
        public static int GetIQFromUser(string name) {
            int IQ;
            switch (name) {
                default:
                    LogE("Person not found!");
                    IQ = 0;
                    break;
                case "Vulfoof":
                    LogM("Fuckin' brainlet lmao.");
                    IQ = int.MinValue;
                    break; 
                case "Enigma":
                    LogM("Huge brain, fuckin' GENIUS!");
                    IQ = int.MaxValue;
                    break; 
                case "ᴬ ˢᵐᵃˡˡ ᴮᵉʳ":
                    LogM("Fuck your unicode-ass name.");
                    IQ = 125;
                    break; 
                case "duck duck grey duck":
                    LogM("Thank you for not using unicode in your username.");
                    IQ = 145;
                    break; 
            }
            return IQ;
        }
        private void Hook() {

            if (!deathMarkMod)
            {
                On.RoR2.BuffCatalog.RegisterBuff += Hooks.BuffCatalog_RegisterBuff;
            }
            else
            {
                LogW("DeathMarkFix is installed, you can uninstall it for lesser loading times as its functionality is in cloudburst.");
            }
        }
        private void AddAchivements() {
            /*LanguageAPI.Add("CLOUDBURST_GRABORDIE_UNLOCKABLE_NAME", "Grab or Die!");
            LanguageAPI.Add("CLOUDBURST_GRABORDIE_ACHIEVEMENT_NAME", "Grab or Die!");
            LanguageAPI.Add("CLOUDBURST_GRABORDIE_ACHIEVEMENT_DESC", "Grab an item while below 30% health.");

            UnlockablesAPI.AddUnlockable<GrabOrDie>(false);*/
        }
        private void AddContent() {
            if (!archaicWispMod) {
                ArchaicWisps.BuildArchWisps();
            }
            else {
                LogE("ReinDirectorCardDemoArchWisp is installed, please uninstall it to have Cloudburst's Archaic Wisp enemy spawn.");
            }
            AddAchivements();
            CreateBuff();   
        }
        private void ModifyContent() {
            Modifications.MakeDeskPlantBetter();
            if (!solidIceMod) {
                Modifications.MakeIceWallSolid();
            }
            else {
                LogE("SolidIceWall is installed, you can uninstall it for lesser loading times as its functionality is in cloudburst.");
            }
            Modifications.ModifyPlasmaBolt();
            Modifications.RenameDaisy();
            ArtificerExtended();
        }
        private void CreateBuff() {
            BuffDef olLoppinDef = new BuffDef
            {
                buffIndex = BuffIndex.Count,
                buffColor = Color.yellow,
                canStack = true,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/TexMovespeedBuffIcon",
                isDebuff = false,
                name = "OlLopping"
            };
            olLoppin = BuffAPI.Add(new CustomBuff(olLoppinDef));
        }
        private void CheckLoadedPlugins() {
            loadedPlugins = new List<PluginInfo>();
            foreach (PluginInfo plugin in Chainloader.PluginInfos.Values) {
                if (plugin != null)
                {
                    loadedPlugins.Add(plugin);
                }
            }
        }
        private void ArtificerExtended()
        {
            //would you give up your humanity for just a touch of sanity?
            /*GameObject mage = Resources.Load<GameObject>("prefabs/characterbodies/MageBody");



            Transform mdlMage = mage.GetComponent<ModelLocator>().modelTransform;
            ChildLocator childLocator = mdlMage.GetComponentInChildren<ChildLocator>();
            int headIndex = childLocator.FindChildIndex("Head");
            Transform head = childLocator.FindChild(headIndex);
            if (head)
            {
                LogD("FUCK HOPOO");
                head.localScale *= 10;
                GameObject headObject = head.gameObject;
                headObject.GetComponent<Rigidbody>().mass *= 100;
            }*/
            /*if (armature) {
                Transform root = armature.Find("ROOT");
                LogD("magearmature");
                if (root) {
                    LogD("mageroot");
                    //FUCK CSHARP//
                    Transform _base = root.Find("base");
                    if (_base) {
                        LogD("magebase");
                        Transform stomach = _base.Find("stomach");
                        if (stomach) {
                            LogD("magestomach");
                            Transform chest = stomach.Find("chest");
                            if (chest) {
                                LogD("magechest");
                                Transform neck = chest.Find("neck");
                                if (neck) {
                                    LogD("mageneck");
                                    Transform head = neck.Find("head");
                                    if (head) {
                                        LogD("so no head?");
                                        head.localScale = head.localScale * 5;
                                    }
                                }
                            }
                        }
                    }
                }
            }*/
        }
        private void CheckCompat() {
            replacedFuncs = new List<string>();
            this.replacedFuncs.Add("com.Rein.ReinDirectorCardDemoArchWisp"); //rein i BEG YOU please shorten that name PLEASE
            foreach (PluginInfo pluginInfo in loadedPlugins) {
                switch ((pluginInfo.Metadata.GUID))
                {
                    case "com.Borbo.DeathMarkFix":
                        deathMarkMod = true;
                        break; //done
                    case "com.ThunderDownUnder.SolidIceWall":
                        solidIceMod = true;
                        break; //done                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        case i don't get how to handle,
                    case "com.Rein.ReinDirectorCardDemoArchWisp":
                        archaicWispMod = true;
                        break; //done
                }
                //oh no, i'm checking plugins for incompats, how evil i am!
                if (this.replacedFuncs.Contains(pluginInfo.Metadata.GUID)) {
                    LogW("Cloudburst may have conflicts with the following mod:" + pluginInfo.Metadata.Name + ". Please disable this mod, as it's functionality is in Cloudburst.");
                    LogW("If you do not choose to disable these mod(s), Cloudburst will disable certain features to provide a bug free experenice at the cost of content.");
                }
            }
        }
        public static void LogF(object data) => logger.LogFatal(data);
        public static void LogE(object data) => logger.LogError(data);
        public static void LogW(object data) => logger.LogDebug(data);
        public static void LogM(object data) => logger.LogMessage(data);
        public static void LogI(object data) => logger.LogInfo(data);
        public static void LogD(object data) => logger.LogDebug(data);
    }
}