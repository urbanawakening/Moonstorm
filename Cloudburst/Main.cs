using BepInEx;
using BepInEx.Bootstrap;
using CloudBurst.Enemies;
using CloudBurst.Equipment;
using CloudBurst.Items;
using CloudBurst.Misc;
using R2API.Utils;
using RoR2;
using System;
using System.Collections.Generic;

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
    })]
    [BepInPlugin("com.AwokeinanEngima.CloudBurst", "CloudBurst", "1.0.0")]
    public class Main : BaseUnityPlugin
    {
        public Items items;
        public Equipment equipment;
        public static Main instance;

        private List<string> replacedFuncs;
        private List<PluginInfo> loadedPlugins;

        private bool archaicWispMod;
        public bool deathMarkMod;
        private bool solidIceMod;

        internal event Action awake;
        internal event Action start;
        public Main()
        {
            awake += Load;
            items = new Items();
            equipment = new Equipment();
        }
        #region Basic
        public void Awake()
        {
            Action awake = this.awake;
            if (awake == null) {
                return;
            }
            awake();
        }
        public void Start() {
            Action start = this.start;
            if (start == null) {
                return;
            }
            start();
        }
        #endregion
        private void Load() {
            if (instance == null || BaseLogger.logger == null) {
                instance = this;
                BaseLogger.logger = base.Logger;
            }
            CheckLoadedPlugins();
            CheckCompat();
            AddContent();
            ModifyContent();

            On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
            if (!deathMarkMod)
            {
                On.RoR2.BuffCatalog.RegisterBuff += Hooks.BuffCatalog_RegisterBuff;
            }
            else {
                BaseLogger.LogE("DeathMarkFix is installed, you can uninstall it for lesser loading times as its functionality is in cloudburst.");
            }
            On.RoR2.EquipmentSlot.PerformEquipmentAction += Hooks.EquipmentSlotOnPerformEquipmentAction;
            On.EntityStates.Commando.DodgeState.OnEnter += Hooks.DodgeState_OnEnter;
            On.EntityStates.Barrel.Opened.OnEnter += Hooks.Opened_OnEnter;
            //On.RoR2.ArenaMissionController.EndRound += ArenaMissionController_EndRound;
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
        private void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self) {
            if (self && self.inventory)
            {
                Inventory inventory = self.inventory;

                #region Item Counts
                int barrierOnCritCount = inventory.GetItemCount(Items.barrierOnCritIndex); //done
                int moneyOnInteractionCount = inventory.GetItemCount(Items.moneyOnInteractionIndex); //done
                int eliteBuffOnInteractionCount = inventory.GetItemCount(Items.eliteBuffOnInteractionIndex); //done
                int itemChampionOnKillCount = inventory.GetItemCount(Items.itemChampionOnKillIndex); //done
                int crippleOnHitCount = inventory.GetItemCount(Items.crippleOnHitIndex); //done
                int randomDebuffOnHitCount = inventory.GetItemCount(Items.randomDebuffOnHitIndex); //done
                int itemOnLevelUpCount = inventory.GetItemCount(Items.itemOnLevelUpIndex); //done
                int cloakOnInteractionCount = inventory.GetItemCount(Items.cloakOnInteractionIndex); //done
                int largerTeleporterRadius = inventory.GetItemCount(Items.largerTeleporterRadiusIndex);//done
                int barrierOnLevelCount = inventory.GetItemCount(Items.barrierOnLevelIndex); //done
                #endregion

                #region Hooks
                if (moneyOnInteractionCount > 0 || eliteBuffOnInteractionCount > 0) {
                    GlobalEventManager.OnInteractionsGlobal += Hooks.GlobalEventManager_OnInteractionsGlobal;
                }
                else if (Hooks.onInteractionSubscription) {
                    GlobalEventManager.OnInteractionsGlobal -= Hooks.GlobalEventManager_OnInteractionsGlobal;
                }
                if (barrierOnCritCount > 0) {
                    On.RoR2.GlobalEventManager.OnCrit += Hooks.GlobalEventManager_OnCrit;
                }
                else if (Hooks.onCritSubscription) {
                    On.RoR2.GlobalEventManager.OnCrit -= Hooks.GlobalEventManager_OnCrit;
                }
                if (crippleOnHitCount > 0 || randomDebuffOnHitCount > 0) {
                    GlobalEventManager.onServerDamageDealt += Hooks.GlobalEventManager_onServerDamageDealt;
                }
                else if (Hooks.onDmgDealtSubscription) {
                    GlobalEventManager.onServerDamageDealt-= Hooks.GlobalEventManager_onServerDamageDealt;
                }
                if (itemChampionOnKillCount > 0) {
                    GlobalEventManager.onCharacterDeathGlobal += Hooks.GlobalEventManagerOnOnCharacterDeath;
                }
                else if (Hooks.onDeathSubscription) {
                    GlobalEventManager.onCharacterDeathGlobal -= Hooks.GlobalEventManagerOnOnCharacterDeath;
                }
                if (itemOnLevelUpCount > 0 ||  barrierOnLevelCount > 0) {
                    On.RoR2.GlobalEventManager.OnTeamLevelUp += Hooks.GlobalEventManager_OnTeamLevelUp;
                }
                else if (Hooks.onTeamLevelUpSubscription) {  
                    On.RoR2.GlobalEventManager.OnTeamLevelUp -= Hooks.GlobalEventManager_OnTeamLevelUp;
                }
                if (largerTeleporterRadius > 0) {
                    TeleporterInteraction.onTeleporterBeginChargingGlobal += Hooks.TeleporterInteractionOnTeleporterBeginChargingGlobal;
                }
                else if (Hooks.onTeleporterBeginChargingSubscription) {
                    TeleporterInteraction.onTeleporterBeginChargingGlobal -= Hooks.TeleporterInteractionOnTeleporterBeginChargingGlobal;
                }
                if (cloakOnInteractionCount > 0) {
                    On.RoR2.GenericPickupController.GrantItem += Hooks.GrantItem;
                }
                else if (Hooks.grantItemSubscription) {
                    On.RoR2.GenericPickupController.GrantItem -= Hooks.GrantItem;
                }
                #endregion
            }
            orig(self);
        }
    }
}