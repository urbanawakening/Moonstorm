using CloudBurst.Equipment;
using CloudBurst.Items;
using RoR2;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst
{
    internal sealed class Hooks
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
        public static bool grantItemSubscription;
        public static bool onTeleporterBeginChargingSubscription;
        #endregion
        #region Voids
        public static List<ItemIndex> bossitemList = new List<ItemIndex>{
            ItemIndex.NovaOnLowHealth,
            ItemIndex.Knurl,
            ItemIndex.BeetleGland,
            ItemIndex.TitanGoldDuringTP,
            ItemIndex.SprintWisp,
            ItemIndex.Incubator
            //Excluding pearls because those aren't boss items, they come from the Cleansing Pool 
        };

        public static List<BuffIndex> scpBuffList = new List<BuffIndex>{
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
        public static void NokiaCall(Transform transform, int itemCount)
        {
            var dropList = Util.CheckRoll((5 * itemCount)) ? Run.instance.availableTier2DropList : Run.instance.availableTier1DropList;
            int nextItem = Run.instance.treasureRng.RangeInt(0, dropList.Count);

            PickupDropletController.CreatePickupDroplet(dropList[nextItem], transform.position, transform.forward * 155);
        }
        public static ItemIndex GetRandomItem(List<ItemIndex> items)
        {
            int itemID = UnityEngine.Random.Range(0, items.Count);
            return items[itemID];
        }
#endregion
        #region GlobalEventManager
        public static void GlobalEventManager_OnCrit(On.RoR2.GlobalEventManager.orig_OnCrit orig, GlobalEventManager self, CharacterBody body, CharacterMaster master, float procCoefficient, ProcChainMask procChainMask)
        {
            if (body && procCoefficient > 0f && body && master && master.inventory)
            {
                Inventory masterInventory = master.inventory;
                HealthComponent healthComponent = body.healthComponent;
                int topazLense = masterInventory.GetItemCount(Items.barrierOnCritIndex);
                if (topazLense > 0)
                {
                    healthComponent.AddBarrier(2.5f + (topazLense * 2.5f));
                }
            }
            onCritSubscription = true;
            orig(self, body, master, procCoefficient, procChainMask);
        }
        public static void GlobalEventManager_OnInteractionsGlobal(Interactor interactor, IInteractable interactable, GameObject gameObject)
        {
            CharacterBody characterBody = interactor.GetComponent<CharacterBody>();
            if (characterBody) {
                Inventory inventory = characterBody.inventory;
                CharacterMaster master = characterBody.master;
                if (inventory && master) {
                    int moneyOnInteractionCount = inventory.GetItemCount(Items.moneyOnInteractionIndex);
                    int eliteBuffOnInteractionCount = inventory.GetItemCount(Items.eliteBuffOnInteractionIndex);
                    if (moneyOnInteractionCount > 0) {
                        master.GiveMoney(5U + (uint)(moneyOnInteractionCount * 3));
                    }
                    if (eliteBuffOnInteractionCount > 0 && Util.CheckRoll(15 + (eliteBuffOnInteractionCount * 5), master)) {
                        Util.PlaySound("Play_item_use_gainArmor", characterBody.gameObject);
                        int count = UnityEngine.Random.Range(0, eliteBuffList.Count);
                        characterBody.AddTimedBuff(eliteBuffList[count], 5);
                    }
                }
            }
            onInteractionSubscription = true;
        }
        public static void GlobalEventManagerOnOnCharacterDeath(DamageReport damageReport)
        {
            if (!damageReport.attackerBody || !damageReport.victimBody || !damageReport.attacker || !damageReport.victim || !damageReport.attackerMaster || !damageReport.victimMaster || damageReport == null)
                return;
            CharacterBody attackerBody = damageReport.attackerBody;
            //CharacterBody victimBody = damageReport.victimBody;
            CharacterMaster attackerMaster = damageReport.attackerMaster;
            //CharacterMaster victimMaster = damageReport.victimMaster;
            Inventory attackerInventory = attackerMaster ? attackerMaster.inventory : null;
            //Inventory victimInventory = victimMaster ? victimMaster.inventory : null;

            int itemChampionOnKillCount = attackerInventory.GetItemCount(Items.itemChampionOnKillIndex);
            if (itemChampionOnKillCount > 0 && attackerMaster && Util.CheckRoll(65 + (itemChampionOnKillCount * 5))&& damageReport.victim.body.isChampion == true) {
                Util.PlaySound("ui_obj_casinoChest_open", attackerBody.gameObject);
                attackerInventory.GiveItem(GetRandomItem(bossitemList), 1);
            }

            onDeathSubscription = true;
        }
        public static void GlobalEventManager_onServerDamageDealt(DamageReport damageReport)
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
                int rootCount = attackerInventory.GetItemCount(Items.crippleOnHitIndex);
                int scpCount = attackerInventory.GetItemCount(Items.randomDebuffOnHitIndex);
                if (rootCount > 0 && Util.CheckRoll(20, attackerMaster) && attackerMaster && damageReport.victimBody)
                {
                    Util.PlaySound("Play_nullifier_attack1_root", victimBody.gameObject);
                    victimBody.AddTimedBuff(BuffIndex.Cripple, 3);
                }
                if (scpCount > 0 && damageReport.victimBody && victimMaster)
                {
                    victimBody.AddTimedBuff(scpBuffList[scpRandom], (scpCount * 2));
                }
            }
            onDmgDealtSubscription = true;
        }
        public static void GlobalEventManager_OnTeamLevelUp(On.RoR2.GlobalEventManager.orig_OnTeamLevelUp orig, TeamIndex teamIndex)
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
                                int itemOnLevelUpCount = master.inventory.GetItemCount(Items.itemOnLevelUpIndex);
                                int barrierOnLevelUpCount = master.inventory.GetItemCount(Items.barrierOnLevelIndex);

                                if (itemOnLevelUpCount > 0 && Util.CheckRoll(75, master)) {
                                    NokiaCall(characterBody.transform, itemOnLevelUpCount);
                                }
                                if (barrierOnLevelUpCount > 0) {
                                    if (characterBody.healthComponent) {
                                        characterBody.healthComponent.AddBarrier(15 + (5 * barrierOnLevelUpCount));

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
        public static void DodgeState_OnEnter(On.EntityStates.Commando.DodgeState.orig_OnEnter orig, EntityStates.Commando.DodgeState self)
        {
            self.outer.commonComponents.characterBody.isSprinting = true;
            dodgeStateSubscription = true;
            orig(self);
        }
        public static void BuffCatalog_RegisterBuff(On.RoR2.BuffCatalog.orig_RegisterBuff orig, BuffIndex buffIndex, BuffDef buffDef)
        {
            //Fixes deathmark
            if (!Main.instance.deathMarkMod)
            {
                if (buffIndex == BuffIndex.DeathMark)
                {
                    buffDef.canStack = true;
                }
            }
            onRegisterBuffSubscription = true;
            orig(buffIndex, buffDef);
        }
        public static bool EquipmentSlotOnPerformEquipmentAction(On.RoR2.EquipmentSlot.orig_PerformEquipmentAction orig, EquipmentSlot self, EquipmentIndex index)
        {
            if (index == Equipment.brokenPrinterIndex)
            {
                Equipment.SummonMjnion(self.characterBody);
                return true;
            }
            if (index == Equipment.unstableQuantumReactorIndex)
            {
                Equipment.BecomeUnstable(self.characterBody);
                return true;
            }
            if (index == Equipment.lumpkinIndex)
            {
                Equipment.Scream(self.characterBody);
                return true;
            }
            equipmentSlotSubscription = true;
            return orig(self, index); //must
        }
        public static void GrantItem(On.RoR2.GenericPickupController.orig_GrantItem orig, GenericPickupController self, CharacterBody body, Inventory inventory)
        {
            orig(self, body, inventory);
            int cloakOnInteractionCount = inventory.GetItemCount(Items.cloakOnInteractionIndex);
            if (self && inventory && cloakOnInteractionCount > 0)
            {
                body.AddTimedBuff(BuffIndex.Cloak, 2 * cloakOnInteractionCount);
            }
            grantItemSubscription = true;
        }
        public static void TeleporterInteractionOnTeleporterBeginChargingGlobal(TeleporterInteraction obj)
        {
            ReadOnlyCollection<TeamComponent> teamMembers = TeamComponent.GetTeamMembers(TeamIndex.Player);
            int count = 0;
            foreach (TeamComponent tc in teamMembers)
            {
                if (Util.LookUpBodyNetworkUser(tc.body))
                {
                    count += tc.body.inventory.GetItemCount(Items.largerTeleporterRadiusIndex);
                }
            }
            if (count > 0)
            {
                foreach (TeamComponent tc in teamMembers)
                {

                    obj.holdoutZoneController.baseRadius = obj.holdoutZoneController.baseRadius + (count * 3);
                }
            }
            onTeleporterBeginChargingSubscription = true;
        }
        public static void Opened_OnEnter(On.EntityStates.Barrel.Opened.orig_OnEnter orig, EntityStates.Barrel.Opened self)
        {
            orig(self);
            Object.Destroy(self.outer.gameObject, 1);
        }
    }
}
