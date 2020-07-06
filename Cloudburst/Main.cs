using BepInEx;
using BepInEx.Bootstrap;
using Cloudburst.Enemies;
using Cloudburst.Misc;
using R2API.Utils;
using RoR2;
using System;
using System.Collections.Generic;
using static RoR2.GivePickupsOnStart;

namespace Cloudburst
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
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
        "ResourcesAPI"
    })]
    [BepInPlugin("com.AwokeinanEngima.CloudBurst", "CloudBurst", "1.0.0")]
    public class Main : BaseUnityPlugin
    {
        public ItemClass items;
        public Equipment equipment;
        public static Main instance;

        private List<string> replacedFuncs;
        private List<PluginInfo> loadedPlugins;

        private bool archaicWispMod;
        private bool deathMarkMod;
        private bool solidIceMod;

        public static event Action awake;
        //internal event Action afterLoad;
        public static event Action start;
        public Main() {
            awake += Load;
            items = new ItemClass();
            equipment = new Equipment();
        }
        #region Basic
        public void Awake() {
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
            if (instance == null || BaseLogger.logger == null)
            {
                instance = this;
                BaseLogger.logger = base.Logger;
            }
            Assets.PopulateAssets();
            Assets.Resize();
            CheckLoadedPlugins();
            CheckCompat();
            AddContent();
            ModifyContent();
            MiscModifications.Modify();
            Hook();

            if (!deathMarkMod) {
                On.RoR2.BuffCatalog.RegisterBuff += Hooks.BuffCatalog_RegisterBuff;
            }
            else {
                BaseLogger.LogE("DeathMarkFix is installed, you can uninstall it for lesser loading times as its functionality is in cloudburst.");
            }

            //On.RoR2.ArenaMissionController.EndRound += ArenaMissionController_EndRound;
        }

        private void Hook() {
            //GlobalEventManager.OnInteractionsGlobal += Hooks.GlobalEventManager_OnInteractionsGlobal;
            On.RoR2.GlobalEventManager.OnCrit += Hooks.GlobalEventManager_OnCrit;
            On.RoR2.GlobalEventManager.OnInteractionBegin += Hooks.GlobalEventManager_OnInteractionBegin;
            GlobalEventManager.onServerDamageDealt += Hooks.GlobalEventManager_onServerDamageDealt;
            GlobalEventManager.onCharacterDeathGlobal += Hooks.GlobalEventManagerOnOnCharacterDeath;
            On.RoR2.GlobalEventManager.OnTeamLevelUp += Hooks.GlobalEventManager_OnTeamLevelUp;
            TeleporterInteraction.onTeleporterBeginChargingGlobal += Hooks.TeleporterInteractionOnTeleporterBeginChargingGlobal;
            On.RoR2.GenericPickupController.GrantItem += Hooks.GrantItem;
            On.RoR2.EquipmentSlot.PerformEquipmentAction += Hooks.EquipmentSlotOnPerformEquipmentAction ;
            On.EntityStates.Commando.DodgeState.OnEnter += Hooks.DodgeState_OnEnter;
            On.EntityStates.Barrel.Opened.OnEnter += Hooks.Opened_OnEnter;
        }
        private void AddContent() {
            if (!archaicWispMod) {
                ArchaicWisps.BuildArchWisps();
            }
            else {
                BaseLogger.LogE("ReinDirectorCardDemoArchWisp is installed, please uninstall it to have Cloudburst's Archaic Wisp enemy spawn.");
            }
            MegaMushrumMonster.BuildMegaMushrums();
            ClayMan.BuildClayMen();
        }
        private void ModifyContent() {
            Modifications.MakeDeskPlantBetter();
            if (!solidIceMod) {
                Modifications.MakeIceWallSolid();
            }
            else {
                BaseLogger.LogE("SolidIceWall is installed, you can uninstall it for lesser loading times as its functionality is in cloudburst.");
            }
            Modifications.ModifyPlasmaBolt();
            Modifications.RenameDaisy();
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
                    BaseLogger.LogW("Cloudburst may have conflicts with the following mod:" + pluginInfo.Metadata.Name + ". Please disable this mod, as it's functionality is in Cloudburst.");
                    BaseLogger.LogW("If you do not choose to disable these mod(s), Cloudburst will disable certain features to provide a bug free experenice at the cost of content.");
                }
            }
        }
    }
}