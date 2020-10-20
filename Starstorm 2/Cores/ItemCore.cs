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

        private const string modPrefix = "@Starstorm2:";
        private const string prefabPath = modPrefix + "Assets/Items/";

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
            RegisterItem(new ItemDef
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
            RegisterItem(new ItemDef
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
            RegisterItem(new ItemDef
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
            RegisterItem(new ItemDef
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
            RegisterItem(new ItemDef
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
            RegisterItem(new ItemDef
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
            RegisterItem(new ItemDef
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
            RegisterItem(new ItemDef
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
            RegisterItem(new ItemDef
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
            RegisterItem(new ItemDef
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
            LanguageAPI.Add("ITEM_COIN_DESC", "<style=cIsHealing>4%</style> <style=cStack>(+3% per stack)</style> chance to <style=cIsDamage>ignite</style> enemies on hit for <style=cIsUtility>+$5</style>."); //by 2<style=cStack>(+1 per stack)</style> seconds.");
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
        }

        protected internal void RegisterItem(ItemDef itemDef)
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
            switch (itemDef.name)
            {
                case "LightningOnCrit":
                    damageOverTimeToLowHealthIndex = ItemAPI.Add(customItem);
                    break;
                case "BloodTester":
                    moneyOnHealIndex = ItemAPI.Add(customItem);
                    break;
                case "DormantFungus":
                    dormantFungusIndex = ItemAPI.Add(customItem);
                    break;
                case "Diary":
                    diaryIndex = ItemAPI.Add(customItem);
                    break;
                case "Coin":
                    coinIndex = ItemAPI.Add(customItem);
                    break;
                case "ItemOnLevelUp":
                    itemOnLevelUpIndex = ItemAPI.Add(customItem);
                    break;
                case "Sauce":
                    fireOnEquipmentUseIndex = ItemAPI.Add(customItem);
                    break;
                case "StrangeCan":
                    poisonOnHitIndex = ItemAPI.Add(customItem);
                    break;
                case "ErraticGadget":
                    erraticGadgetIndex = ItemAPI.Add(customItem);
                    break;
                case "PrototypeJetBoots":
                    explodeOnJumpIndex = ItemAPI.Add(customItem);
                    break;
                default:
                    LogCore.LogF(itemDef.name + " is unregistered! Item will not spawn!");
                    break;
            }
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
                if (self.inventory)
                {
                    var inventory = self.inventory;

                    var fungusCount = inventory.GetItemCount(dormantFungusIndex);
                    var diaryCount = inventory.GetItemCount(diaryIndex);
                    var erraticGadgetCount = inventory.GetItemCount(erraticGadgetIndex);


                    if (fungusCount > 0)
                    {
                        self.AddItemBehavior<DormantFungusBehavior>(fungusCount);
                    }
                    if (diaryCount > 0)
                    {
                        self.AddItemBehavior<DairyBehavior>(diaryCount);
                    }
                    if (erraticGadgetCount > 0)
                    {
                        //This is where you add the actual model of the item as a body attachment... I think
                    }
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
                                    Debug.Log(healAccum);
                                }
                            }
                            if (healAccum >= 15f)
                            {
                                healAccum -= 15f;
                                //sound/vfx here
                                TeamManager.instance.GiveTeamMoney(body.teamComponent.teamIndex, (uint)amountToGift);
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
                    if (attackerBody.inventory)
                    {
                        var victimBody = self.body;

                        Inventory inv = attackerBody.inventory;
                        Inventory inv2 = null;

                        if (self.body && self.body.inventory)
                        {
                            inv2 = self.body.inventory;
                        }

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
            if (self && damageInfo.attacker)
            {
                var inventory = damageInfo.attacker.GetComponent<CharacterBody>().inventory;
                if (inventory)
                {
                    var master = damageInfo.attacker.GetComponent<CharacterMaster>();
                    int itemCount = inventory.GetItemCount(poisonOnHitIndex);
                    int itemCount2 = inventory.GetItemCount(erraticGadgetIndex);
                    if (itemCount > 0)
                    {
                        GameObject chad = damageInfo.attacker;
                        bool flag = (damageInfo.damageType & DamageType.PoisonOnHit) > DamageType.Generic;
                        if ((itemCount > 0 || flag) && (flag || Util.CheckRoll((8.5f + (5f * (float)itemCount)) * damageInfo.procCoefficient, master)))
                        {
                            ProcChainMask procChainMask = damageInfo.procChainMask;
                            procChainMask.AddProc(ProcType.BleedOnHit);
                            DotController.InflictDot(victim, chad, DoTCore.instance.strangeCanPoison, 3.5f, 1f);
                        }
                    }
                    if (itemCount2 > 0)
                    {
                        if (damageInfo.crit)
                        {
                            GameObject erraticGadgetEffectPrefab = Resources.Load<GameObject>("prefabs/effects/tracers/TracerCaptainDefenseMatrix");
                            if (erraticGadgetEffectPrefab)
                            {
                                EffectData effectData = new EffectData
                                {
                                    origin = victim.GetComponent<CharacterBody>().corePosition,
                                    start = damageInfo.attacker.GetComponent<CharacterBody>().corePosition
                                };
                                EffectManager.SpawnEffect(erraticGadgetEffectPrefab, effectData, true);
                            }
                            DamageInfo newDamageInfo = damageInfo;
                            newDamageInfo.damage = damageInfo.damage * (.5f * itemCount2);
                            victim.GetComponent<HealthComponent>().TakeDamage(newDamageInfo);
                            //This needs to be edited so that the effect actually originates from a body attachment
                        }
                    }
                }
            }
            orig(self, damageInfo, victim);
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
                if (timer >= 5)
                {
                    manager.HealFractionAuthority(.1f * stack);
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
}
