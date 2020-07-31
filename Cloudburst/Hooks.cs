using Cloudburst.Equipment;
using Cloudburst.Items;
using EntityStates.Mage.Weapon;
using R2API.Utils;
using RoR2;
using RoR2.Orbs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst
{
    public sealed class Hooks
    {
        #region Subscription Bools
        public static bool onCritSubscription;
        public static bool onInteractionSubscription;
        public static bool onDeathSubscription;
        public static bool onDmgDealtSubscription;
        public static bool onTeamLevelUpSubscription;
        public static bool dodgeStateSubscription;
        public static bool onRegisterBuffSubscription;
        public static bool equipmentSlotSubscription;
        public static bool grantItemubscription;
        public static bool onTeleporterBeginChargingSubscription;
        #endregion
        #region Voids
        public List<ItemIndex> bossitemList = new List<ItemIndex>{
            ItemIndex.NovaOnLowHealth,
            ItemIndex.Knurl,
            ItemIndex.BeetleGland,
            ItemIndex.TitanGoldDuringTP,
            ItemIndex.SprintWisp,
            ItemIndex.Incubator
            //Excluding pearls because those aren't boss Item, they come from the Cleansing Pool 
        };
        public HurtBox PickRandomTarget(Vector3 position, TeamIndex teamIndex, float range)
        {
            BullseyeSearch search = new BullseyeSearch();

            search.searchOrigin = position;
            search.searchDirection = Vector3.zero;
            search.teamMaskFilter = TeamMask.allButNeutral;
            search.teamMaskFilter.RemoveTeam(teamIndex);
            search.filterByLoS = false;
            search.sortMode = BullseyeSearch.SortMode.Distance;
            search.maxDistanceFilter = range;
            search.RefreshCandidates();
            HurtBox hurtBox = (from v in search.GetResults()
                               select v).FirstOrDefault<HurtBox>();
            return hurtBox;
        }
        public List<BuffIndex> scpBuffList = new List<BuffIndex>{
            BuffIndex.NullifyStack,
            BuffIndex.ClayGoo,
            BuffIndex.BeetleJuice,
            BuffIndex.HealingDisabled
        };
        public static List<BuffIndex> eliteBuffList = new List<BuffIndex>{
            BuffIndex.AffixPoison,
            BuffIndex.AffixRed,
            BuffIndex.AffixBlue,
            BuffIndex.AffixWhite
        };
        public void NokiaCall(Transform transform, int itemCount)
        {
            var dropList = Util.CheckRoll((5 * itemCount)) ? Run.instance.availableTier2DropList : Run.instance.availableTier1DropList;
            int nextItem = Run.instance.treasureRng.RangeInt(0, dropList.Count);

            PickupDropletController.CreatePickupDroplet(dropList[nextItem], transform.position, new Vector3(0, 0, 0));

            EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/impacteffects/PodGroundImpact"), new EffectData
            {
                origin = transform.position,
                scale = 15
            }, true);
        }
        public ItemIndex GetRandomItem(List<ItemIndex> Item)
        {
            int itemID = UnityEngine.Random.Range(0, Item.Count);
            return Item[itemID];
        }
        #endregion

        public Hooks() {
            On.RoR2.GlobalEventManager.OnCrit += GlobalEventManager_OnCrit;
            On.RoR2.GlobalEventManager.OnInteractionBegin += GlobalEventManager_OnInteractionBegin;
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManagerOnOnCharacterDeath;
            On.RoR2.GlobalEventManager.OnTeamLevelUp += GlobalEventManager_OnTeamLevelUp;
            TeleporterInteraction.onTeleporterBeginChargingGlobal += TeleporterInteractionOnTeleporterBeginChargingGlobal;
            On.RoR2.GenericPickupController.GrantItem += GrantItem;
            On.RoR2.EquipmentSlot.PerformEquipmentAction += EquipmentSlotOnPerformEquipmentAction;
            On.EntityStates.Commando.DodgeState.OnEnter += DodgeState_OnEnter;
            On.EntityStates.Barrel.Opened.OnEnter += Opened_OnEnter;
            On.RoR2.EquipmentCatalog.RegisterEquipment += EquipmentCatalog_RegisterEquipment;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }



        private void HealthComponent_TakeDamage  (On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            if (damageInfo.attacker && self) {
                GameObject attacker = damageInfo.attacker;
                if (damageInfo.attacker.HasComponent<CharacterBody>()) {
                    CharacterBody attackerBody = attacker.GetComponent<CharacterBody>();
                    if (attackerBody.inventory) {
                        Inventory inv = attackerBody.inventory;
                        int experienceOnHitCount = inv.GetItemCount(Item.experienceOnHitIndex);
                        if (experienceOnHitCount > 0) {
                            float exp = 0.1f + (experienceOnHitCount * 0.4f);
                            ExperienceManager.instance.AwardExperience(self.transform.position, attackerBody, (ulong)(uint)exp);
                        }
                        //Main.LogD(damageInfo.damage.ToString());
                        int betterCritDamageOnLowHealth = inv.GetItemCount(Item.betterCritDamageOnLowHealthIndex);
                        if (betterCritDamageOnLowHealth > 0 && damageInfo.crit && attackerBody.healthComponent && attackerBody.healthComponent.fullCombinedHealth < 0.4) {
                            damageInfo.crit = true;
                            //Main.LogD(damageInfo.damage.ToString());
                        }
                    }
                }
            }
            orig(self, damageInfo);
        }
        #region GlobalEventManager
        public void GlobalEventManager_OnCrit(On.RoR2.GlobalEventManager.orig_OnCrit orig, GlobalEventManager self, CharacterBody body, CharacterMaster master, float procCoefficient, ProcChainMask procChainMask)
        {
            if (body && procCoefficient > 0f && body && master && master.inventory) {
                Inventory masterInventory = master.inventory;
                HealthComponent healthComponent = body.healthComponent;
                int topazLense = masterInventory.GetItemCount(Item.barrierOnCritIndex);
                int lightningEye = masterInventory.GetItemCount(Item.lightningOnCritIndex);
                if (topazLense > 0 && healthComponent) {
                    healthComponent.AddBarrier(2.5f + (topazLense * 2.5f));
                }
                if (lightningEye > 0 && Util.CheckRoll(20, master)) {
                    HealthComponent com = PickRandomTarget(body.transform.position, body.teamComponent.teamIndex, 25).healthComponent;
                    if (com)
                    {
                        Vector3 pos = com.transform.position;
                        BlastAttack attack = new BlastAttack
                        {
                            attacker = body.gameObject,
                            attackerFiltering = AttackerFiltering.Default,
                            baseDamage = 1 + (2f * lightningEye),
                            baseForce = 500,
                            bonusForce = new Vector3(0, 0, 0),
                            crit = false,
                            damageColorIndex = DamageColorIndex.Default,
                            damageType = DamageType.Stun1s,
                            falloffModel = BlastAttack.FalloffModel.Linear,
                            //impactEffect = EffectIndex.Invalid,
                            inflictor = body.gameObject,
                            losType = BlastAttack.LoSType.NearestHit,
                            position = pos,
                            procChainMask = procChainMask,
                            procCoefficient = 0,
                            radius = 15,
                            teamIndex = body.teamComponent.teamIndex
                        };
                        EffectManager.SpawnEffect(ChargeMeteor.meteorEffect, new EffectData
                        {
                            origin = pos,
                            scale = 15
                        }, true);
                        attack.Fire();
                    }
                }
            }
            onCritSubscription = true;
            orig(self, body, master, procCoefficient, procChainMask);
        }
        public void EquipmentCatalog_RegisterEquipment(On.RoR2.EquipmentCatalog.orig_RegisterEquipment orig, EquipmentIndex equipmentIndex, EquipmentDef equipmentDef)
        {
            if (equipmentIndex == EquipmentIndex.Meteor) {
                equipmentDef.enigmaCompatible = false;
            }
            orig(equipmentIndex, equipmentDef);
        }
        public  void GlobalEventManagerOnOnCharacterDeath(DamageReport damageReport)
        {
            if (!damageReport.attackerBody || !damageReport.victimBody || !damageReport.attacker || !damageReport.victim || !damageReport.attackerMaster || !damageReport.victimMaster || damageReport == null)
                return;
            CharacterBody attackerBody = damageReport.attackerBody;
            //CharacterBody victimBody = damageReport.victimBody;
            CharacterMaster attackerMaster = damageReport.attackerMaster;
            //CharacterMaster victimMaster = damageReport.victimMaster;
            Inventory attackerInventory = attackerMaster ? attackerMaster.inventory : null;
            //Inventory victimInventory = victimMaster ? victimMaster.inventory : null;

            int itemChampionOnKillCount = attackerInventory.GetItemCount(Item.itemChampionOnKillIndex);
            if (itemChampionOnKillCount > 0 && attackerMaster && Util.CheckRoll(15 + (itemChampionOnKillCount * 10)) && damageReport.victim.body.isBoss == true) {
                Util.PlaySound("ui_obj_casinoChest_open", attackerBody.gameObject);
                attackerInventory.GiveItem(GetRandomItem(bossitemList), 1);
            }

            onDeathSubscription = true;
        }
        public void GlobalEventManager_onServerDamageDealt(DamageReport damageReport)
        {
            if (!damageReport.attackerBody || !damageReport.victimBody || !damageReport.attacker || !damageReport.victim || !damageReport.attackerMaster || !damageReport.victimMaster || damageReport == null)
                return;
            if (damageReport.attackerBody && damageReport != null)
            {
                //CharacterBody attackerBody = damageReport.attackerBody;
                CharacterBody victimBody = damageReport.victimBody;
                CharacterMaster attackerMaster = damageReport.attackerMaster;
                CharacterMaster victimMaster = damageReport.victimMaster;
                Inventory attackerInventory = attackerMaster ? attackerMaster.inventory : null;
                //Inventory victimInventory = victimMaster ? victimMaster.inventory : null;
                int scpRandom = UnityEngine.Random.Range(0, scpBuffList.Count);
                int rootCount = attackerInventory.GetItemCount(Item.crippleOnHitIndex);
                int scpCount = attackerInventory.GetItemCount(Item.randomDebuffOnHitIndex);
                if (rootCount > 0 && Util.CheckRoll(20, attackerMaster) && attackerMaster && damageReport.victimBody) {
                    Util.PlaySound("Play_nullifier_attack1_root", victimBody.gameObject);
                    victimBody.AddTimedBuff(BuffIndex.Cripple, 3);
                }
                if (scpCount > 0 && damageReport.victimBody && victimMaster) {
                    victimBody.AddTimedBuff(scpBuffList[scpRandom], (scpCount * 2));
                    }
                }
                onDmgDealtSubscription = true;
            }
            public void GlobalEventManager_OnTeamLevelUp(On.RoR2.GlobalEventManager.orig_OnTeamLevelUp orig, TeamIndex teamIndex)
            {
                ReadOnlyCollection<TeamComponent> teamMembers = TeamComponent.GetTeamMembers(teamIndex);
            for (int i = 0; i < teamMembers.Count; i++) {
                TeamComponent teamComponent = teamMembers[i];
                if (teamComponent) {
                    CharacterBody characterBody = teamComponent.GetComponent<CharacterBody>();
                    if (characterBody) {
                        if (NetworkServer.active) {
                            CharacterMaster master = characterBody.master;
                            if (master) {
                                int itemOnLevelUpCount = master.inventory.GetItemCount(Item.itemOnLevelUpIndex);
                                int barrierOnLevelUpCount = master.inventory.GetItemCount(Item.barrierOnLevelIndex);

                                if (itemOnLevelUpCount > 0 && Util.CheckRoll(50, master)) {
                                    NokiaCall(characterBody.transform, itemOnLevelUpCount);
    
                                }
                                if (barrierOnLevelUpCount > 0) {
                                    if (characterBody.healthComponent) {
                                        characterBody.healthComponent.AddBarrier(25 + (5 * barrierOnLevelUpCount));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            onTeamLevelUpSubscription = true;
            orig(teamIndex);
        }
        #endregion
        public void DodgeState_OnEnter(On.EntityStates.Commando.DodgeState.orig_OnEnter orig, EntityStates.Commando.DodgeState self)
        {
            self.outer.commonComponents.characterBody.isSprinting = true;
            dodgeStateSubscription = true;
            orig(self);
        }
        public static void CharacterSpawnCard_Awake(On.RoR2.CharacterSpawnCard.orig_Awake orig, CharacterSpawnCard self)
        {
            self.loadout = new SerializableLoadout();
            orig(self);
        }
        public static void BuffCatalog_RegisterBuff(On.RoR2.BuffCatalog.orig_RegisterBuff orig, BuffIndex buffIndex, BuffDef buffDef)
        {
            //Fixes deathmark
            if (buffIndex == BuffIndex.DeathMark) {
                buffDef.canStack = true;
            }
            onRegisterBuffSubscription = true;
            orig(buffIndex, buffDef);
        }
        public bool EquipmentSlotOnPerformEquipmentAction(On.RoR2.EquipmentSlot.orig_PerformEquipmentAction orig, EquipmentSlot self, EquipmentIndex index)
        {
            if (index == Equip.brokenPrinterIndex) {
                Equip.SummonMjnion(self.characterBody);
                return true;
            }
            if (index == Equip.unstableQuantumReactorIndex) {
                Equip.BecomeUnstable(self.characterBody);
                return true;
            }
            if (index == Equip.lumpkinIndex) {
                Equip.Scream(self.characterBody);
                return true;
            }
            equipmentSlotSubscription = true;
            return orig(self, index); //must
        }
        public void GrantItem(On.RoR2.GenericPickupController.orig_GrantItem orig, GenericPickupController self, CharacterBody body, Inventory inventory)
        {
            orig(self, body, inventory);
            int cloakOnInteractionCount = inventory.GetItemCount(Item.cloakOnInteractionIndex);
            if (self && inventory && cloakOnInteractionCount > 0) {
                body.AddTimedBuff(BuffIndex.CloakSpeed, 2 * cloakOnInteractionCount);
            }
            grantItemubscription = true;
        }
        public void TeleporterInteractionOnTeleporterBeginChargingGlobal(TeleporterInteraction obj)
        {
            int count = Util.GetItemCountForTeam(TeamIndex.Player, Item.largerTeleporterRadiusIndex, true, true);
            if (count > 0) {
                obj.holdoutZoneController.baseRadius = obj.holdoutZoneController.baseRadius + (count * 3);
            }
            onTeleporterBeginChargingSubscription = true;
        }
        public static void Opened_OnEnter(On.EntityStates.Barrel.Opened.orig_OnEnter orig, EntityStates.Barrel.Opened self)
        {
            orig(self);
            Object.Destroy(self.outer.gameObject, 1);
        }
        public static void GlobalEventManager_OnInteractionBegin(On.RoR2.GlobalEventManager.orig_OnInteractionBegin orig, GlobalEventManager self, Interactor interactor, IInteractable interactable, UnityEngine.GameObject interactableObject)
        {
            orig(self, interactor, interactable, interactableObject);
            CharacterBody characterBody = interactor.GetComponent<CharacterBody>();
            if (characterBody)
            {
                Inventory inventory = characterBody.inventory;
                if (inventory && !interactableObject.HasComponent<GenericPickupController>())
                {
                    int moneyOnInteractionCount = inventory.GetItemCount(Item.moneyOnInteractionIndex);
                    int eliteBuffOnInteractionCount = inventory.GetItemCount(Item.eliteBuffOnInteractionIndex);

                    if (moneyOnInteractionCount > 0) {
                        TeamManager.instance.GiveTeamMoney(characterBody.teamComponent.teamIndex, 5U + (uint)(moneyOnInteractionCount * 5));
                        //characterBody.master.GiveMoney(5U + (uint)(moneyOnInteractionCount * 5));
                    }
                    if (eliteBuffOnInteractionCount > 0) {
                        Util.PlaySound("Play_item_use_gainArmor", characterBody.gameObject);
                        int count = UnityEngine.Random.Range(0, eliteBuffList.Count);
                        characterBody.AddTimedBuff(eliteBuffList[count], 5 * eliteBuffOnInteractionCount);
                    }
                }
            }
            onInteractionSubscription = true;
        }
    }
}