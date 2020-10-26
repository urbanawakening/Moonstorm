using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

namespace Starstorm2.Cores
{
    class ItemCore
    {
        public ItemCore instance;

        /* To add an item:
         * 1. define its ItemIndex at top
         * 2. register it in RegisterItems
         * 3. add its language tokens in RegisterTokens
         * 4. implement its behavior using existing or new hooks (called by Hook)
         */

        //TODO:
        //Give Items Models
        //Give items icons
        //These values are set by RegisterItem(ItemIndex, ItemDef).

        //STARSTORM 2
        protected internal ItemIndex dormantFungusIndex;
        protected internal ItemIndex diaryIndex;
        protected internal ItemIndex coinIndex;
        protected internal ItemIndex itemOnLevelUpIndex;
        protected internal ItemIndex damageOverTimeToLowHealthIndex;
        protected internal ItemIndex moneyOnHealIndex;
        protected internal ItemIndex fireOnEquipmentUseIndex;
        protected internal ItemIndex poisonOnHitIndex;
        protected internal ItemIndex erraticGadgetIndex;
        protected internal ItemIndex explodeOnJumpIndex;
        protected internal ItemIndex aoeOnHitIndex;
        protected internal ItemIndex forkIndex;
        protected internal ItemIndex coffeeIndex;
        protected internal ItemIndex metronomeIndex;
        protected internal ItemIndex huntersSigilIndex;

        private const string modPrefix = "@Starstorm2:";
        private const string prefabPath = modPrefix + "Assets/Items/";

        public static GameObject maliceEffect = AssetsCore.mainAssetBundle.LoadAsset<GameObject>("MaliceEffect");

        public ItemCore() => RegisterItems();

        protected internal void RegisterItems()
        {
            LogCore.LogI("Initializing Core: " + base.ToString());

            instance = this;

            RegisterTokens();

            ItemTag[] itemOnLevelUpTags = new ItemTag[2]
            {
                ItemTag.AIBlacklist,
                ItemTag.Utility
            };
            itemOnLevelUpIndex = RegisterItem(new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_ITEMONLEVELUP_DESC",
                hidden = false,
                loreToken = "",
                name = "ItemOnLevelUp",
                nameToken = "ITEM_ITEMONLEVELUP_NAME",
                pickupIconPath = prefabPath + "NkotasHeritage/NkotasHeritage_Icon.png",
                pickupModelPath = prefabPath + "NkotasHeritage/MDLNkotasHeritage.prefab",
                pickupToken = "ITEM_ITEMONLEVELUP_PICKUP",
                tags = itemOnLevelUpTags,
                tier = ItemTier.Tier3,
                unlockableName = ""
            });
            ItemTag[] lightningOnCritTags = new ItemTag[2]
            {
                    ItemTag.Utility,
                    ItemTag.Damage
            };
            damageOverTimeToLowHealthIndex = RegisterItem(new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_LIGHTNINGONCRIT_DESC",
                hidden = false,
                loreToken = "",
                name = "LightningOnCrit",
                nameToken = "ITEM_LIGHTNINGONCRIT_NAME",
                pickupIconPath = prefabPath + "DetritiveTrematode/DetritiveTrematode_Icon.png",
                pickupModelPath = prefabPath + "DetritiveTrematode/MDLDetritiveTrematode.prefab",
                pickupToken = "ITEM_LIGHTNINGONCRIT_PICKUP",
                tags = lightningOnCritTags,
                tier = ItemTier.Tier1,
                unlockableName = ""
            });
            ItemTag[] moneyOnHealTags = new ItemTag[2]
            {
                    ItemTag.Utility,
                    ItemTag.AIBlacklist
            };
            moneyOnHealIndex = RegisterItem(new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_BLOODTESTER_DESC",
                hidden = false,
                loreToken = "",
                name = "BloodTester",
                nameToken = "ITEM_BLOODTESTER_NAME",
                pickupIconPath = prefabPath + "BloodTester/BloodTester_Icon.png",
                pickupModelPath = prefabPath + "BloodTester/MDLBloodTester.prefab",
                pickupToken = "ITEM_BLOODTESTER_PICKUP",
                tags = moneyOnHealTags,
                tier = ItemTier.Tier2,
                unlockableName = ""
            });
            ItemTag[] dormantFungusTags = new ItemTag[2]
{
                    ItemTag.Healing,
                    ItemTag.AIBlacklist
};
            dormantFungusIndex = RegisterItem(new ItemDef
            {
                canRemove = true,
                //descriptionToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_DESC",
                hidden = false,
                loreToken = "",
                name = "DormantFungus",
                //nameToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_NAME",
                pickupIconPath = prefabPath + "DormantFungus/DormantFungus_Icon.png",
                pickupModelPath = prefabPath + "DormantFungus/MDLDormantFungus.prefab",
                //pickupToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_PICKUP",
                tags = dormantFungusTags,
                tier = ItemTier.Tier1,
                unlockableName = ""
            });
            ItemTag[] diaryTags = new ItemTag[2]
{
                    ItemTag.Utility,
                    ItemTag.AIBlacklist
};
            diaryIndex = RegisterItem(new ItemDef
            {
                canRemove = true,
                //descriptionToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_DESC",
                hidden = false,
                loreToken = "",
                name = "Diary",
                //nameToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_NAME",
                pickupIconPath = prefabPath + "Diary/Diary_Icon.png",
                pickupModelPath = prefabPath + "Diary/MDLDiary.prefab",
                //pickupToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_PICKUP",
                tags = diaryTags,
                tier = ItemTier.Tier1,
                unlockableName = ""
            });
            ItemTag[] coinTags = new ItemTag[1]
            {
                    ItemTag.Damage
            };
            coinIndex = RegisterItem(new ItemDef
            {
                canRemove = true,
                //descriptionToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_DESC",
                hidden = false,
                loreToken = "",
                name = "Coin",
                //nameToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_NAME",
                pickupIconPath = prefabPath + "MoltenCoin/MoltenCoin_Icon.png",
                pickupModelPath = prefabPath + "MoltenCoin/MDLMoltenCoin.prefab",
                //pickupToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_PICKUP",
                tags = coinTags,
                tier = ItemTier.Tier1,
                unlockableName = ""
            });
            ItemTag[] fireOnEquipmentUseTags = new ItemTag[2]
            {
                    ItemTag.Damage,
                    ItemTag.AIBlacklist
            };
            fireOnEquipmentUseIndex = RegisterItem(new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_FIREONEQUIPMENTUSE_DESC",
                hidden = false,
                loreToken = "",
                name = "Sauce",
                nameToken = "ITEM_FIREONEQUIPMENTUSE_NAME",
                pickupIconPath = prefabPath + "HottestSauce/HottestSauce_Icon.png",
                pickupModelPath = prefabPath + "HottestSauce/MDLHottestSauce.prefab",
                pickupToken = "ITEM_FIREONEQUIPMENTUSE_PICKUP",
                tags = fireOnEquipmentUseTags,
                tier = ItemTier.Tier2,
                unlockableName = ""
            });
            ItemTag[] poisonOnHitTags = new ItemTag[1]
            {
                    ItemTag.Damage
            };
            poisonOnHitIndex = RegisterItem(new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_POISONONHIT_DESC",
                hidden = false,
                loreToken = "",
                name = "StrangeCan",
                nameToken = "ITEM_POISONONHIT_NAME",
                pickupIconPath = prefabPath + "StrangeCan/StrangeCan_Icon.png",
                pickupModelPath = prefabPath + "StrangeCan/MDLStrangeCan.prefab",
                pickupToken = "ITEM_POISONONHIT_PICKUP",
                tags = poisonOnHitTags,
                tier = ItemTier.Tier2,
                unlockableName = ""
            });
            ItemTag[] erraticGadgetTags = new ItemTag[2]
            {
                    ItemTag.Damage,
                    ItemTag.AIBlacklist
            };
            erraticGadgetIndex = RegisterItem(new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_ERRATICGADGET_DESC",
                hidden = false,
                loreToken = "",
                name = "ErraticGadget",
                nameToken = "ITEM_ERRATICGADGET_NAME",
                pickupIconPath = prefabPath + "ErraticGadget/ErraticGadget_Icon.png",
                pickupModelPath = prefabPath + "ErraticGadget/MDLErraticGadget.prefab",
                pickupToken = "ITEM_ERRATICGADGET_PICKUP",
                tags = erraticGadgetTags,
                tier = ItemTier.Tier3,
                unlockableName = ""
            });
            ItemTag[] explodeOnJumpTags = new ItemTag[1]
            {
                    ItemTag.Damage
            };
            explodeOnJumpIndex = RegisterItem(new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_EXPLODEONJUMP_DESC",
                hidden = false,
                loreToken = "",
                name = "PrototypeJetBoots",
                nameToken = "ITEM_EXPLODEONJUMP_NAME",
                pickupIconPath = prefabPath + "JetBoots/JetBoots_Icon.png",
                pickupModelPath = prefabPath + "Jetboots/MDLJetBoots.prefab",
                pickupToken = "ITEM_EXPLODEONJUMP_PICKUP",
                tags = explodeOnJumpTags,
                tier = ItemTier.Tier2,
                unlockableName = ""
            });
            ItemTag[] aoeOnHitTags = new ItemTag[]
            {
                ItemTag.Damage
            };
            aoeOnHitIndex = RegisterItem(new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_AOEONHIT_DESC",
                hidden = false,
                loreToken = "",
                name = "Malice",
                nameToken = "ITEM_AOEONHIT_NAME",
                pickupIconPath = prefabPath + "Malice/Malice_Icon.png",

                pickupToken = "ITEM_AOEONHIT_PICKUP",
                tags = aoeOnHitTags,
                tier = ItemTier.Tier1,
                unlockableName = ""
            });
            ItemTag[] forkTags = new ItemTag[]
            {
                ItemTag.Damage
            };
            forkIndex = RegisterItem(new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_FORK_DESC",
                hidden = false,
                loreToken = "",
                name = "Fork",
                nameToken = "ITEM_FORK_NAME",
                pickupIconPath = prefabPath + "Fork/Fork_Icon.png",

                pickupToken = "ITEM_FORK_PICKUP",
                tags = forkTags,
                tier = ItemTier.Tier1,
                unlockableName = ""
            });
            ItemTag[] coffeeTags = new ItemTag[]
            {
                ItemTag.Damage,
                ItemTag.Utility
            };
            coffeeIndex = RegisterItem(new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_COFFEE_DESC",
                hidden = false,
                loreToken = "",
                name = "CoffeeBag",
                nameToken = "ITEM_COFFEE_NAME",
                pickupIconPath = prefabPath + "CoffeeBag/CoffeeBag_Icon.png",

                pickupToken = "ITEM_COFFEE_PICKUP",
                tags = coffeeTags,
                tier = ItemTier.Tier1,
                unlockableName = ""
            });
            ItemTag[] metronomeTags = new ItemTag[]
            {
                ItemTag.Utility
            };
            metronomeIndex = RegisterItem(new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_METRONOME_DESC",
                hidden = false,
                loreToken = "",
                name = "WatchMetronome",
                nameToken = "ITEM_METRONOME_NAME",
                pickupIconPath = prefabPath + "WatchMetronome/WatchMetronome_Icon.png",

                pickupToken = "ITEM_METRONOME_PICKUP",
                tags = metronomeTags,
                tier = ItemTier.Tier2,
                unlockableName = ""
            });
            ItemTag[] sigilTags = new ItemTag[]
            {
                ItemTag.Damage,
                ItemTag.Utility
            };
            huntersSigilIndex = RegisterItem(new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_SIGIL_DESC",
                hidden = false,
                loreToken = "",
                name = "HuntersSigil",
                nameToken = "ITEM_SIGIL_NAME",
                pickupIconPath = prefabPath + "HuntersSigil/HuntersSigil_Icon.png",

                pickupToken = "ITEM_SIGIL_PICKUP",
                tags = sigilTags,
                tier = ItemTier.Tier2,
                unlockableName = ""
            });

            Hook();
        }

        protected internal void RegisterTokens()
        {
            //STARSTORM 2

            LanguageAPI.Add("ITEM_LIGHTNINGONCRIT_NAME", "Detritive Trematode"); //model(??)
            LanguageAPI.Add("ITEM_LIGHTNINGONCRIT_DESC", "Enemies with low health recieve a <style=cIsDamage>damage over time</style> that deals <style=cIsDamage>100%</style> <style=cStack>(+100% per stack)</style> damage.");
            LanguageAPI.Add("ITEM_LIGHTNINGONCRIT_PICKUP", "Low health enemies recieve damage over time.");

            LanguageAPI.Add("ITEM_BLOODTESTER_NAME", "Broken Blood Tester");
            LanguageAPI.Add("ITEM_BLOODTESTER_DESC", "Gain <style=cIsUtility>5 gold</style> <style=cStack>(+5 gold per stack)</style> every time you heal.");
            LanguageAPI.Add("ITEM_BLOODTESTER_PICKUP", "Gain money on healing.");

            LanguageAPI.Add("ITEM_DORMANTFUNGUS_NAME", "Dormant Fungus");
            LanguageAPI.Add("ITEM_DORMANTFUNGUS_DESC", "<style=cIsHealing>Heal 1%</style> <style=cStack>(+1% per stack)</style> every <style=cIsHealing>5</style> seconds while sprinting."); //by 2<style=cStack>(+1 per stack)</style> seconds.");
            LanguageAPI.Add("ITEM_DORMANTFUNGUS_PICKUP", "Heal every five seconds while sprinting.");

            LanguageAPI.Add("ITEM_DIARY_NAME", "Diary");
            LanguageAPI.Add("ITEM_DIARY_DESC", "Gain <style=cIsUtility>5% experience</style> <style=cStack>(+5% per stack)</style> every <style=cIsUtility>2</style> seconds."); //by 2<style=cStack>(+1 per stack)</style> seconds.");
            LanguageAPI.Add("ITEM_DIARY_PICKUP", "Gain experience overtime.");

            LanguageAPI.Add("ITEM_COIN_NAME", "Molten Coin");
            LanguageAPI.Add("ITEM_COIN_DESC", "<style=cIsDamage>6%</style> chance to <style=cIsDamage>ignite</style> enemies on hit for <style=cIsDamage>50%</style> base damage <style=cStack>(+50% per stack)</style> and earn <style=cIsUtility>$1</style> <style=cStack>(+$1 per stack)."); //by 2<style=cStack>(+1 per stack)</style> seconds.");
            LanguageAPI.Add("ITEM_COIN_PICKUP", "Chance to ignite enemies on hit.");

            LanguageAPI.Add("ITEM_ITEMONLEVELUP_NAME", "Nkota's Heritage");
            LanguageAPI.Add("ITEM_ITEMONLEVELUP_DESC", "Gain an item on level up."); //by 2<style=cStack>(+1 per stack)</style> seconds.");
            LanguageAPI.Add("ITEM_ITEMONLEVELUP_PICKUP", "Gain an item on level up.");

            LanguageAPI.Add("ITEM_FIREONEQUIPMENTUSE_NAME", "Hottest Sauce");
            LanguageAPI.Add("ITEM_FIREONEQUIPMENTUSE_DESC", "Activating your equipment <style=cIsDamage>ignites</style> enemies within <style=cIsDamage>30m</style> for <style=cIsDamage>5</style> <style=cStack>(+4 per stack)</style> seconds.");
            LanguageAPI.Add("ITEM_FIREONEQUIPMENTUSE_PICKUP", "Activating your equipment ignites nearby enemies.");

            LanguageAPI.Add("ITEM_POISONONHIT_NAME", "Strange Can");
            LanguageAPI.Add("ITEM_POISONONHIT_DESC", "<style=cIsDamage>8.5%</style> <style=cStack>(+5% per stack)</style> chance to <style=cIsDamage>intoxicate</style> enemies dealing <style=cIsDamage>350% damage over time</style>.");
            LanguageAPI.Add("ITEM_POISONONHIT_PICKUP", "Chance to intoxicate enemies, causing damage over time.");

            LanguageAPI.Add("ITEM_ERRATICGADGET_NAME", "Erratic Gadget");
            LanguageAPI.Add("ITEM_ERRATICGADGET_DESC", "Critical strikes deal <style=cIsDamage>50%</style> <style=cStack>(+50% per stack)</style> extra damage.");
            LanguageAPI.Add("ITEM_ERRATICGADGET_PICKUP", "Critical strikes deal extra damage.");

            LanguageAPI.Add("ITEM_EXPLODEONJUMP_NAME", "Prototype Jet Boots");
            LanguageAPI.Add("ITEM_EXPLODEONJUMP_DESC", "Create an <style=cIsDamage>explosion</style> in a small area around you whenever you jump for <style=cIsDamage>150%</style> <style=cStack>(+100% per stack)</style> damage.");
            LanguageAPI.Add("ITEM_EXPLODEONJUMP_PICKUP", "Detonate on jump!");

            LanguageAPI.Add("ITEM_AOEONHIT_NAME", "Malice");
            LanguageAPI.Add("ITEM_AOEONHIT_DESC", "On damaging an enemy, up to <style=cIsDamage>1</style> other enemy <style=cStack>(+1 per stack)</style> within 5m of that enemy <style=cStack>(+1m per stack)</style> receives <style=cIsDamage>55%</style> damage.");
            LanguageAPI.Add("ITEM_AOEONHIT_PICKUP", "Damage dealt spreads to nearby enemies.");

            LanguageAPI.Add("ITEM_FORK_NAME", "Fork");
            LanguageAPI.Add("ITEM_FORK_DESC", "Increase your base damage by <style=cIsDamage>25%</style> <style=cStack>(+25% per stack)</style>.");
            LanguageAPI.Add("ITEM_FORK_PICKUP", "Deal more damage.");

            LanguageAPI.Add("ITEM_COFFEE_NAME", "Coffee Bag");
            LanguageAPI.Add("ITEM_COFFEE_DESC", "Increase your base movement speed by <style=cIsUtility>10%</style> <style=cStack>(+10% per stack)</style> and your base attack speed by <style=cIsDamage>7%</style> <style=cStack>(+7% per stack)</style>.");
            LanguageAPI.Add("ITEM_COFFEE_PICKUP", "Move and attack faster.");

            LanguageAPI.Add("ITEM_METRONOME_NAME", "Watch Metronome");
            LanguageAPI.Add("ITEM_METRONOME_DESC", "<style=cIsUtility>Standing still</style> for up to 4 seconds gradually increases your <style=cIsUtility>movement speed</style> by up to <style=cIsUtility>100%</style>. Increased movement speed decays linearly over <style=cIsUtility>2</style> seconds of movement <style=cStack>(+2 seconds per stack)</style>.");
            LanguageAPI.Add("ITEM_METRONOME_PICKUP", "Standing still charges speed for your next movement.");

            LanguageAPI.Add("ITEM_SIGIL_NAME", "Hunter's Sigil");
            LanguageAPI.Add("ITEM_SIGIL_DESC", "While <style=cIsUtility>standing still</style>, gain <style=cIsUtility>15</style> armor <style=cStack>(+10 armor per stack)</style> and <style=cIsDamage>25%</style> critical chance <style=cStack>(+20% per stack)</style>.");
            LanguageAPI.Add("ITEM_SIGIL_PICKUP", "Standing still grants bonus armor and critical strike chance.");
        }

        protected internal ItemIndex RegisterItem(ItemDef itemDef)
        {
            var internalName = itemDef.name;
            var upperName = internalName.ToUpper(CultureInfo.InvariantCulture);
            if (itemDef.nameToken == null)
            {
                itemDef.nameToken = string.Format(CultureInfo.InvariantCulture, "ITEM_{0}_NAME", upperName);
            }
            if (itemDef.descriptionToken == null)
            {
                itemDef.descriptionToken = string.Format(CultureInfo.InvariantCulture, "ITEM_{0}_DESC", upperName);
            }
            if (itemDef.pickupToken == null)
            {
                itemDef.pickupToken = string.Format(CultureInfo.InvariantCulture, "ITEM_{0}_PICKUP", upperName);
            }
            if (itemDef.loreToken == null)
            {
                itemDef.loreToken = string.Format(CultureInfo.InvariantCulture, "ITEM_{0}_LORE", upperName);
            }
            var customItem = new CustomItem(itemDef, new ItemDisplayRule[0]);
            return ItemAPI.Add(customItem);
        }

        protected internal void Hook()
        {
            On.RoR2.GlobalEventManager.OnTeamLevelUp += GlobalEventManager_OnTeamLevelUp;
            On.RoR2.HealthComponent.Heal += HealthComponent_Heal;
            On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            On.RoR2.EquipmentSlot.PerformEquipmentAction += EquipmentSlot_PerformEquipmentAction;
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            On.RoR2.CharacterMotor.OnLeaveStableGround += CharacterMotor_OnLeaveStableGround;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private void CharacterMotor_OnLeaveStableGround(On.RoR2.CharacterMotor.orig_OnLeaveStableGround orig, CharacterMotor self)
        {
            bool hasJumped = self.GetComponent<InputBankTest>().jump.justPressed;
            if (self && hasJumped)
            {
                var inventory = self.GetComponent<CharacterBody>().inventory;
                if (inventory)
                {
                    var boots = inventory.GetItemCount(explodeOnJumpIndex);
                    if (boots > 0)
                    {
                        var attacker = self.GetComponent<CharacterBody>().gameObject;
                        var damage = attacker.GetComponent<CharacterBody>().damage * (1.5f + (1 * boots));
                        GameObject bootsEffect = Resources.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFX");
                        EffectData bootsEffectData = new EffectData()
                        {
                            color = new Color32(0, 255, 0, 255),
                            scale = 7.5f,
                            origin = self.GetComponent<CharacterBody>().footPosition
                        };
                        new BlastAttack
                        {
                            attacker = attacker,
                            baseDamage = damage,
                            radius = 7.5f,
                            falloffModel = BlastAttack.FalloffModel.None,
                            procCoefficient = 0f,
                            teamIndex = TeamIndex.Player,
                            position = attacker.transform.position,
                        }.Fire();
                        EffectManager.SpawnEffect(bootsEffect, bootsEffectData, false);
                    }
                }
            }
            orig(self);
        }

        private void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            //lol
            if (self)
            {
                Inventory inventory = self.inventory;
                if (inventory)
                {
                    self.AddItemBehavior<DormantFungusBehavior>(inventory.GetItemCount(dormantFungusIndex));
                    self.AddItemBehavior<DairyBehavior>(inventory.GetItemCount(diaryIndex));
                    self.AddItemBehavior<MetronomeBehavior>(inventory.GetItemCount(metronomeIndex));
                    self.AddItemBehavior<SigilBehavior>(inventory.GetItemCount(huntersSigilIndex));
                }
            }
            orig(self);
        }

        static float healAccum = 0f;
        private float HealthComponent_Heal(On.RoR2.HealthComponent.orig_Heal orig, HealthComponent self, float amount, ProcChainMask procChainMask, bool nonRegen)
        {
            if (self)
            {
                var body = self.body;
                if (body)
                {
                    var inventory = body.inventory;
                    if (inventory)
                    {
                        var bloodTesters = inventory.GetItemCount(moneyOnHealIndex);
                        var amountToGift = bloodTesters * 5;
                        if (bloodTesters > 0)
                        {
                            var tele = TeleporterInteraction.instance;
                            //stop giving money if we're waiting to teleport (prevents a softlock)
                            if (!tele || tele.activationState != TeleporterInteraction.ActivationState.Finished)
                            {
                                if (body.teamComponent && body.isPlayerControlled && self.health < self.fullHealth && nonRegen == true)
                                {
                                    healAccum += amount;
                                }
                            }
                            if (healAccum >= 15f)
                            {
                                healAccum -= 15f;
                                //sound/vfx here
                                body.master.GiveMoney((uint)amountToGift);
                            }
                        }
                    }
                }
            }
            return orig(self, amount, procChainMask, nonRegen);
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            if (damageInfo.attacker && self)
            {
                GameObject attacker = damageInfo.attacker;
                if (damageInfo.attacker.HasComponent<CharacterBody>())
                {
                    CharacterBody attackerBody = attacker.GetComponent<CharacterBody>();
                    Inventory inv = attackerBody.inventory;
                    if (inv)
                    {
                        // ????
                        /*
                        var victimBody = self.body;
                        Inventory inv2 = null;
                        if (self.body && self.body.inventory)
                        {
                            inv2 = self.body.inventory;
                        }
                        */

                        //trematode
                        int lightningOnCritCOunt = inv.GetItemCount(damageOverTimeToLowHealthIndex);

                        var dotController = DotController.FindDotController(self.gameObject);
                        bool hasDot = false;
                        if (dotController)
                        {
                            hasDot = dotController.HasDotActive(DoTCore.instance.detritive);
                            //LogCore.LogI(hasDot);
                        }
                        if (self.combinedHealthFraction < .4f && lightningOnCritCOunt > 0 && !hasDot)
                        {
                            DotController.InflictDot(self.gameObject, attacker, DoTCore.instance.detritive, 3 * lightningOnCritCOunt, 1);
                        }

                    }
                }
            }
            var combinedHealthb4Damage = self.combinedHealth;
            orig(self, damageInfo);
        }

        #region GlobalEventManager

        public void GlobalEventManager_OnTeamLevelUp(On.RoR2.GlobalEventManager.orig_OnTeamLevelUp orig, TeamIndex teamIndex)
        {
            ReadOnlyCollection<TeamComponent> teamMembers = TeamComponent.GetTeamMembers(teamIndex);
            for (int i = 0; i < teamMembers.Count; i++)
            {
                TeamComponent teamComponent = teamMembers[i];
                if (teamComponent)
                {
                    CharacterBody characterBody = teamComponent.GetComponent<CharacterBody>();
                    if (characterBody)
                    {
                        if (NetworkServer.active)
                        {
                            CharacterMaster master = characterBody.master;
                            if (master)
                            {
                                int itemOnLevelUpCount = master.inventory.GetItemCount(itemOnLevelUpIndex);

                                if (itemOnLevelUpCount > 0)
                                {
                                    DropShipCall(characterBody.transform, itemOnLevelUpCount);

                                }
                            }
                        }
                    }
                }
            }
            orig(teamIndex);
        }
        #endregion

        public bool EquipmentSlot_PerformEquipmentAction(On.RoR2.EquipmentSlot.orig_PerformEquipmentAction orig, EquipmentSlot self, EquipmentIndex equipmentIndex)
        {
            if (self)
            {
                var body = self.characterBody;
                if (body)
                {
                    var inventory = body.inventory;
                    if (inventory)
                    {
                        int itemCount = inventory.GetItemCount(fireOnEquipmentUseIndex);
                        if (itemCount > 0)
                        {
                            new BlastAttack()
                            {
                                attacker = self.gameObject,
                                attackerFiltering = AttackerFiltering.Default,
                                baseDamage = 0,
                                baseForce = 100,
                                //bonusForce = 0,
                                crit = false,
                                damageColorIndex = DamageColorIndex.Item,
                                damageType = DamageTypeCore.burnOnHit,
                                falloffModel = BlastAttack.FalloffModel.None,
                                impactEffect = EffectIndex.Invalid,
                                inflictor = self.gameObject,
                                losType = BlastAttack.LoSType.None,
                                position = self.transform.position,
                                procChainMask = default,
                                procCoefficient = 0,
                                radius = 10,
                                teamIndex = body.teamComponent.teamIndex
                            }.Fire();
                        }
                    }
                }
            }
            return orig(self, equipmentIndex);
        }

        public void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            GameObject attacker = damageInfo.attacker;
            if (self && attacker)
            {
                var attackerBody = attacker.GetComponent<CharacterBody>();
                var victimBody = victim.GetComponent<CharacterBody>();
                var inventory = attackerBody.inventory;
                if (inventory)
                {
                    int trematodeCount = inventory.GetItemCount(poisonOnHitIndex);
                    if (trematodeCount > 0)
                    {
                        GameObject chad = attacker;
                        bool flag = (damageInfo.damageType & DamageType.PoisonOnHit) > DamageType.Generic;
                        if ((trematodeCount > 0 || flag) && (flag || Util.CheckRoll((8.5f + (5f * (float)trematodeCount)) * damageInfo.procCoefficient, attackerBody.master)))
                        {
                            ProcChainMask procChainMask = damageInfo.procChainMask;
                            procChainMask.AddProc(ProcType.BleedOnHit);
                            DotController.InflictDot(victim, chad, DoTCore.instance.strangeCanPoison, 3.5f, 1f);
                        }
                    }

                    int gadgetCount = inventory.GetItemCount(erraticGadgetIndex);
                    if (gadgetCount > 0)
                    {
                        if (damageInfo.crit)
                        {
                            GameObject erraticGadgetEffectPrefab = Resources.Load<GameObject>("prefabs/effects/tracers/TracerCaptainDefenseMatrix");
                            if (erraticGadgetEffectPrefab)
                            {
                                EffectData effectData = new EffectData
                                {
                                    origin = victimBody.corePosition,
                                    start = attackerBody.corePosition
                                };
                                EffectManager.SpawnEffect(erraticGadgetEffectPrefab, effectData, true);
                            }
                            DamageInfo newDamageInfo = damageInfo;
                            newDamageInfo.damage = damageInfo.damage * (.5f * gadgetCount);
                            victim.GetComponent<HealthComponent>().TakeDamage(newDamageInfo);
                            //This needs to be edited so that the effect actually originates from a body attachment
                        }
                    }

                    //molten coin
                    int coinCount = inventory.GetItemCount(coinIndex);
                    if (coinCount > 0 && Util.CheckRoll(6f, attackerBody.master))
                    {
                        DotController.InflictDot(victim, attacker, DotController.DotIndex.Burn, 4f, coinCount);
                        attackerBody.master.GiveMoney((uint)coinCount);
                    }

                    //malice
                    int maliceCount = inventory.GetItemCount(aoeOnHitIndex); 
                    if (maliceCount > 0)
                    {
                        int hits = 0;
                        float maliceDmg = 0.55f;
                        float maliceRadius = 7.0f + (maliceCount - 1.0f);
                        EffectData maliceData = new EffectData();
                        maliceData.origin = damageInfo.position;
                        maliceData.scale = maliceRadius;
                        EffectManager.SpawnEffect(maliceEffect, maliceData, true);
                        DamageInfo di = new DamageInfo();
                        di.attacker = attacker;
                        di.crit = damageInfo.crit;
                        di.damage = damageInfo.damage * maliceDmg;
                        di.damageColorIndex = DamageColorIndex.Default;
                        di.damageType = DamageType.AOE;
                        di.dotIndex = DotController.DotIndex.None;
                        di.inflictor = attacker;
                        di.procChainMask = default(ProcChainMask);
                        di.procCoefficient = 0f;
                        Collider[] aoe = Physics.OverlapSphere(damageInfo.position, maliceRadius);
                        for (int i = 0; hits < maliceCount && i < aoe.Length; i++)
                        {
                            HealthComponent hp = aoe[i].GetComponent<HealthComponent>();
                            if (hp && hp.GetComponent<TeamComponent>().teamIndex != TeamComponent.GetObjectTeam(attacker) && hp.body != victim.GetComponent<CharacterBody>())
                            {
                                di.position = aoe[i].transform.position;
                                //diminishing scaling per stack (remove multiplier from di.damage above)
                                //di.damage *= 0.55f;
                                hp.TakeDamage(di);
                                hits++;
                            }
                        }
                    }
                }
            }
            orig(self, damageInfo, victim);
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);
            Inventory inv = self.inventory;
            if (inv)
            {
                //fork
                int forkCount = inv.GetItemCount(forkIndex);
                if (forkCount > 0)
                    self.damage += (self.baseDamage + self.levelDamage * (self.level - 1)) * 0.25f * forkCount;

                //coffee
                int coffeeCount = inv.GetItemCount(coffeeIndex);
                self.moveSpeed += self.baseMoveSpeed * 0.1f * coffeeCount;
                self.attackSpeed += self.baseAttackSpeed * 0.07f * coffeeCount;

                //sigil
                int sigilCount = inv.GetItemCount(huntersSigilIndex);
                if (sigilCount > 0 && self.HasBuff(BuffCore.instance.sigilBuffIndex))
                {
                    self.armor += 15 + 10 * (sigilCount - 1);
                    self.crit += 25 + 20 * (sigilCount - 1);
                }
            }

            //metronome
            var metronome = self.GetComponent<MetronomeBehavior>();
            if (metronome)
            {
                self.moveSpeed += (self.baseMoveSpeed * metronome.metroCharge * MetronomeBehavior.speedBoostMult);
            }
        }

        public void DropShipCall(Transform transform, int itemCount)
        {
            List<PickupIndex> list = Util.CheckRoll((5 * itemCount)) ? Run.instance.availableTier2DropList : Run.instance.availableTier1DropList;
            int item = Run.instance.treasureRng.RangeInt(0, list.Count);

            PickupDropletController.CreatePickupDroplet(list[item], transform.position, new Vector3(0, 0, 0));

            EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/impacteffects/PodGroundImpact"), new EffectData
            {
                origin = transform.position,
                scale = 15
            }, true);
        }
    }

    public class DormantFungusBehavior : CharacterBody.ItemBehavior
    {
        //VICE VS YOU
        //YOUUUUU

        private float timer;

        public Starstorm2ItemManager manager;
        public void Awake()
        {
            body = gameObject.GetComponent<CharacterBody>();
            manager = body.gameObject.AddOrGetComponent<Starstorm2ItemManager>();
        }
        public void FixedUpdate()
        {
            if (body.isSprinting)
            {
                timer += Time.deltaTime;
                if (timer >= 1f)
                {
                    manager.HealFractionAuthority(.02f * stack);
                    timer = 0;
                }
            }
            else
            {
                timer = 0;
            }
        }
    }
    public class DairyBehavior : CharacterBody.ItemBehavior
    {
        //VICE VS YOU
        //YOUUUUU
        private float timer;
        private bool fatalLogged;

        public Starstorm2ItemManager manager;
        public void Awake()
        {
            body = gameObject.GetComponent<CharacterBody>();
            manager = body.gameObject.AddOrGetComponent<Starstorm2ItemManager>();
        }

        public void FixedUpdate()
        {
            if (Run.instance && !Run.instance.isRunStopwatchPaused)
            {
                timer += Time.deltaTime;
                if (timer >= 3)
                {
                    if (body.teamComponent)
                    {
                        manager.AddExperienceAuthority((uint)(3 * stack));
                    }
                    else
                    {
                        if (!fatalLogged)
                        {
                            fatalLogged = true;
                            LogCore.LogF(Language.GetString(body.baseNameToken) + " doesn't have a team component!");
                        }
                    }
                    timer = 0;
                }
            }
        }
    }
    public class MetronomeBehavior : CharacterBody.ItemBehavior
    {
        public static float speedBoostMult = 2.0f;
        public float metroCharge;

        public void Awake()
        {
            body = gameObject.GetComponent<CharacterBody>();
            metroCharge = 0f;
        }

        public void FixedUpdate()
        {
            if (body.notMovingStopwatch > 0f)
            {
                if (metroCharge < 1.0f)
                {
                    metroCharge += Time.fixedDeltaTime / 4.0f;
                    metroCharge = Math.Min(metroCharge, 1.0f);
                    body.statsDirty = true;
                }
            }
            else if (metroCharge > 0f)
            {
                metroCharge -= Time.fixedDeltaTime / (2.0f * stack);
                metroCharge = Math.Max(metroCharge, 0f);
                body.statsDirty = true;
            }
        }
    }

    public class SigilBehavior : CharacterBody.ItemBehavior
    {
        private bool sigilActive = false;

        public void Awake()
        {
            body = gameObject.GetComponent<CharacterBody>();
        }

        public void FixedUpdate()
        {
            if (body.notMovingStopwatch > 0f && !sigilActive)
            {
                Debug.Log("sigil on");
                body.AddBuff(BuffCore.instance.sigilBuffIndex);
                //add visual effect
                sigilActive = true;
                body.statsDirty = true;
            }
            else if (body.notMovingStopwatch == 0f && sigilActive)
            {
                Debug.Log("sigil off");
                body.RemoveBuff(BuffCore.instance.sigilBuffIndex);
                //remove visual effect
                sigilActive = false;
                body.statsDirty = true;
            }
        }
    }
}
