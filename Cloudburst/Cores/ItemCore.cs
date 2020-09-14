using EntityStates.Mage.Weapon;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.Cores
{
    class ItemCore
    {
        public ItemCore instance;

        //TODO:
        //Fix OnCrit
        //Give Items Models
        //Give items icons
        //These values are set by RegisterItem(ItemIndex, ItemDef).
        protected internal ItemIndex itemChampionOnKillIndex;
        protected internal ItemIndex largerTeleporterRadiusIndex;
        protected internal ItemIndex crippleOnHitIndex;
        protected internal ItemIndex cloakOnInteractionIndex;
        protected internal ItemIndex itemOnLevelUpIndex;
        protected internal ItemIndex randomDebuffOnHitIndex;
        protected internal ItemIndex moneyOnInteractionIndex;
        protected internal ItemIndex barrierOnCritIndex;
        protected internal ItemIndex barrierOnLevelIndex;
        protected internal ItemIndex lightningOnCritIndex;
        protected internal ItemIndex experienceOnHitIndex;
        protected internal ItemIndex lemdogIndex;
        protected internal ItemIndex moneyOnHealIndex;
        protected internal ItemIndex extendEnemyBuffDurationIndex;
        protected internal ItemIndex skateboardIndex;

        public List<ItemIndex> bossitemList = new List<ItemIndex>{
            ItemIndex.NovaOnLowHealth,
            ItemIndex.Knurl,
            ItemIndex.BeetleGland,
            ItemIndex.TitanGoldDuringTP,
            ItemIndex.SprintWisp,
            ItemIndex.Incubator
            //Excluding pearls because those aren't boss Item, they come from the Cleansing Pool 
        };

        public HurtBox PickRandomTarget(Vector3 position, TeamIndex teamIndex, float range, GameObject filter)
        {
            //imagine stealing from game code
            //couldn't be me :^)

            BullseyeSearch search = new BullseyeSearch();
            search.teamMaskFilter = TeamMask.GetUnprotectedTeams(teamIndex);
            search.filterByLoS = true;
            search.searchOrigin = position;
            search.searchDirection = Vector3.zero;
            search.sortMode = BullseyeSearch.SortMode.Angle;
            search.maxDistanceFilter = range;

            search.RefreshCandidates();
            search.FilterOutGameObject(filter);

            //wouldn't it be really fucking funny
            //if this was the cause?
            //return search.GetResults().FirstOrDefault<HurtBox>();

            //it'd be really fucking funny if this caused a null reference 
            //nope, i think this did something??
            //i still got the error though
            var result = search.GetResults().FirstOrDefault<HurtBox>();
            if (result)
            {
                return result;
            }
            else return null;
        }

        public List<BuffIndex> scpBuffList = new List<BuffIndex>{
            BuffIndex.NullifyStack,
            BuffIndex.ClayGoo,
            BuffIndex.BeetleJuice,
            BuffIndex.HealingDisabled
        };
        public List<BuffIndex> eliteBuffList = new List<BuffIndex>{
            BuffIndex.AffixPoison,
            BuffIndex.AffixRed,
            BuffIndex.AffixBlue,
            BuffIndex.AffixWhite
        };
        public List<BuffIndex> lemdogList = new List<BuffIndex>{
            BuffIndex.Warbanner,
            BuffIndex.Cloak,
            BuffIndex.CloakSpeed,
            BuffIndex.EngiShield,
            BuffIndex.MeatRegenBoost,
            BuffIndex.WhipBoost,
            BuffIndex.TeamWarCry,
        };
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
        public ItemIndex GetRandomItem(List<ItemIndex> Item)
        {
            int itemID = UnityEngine.Random.Range(0, Item.Count);
            return Item[itemID];
        }

        public ItemCore() => RegisterItems();

        protected internal void RegisterItems() {
            LogCore.LogI("Initializing Core: " + base.ToString());

            instance = this;

            RegisterTokens();
            ItemTag[] itemChampionOnKillTags = new ItemTag[2]
            {
                ItemTag.OnKillEffect,
                ItemTag.AIBlacklist
            };
            RegisterItem(new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_ITEMONCHAMPIONKILL_DESC",
                hidden = false,
                loreToken = "ITEM_ITEMONCHAMPIONKILL_LORE",
                name = "ItemChampionOnKill",
                nameToken = "ITEM_ITEMONCHAMPIONKILL_NAME",
                //pickupIconPath = pickUpIconPath, 
                pickupModelPath = "@Cloudburst:Assets/Items/Grinder/MDLGrinder.prefab",
                pickupToken = "ITEM_ITEMONCHAMPIONKILL_PICKUP",
                tags = itemChampionOnKillTags,
                tier = ItemTier.Tier3,
                unlockableName = ""
            });
            ItemTag[] largerTeleporterRadiusTags = new ItemTag[2]
            {
                ItemTag.AIBlacklist,
                ItemTag.Utility
            };
            RegisterItem(new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_LARGERTELEPORTERRADIUS_DESC",
                hidden = false,
                loreToken = "",
                name = "LargerTeleporterRadius",
                nameToken = "ITEM_LARGERTELEPORTERRADIUS_NAME",
                pickupIconPath = "@Cloudburst:Assets/Items/MechanicalTrinket/Icon.png",
                pickupModelPath = "@Cloudburst:Assets/Items/MechanicalTrinket/MDLMechanicalTrinket.prefab",
                pickupToken = "ITEM_LARGERTELEPORTERRADIUS_PICKUP",
                tags = largerTeleporterRadiusTags,
                tier = ItemTier.Tier2,
                unlockableName = ""
            });
            ItemTag[] crippleOnHitTags = new ItemTag[1]
            {
                ItemTag.Damage
            };
            RegisterItem(new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_CRIPPLEONHIT_DESC",
                hidden = false,
                loreToken = "",
                name = "CrippleOnHit",
                nameToken = "ITEM_CRIPPLEONHIT_NAME",
                //pickupIconPath = pickUpIconPath,
                pickupModelPath = "@Cloudburst:Assets/Items/FabinhoruDagger/MDLDagger.prefab",
                pickupToken = "ITEM_CRIPPLEONHIT_PICKUP",
                tags = crippleOnHitTags,
                tier = ItemTier.Tier1,
                unlockableName = ""
            });
            ItemTag[] cloakOnInteractionTags = new ItemTag[2]
            {
                ItemTag.AIBlacklist,
                ItemTag.Utility
            };
            //string unlockAble = Main.unlockAll.Value ? "CLOUDBURST_GRABORDIE_REWARD_ID" : "";
            RegisterItem(new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_CLOAKBUFFONINTERACTION_DESC",
                hidden = false,
                loreToken = "",
                name = "CloakOnInteraction",
                nameToken = "ITEM_CLOAKBUFFONINTERACTION_NAME",
                //pickupIconPath = pickUpIconPath,
                //pickupModelPath = pickUpModelPath,
                pickupToken = "ITEM_CLOAKBUFFONINTERACTION_PICKUP",
                tags = cloakOnInteractionTags,
                tier = ItemTier.Tier2,
                unlockableName = AchievementCore.GetUnlockableString("GrabOrDie")
            });
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
                //pickupIconPath = pickUpIconPath,
                //pickupModelPath = pickUpModelPath,
                pickupToken = "ITEM_ITEMONLEVELUP_PICKUP",
                tags = itemOnLevelUpTags,
                tier = ItemTier.Tier3,
                unlockableName = ""
            });
            ItemTag[] randomDebuffOnHitTags = new ItemTag[2]
            {
                ItemTag.Utility,
                ItemTag.Damage
            };
            RegisterItem(new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_RANDOMDEBUFFONHIT_DESC",
                hidden = false,
                loreToken = "",
                name = "RandomDebuffOnHit",
                nameToken = "ITEM_RANDOMDEBUFFONHIT_NAME",
                //pickupIconPath = pickUpIconPath,
                //pickupModelPath = pickUpModelPath,
                pickupToken = "ITEM_RANDOMDEBUFFONHIT_PICKUP",
                tags = randomDebuffOnHitTags,
                tier = ItemTier.Tier3,
                unlockableName = ""
            });
            ItemTag[] moneyOnInteractionTags = new ItemTag[2]
            {
                ItemTag.Utility,
                ItemTag.AIBlacklist
            };
            RegisterItem(new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_MONEYONINTERACTION_DESC",
                hidden = false,
                loreToken = "",
                name = "MoneyOnInteraction",
                nameToken = "ITEM_MONEYONINTERACTION_NAME",
                //pickupIconPath = pickUpIconPath,
                pickupModelPath = "@Cloudburst:Assets/Items/UESKeycard/MDLKeycard.prefab",
                pickupToken = "ITEM_MONEYONINTERACTION_PICKUP",
                tags = moneyOnInteractionTags,
                tier = ItemTier.Tier1,
                unlockableName = ""
            });
            ItemTag[] barrierOnCritTags = new ItemTag[2]
            {
                    ItemTag.Utility,
                    ItemTag.Healing
            };
            RegisterItem(new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_BARRIERONCRIT_DESC",
                hidden = false,
                loreToken = "",
                name = "BarrierOnCrit",
                nameToken = "ITEM_BARRIERONCRIT_NAME",
                //pickupIconPath = pickUpIconPath,
                //pickupModelPath = pickUpModelPath,
                pickupToken = "ITEM_BARRIERONCRIT_PICKUP",
                tags = barrierOnCritTags,
                tier = ItemTier.Tier2,
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
                //pickupIconPath = pickUpIconPath,
                pickupModelPath = "@Cloudburst:Assets/Items/LightningEye/MDLLightningEye.prefab", //
                pickupToken = "ITEM_LIGHTNINGONCRIT_PICKUP",
                tags = lightningOnCritTags,
                tier = ItemTier.Tier1,
                unlockableName = ""
            });
            ItemTag[] barrierOnLevelTags = new ItemTag[2]
            {
                    ItemTag.Utility,
                    ItemTag.Healing
            };
            RegisterItem(new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_BARRIERONLEVEL_DESC",
                hidden = false,
                loreToken = "",
                name = "BarrierOnLevel",
                nameToken = "ITEM_BARRIERONLEVEL_NAME",
                //pickupIconPath = pickUpIconPath,
                //pickupModelPath = pickUpModelPath,
                pickupToken = "ITEM_BARRIERONLEVEL_PICKUP",
                tags = barrierOnLevelTags,
                tier = ItemTier.Tier1,
                unlockableName = ""
            });
            ItemTag[] experienceOnHitTags = new ItemTag[2]
            {
                    ItemTag.Utility,
                    ItemTag.AIBlacklist
            };
            RegisterItem(new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_EXPERIENCEONHIT_DESC",
                hidden = false,
                loreToken = "",
                name = "ExperienceOnHit",
                nameToken = "ITEM_EXPERIENCEONHIT_NAME",
                //pickupIconPath = pickUpIconPath,
                //pickupModelPath = pickUpModelPath,
                pickupToken = "ITEM_EXPERIENCEONHIT_PICKUP",
                tags = experienceOnHitTags,
                tier = ItemTier.Tier1,
                unlockableName = AchievementCore.GetUnlockableString("HitLevelCap")
            });
            ItemTag[] betterCritsOnLowHealthTags = new ItemTag[2]
            {
                    ItemTag.Damage,
                    ItemTag.AIBlacklist
            };
            RegisterItem(new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_LEMDOG_DESC",
                hidden = false,
                loreToken = "",
                name = "Lemdog",
                nameToken = "ITEM_LEMDOG_NAME",
                pickupIconPath = "@Cloudburst:Assets/Items/Lemdog/LemDog_TexIcon.png",
                pickupModelPath = "@Cloudburst:Assets/Items/Lemdog/MDLLemDog.prefab",
                pickupToken = "ITEM_LEMDOG_PICKUP",
                tags = betterCritsOnLowHealthTags,
                tier = ItemTier.Tier3,
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
                //pickupIconPath = "@Cloudburst:Assets/Items/Lemdog/LemDog_TexIcon.png",
                ////pickupModelPath = "@Cloudburst:Assets/Items/Lemdog/MDLLemDog.prefab",
                pickupToken = "ITEM_BLOODTESTER_PICKUP",
                tags = moneyOnHealTags,
                tier = ItemTier.Tier2,
                unlockableName = ""
            });
            ItemTag[] extendEnemyBuffDurationTags = new ItemTag[2]
            {
                    ItemTag.Utility,
                    ItemTag.AIBlacklist
            };
            RegisterItem(new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_DESC",
                hidden = false,
                loreToken = "",
                name = "ExtendedEnemyBuffDuration",
                nameToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_NAME",
                //pickupIconPath = "@Cloudburst:Assets/Items/Lemdog/LemDog_TexIcon.png",
                ////pickupModelPath = "@Cloudburst:Assets/Items/Lemdog/MDLLemDog.prefab",
                pickupToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_PICKUP",
                tags = extendEnemyBuffDurationTags,
                tier = ItemTier.Tier2,
                unlockableName = ""
            });
            ItemTag[] redSkateboardTags = new ItemTag[2]
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
                name = "RedSkateboard",
                //nameToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_NAME",
                //pickupIconPath = "@Cloudburst:Assets/Items/Lemdog/LemDog_TexIcon.png",
                //pickupModelPath = "@Cloudburst:Assets/Items/Lemdog/MDLLemDog.prefab",
                //pickupToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_PICKUP",
                tags = redSkateboardTags,
                tier = ItemTier.Tier3,
                unlockableName = ""
            });
            Hook();
        }

        protected internal void RegisterTokens()
        {
            LanguageAPI.Add("ITEM_ITEMONCHAMPIONKILL_NAME", "Extractor"); //model
            LanguageAPI.Add("ITEM_ITEMONCHAMPIONKILL_DESC", "<style=cIsUtility>25% chance <style=cStack>(+10% per stack)</style> for bosses to drop a random boss item when killed.</style>");
            LanguageAPI.Add("ITEM_ITEMONCHAMPIONKILL_PICKUP", "Chance for bosses to drop boss items when killed");

            LanguageAPI.Add("ITEM_LARGERTELEPORTERRADIUS_NAME", "Mechanical Trinket"); //model
            LanguageAPI.Add("ITEM_LARGERTELEPORTERRADIUS_DESC", "<style=cIsUtility>Increase teleporter radius by 3m</style> <style=cStack>(+3m per stack)</style>.");
            LanguageAPI.Add("ITEM_LARGERTELEPORTERRADIUS_PICKUP", "Increase teleporter radius.");

            LanguageAPI.Add("ITEM_CRIPPLEONHIT_NAME", "Fabinhoru's Dagger");
            LanguageAPI.Add("ITEM_CRIPPLEONHIT_DESC", "20% Chance to <style=cIsDamage>cripple enemies</style> for 3 <style=cStack>(+3 seconds per stack)</style> seconds. ");
            LanguageAPI.Add("ITEM_CRIPPLEONHIT_PICKUP", "Chance to cripple enemies.");

            LanguageAPI.Add("ITEM_CLOAKBUFFONINTERACTION_NAME", "Jape's Cloak");
            LanguageAPI.Add("ITEM_CLOAKBUFFONINTERACTION_DESC", "Become <style=cIsUtility>cloaked for 2 seconds</style> <style=cStack>(+2 seconds per stack)</style> when picking up an item.");
            LanguageAPI.Add("ITEM_CLOAKBUFFONINTERACTION_PICKUP", "Become cloaked when picking up an item.");

            LanguageAPI.Add("ITEM_ITEMONLEVELUP_NAME", "Dropship Caller");
            LanguageAPI.Add("ITEM_ITEMONLEVELUP_DESC", "<style=cIsUtility>75% Chance to gain an item on level up</style>. <style=cStack>+5% chance to gain a green item per stack</style>");
            LanguageAPI.Add("ITEM_ITEMONLEVELUP_PICKUP", "Chance to gain an item on level up");

            LanguageAPI.Add("ITEM_RANDOMDEBUFFONHIT_NAME", "[REDACTED]");
            LanguageAPI.Add("ITEM_RANDOMDEBUFFONHIT_DESC", "[REDACTED]");
            LanguageAPI.Add("ITEM_RANDOMDEBUFFONHIT_PICKUP", "[REDACTED]");
            LanguageAPI.Add("ITEM_RANDOMDEBUFFONHIT_LORE", "Order: \u201C[REDACTED]\u201D\r\nTracking Number: [REDACTED]\r\nEstimated Delivery: [REDACTED]\r\nShipping Method: [REDACTED]\r\nShipping Address: [REDACTED], [REDACTED]\r\nShipping Details:\r\n\r\nSecure, contain, protect.\r\n");

            LanguageAPI.Add("ITEM_MONEYONINTERACTION_NAME", "UES Keycard");
            LanguageAPI.Add("ITEM_MONEYONINTERACTION_DESC", "Gain <style=cIsUtility>10 dollars</style> <style=cStack>(+5 dollars chance per stack)</style> upon opening a chest. ");
            LanguageAPI.Add("ITEM_MONEYONINTERACTION_PICKUP", "Activating an interactable gives you money");

            LanguageAPI.Add("ITEM_BARRIERONCRIT_NAME", "Topaz Lense");
            LanguageAPI.Add("ITEM_BARRIERONCRIT_DESC", "Gain a <style=cIsHealing>temporary barrier</style> on critical hits for <style=cIsHealing>10 health</style> <style=cStack>(+2.5 per stack)</style>.");
            LanguageAPI.Add("ITEM_BARRIERONCRIT_PICKUP", "Gain barrier on critical hits");

            LanguageAPI.Add("ITEM_BARRIERONLEVEL_NAME", "Topaz Crown");
            LanguageAPI.Add("ITEM_BARRIERONLEVEL_DESC", "Gain a <style=cIsHealing>temporary barrier</style> on level up for <style=cIsHealing>30 health</style> <style=cStack>(+5 per stack)</style>.");
            LanguageAPI.Add("ITEM_BARRIERONLEVEL_PICKUP", "Gain barrier on level up");

            LanguageAPI.Add("ITEM_LIGHTNINGONCRIT_NAME", "Detritive Trematode"); //model(??)
            LanguageAPI.Add("ITEM_LIGHTNINGONCRIT_DESC", "Enemies with low health recieve a <style=cIsDamage>damage over time</style> that deals 100%<style=cStack>(+100% per stack)</style> damage.");
            LanguageAPI.Add("ITEM_LIGHTNINGONCRIT_PICKUP", "Low health enemies recieve damage over time.");

            LanguageAPI.Add("ITEM_EXPERIENCEONHIT_NAME", "Glass Harvester");
            LanguageAPI.Add("ITEM_EXPERIENCEONHIT_DESC", "Gain 0.5 <style=cStack>(+0.4 per stack)</style> <style=cIsUtility>experience</style> on hit.");
            LanguageAPI.Add("ITEM_EXPERIENCEONHIT_PICKUP", "Gain experience on hit");

            LanguageAPI.Add("ITEM_LEMDOG_NAME", "Lemdog");
            LanguageAPI.Add("ITEM_LEMDOG_DESC", "25% <style=cStack>(+2.5)<style=cStack> chance for <style=cIsUtility>applied debuffs to become beneficial buffs</style>");
            LanguageAPI.Add("ITEM_LEMDOG_PICKUP", "Chance for debuffs to become beneficial buffs when applied");

            LanguageAPI.Add("ITEM_BLOODTESTER_NAME", "Portable Blood Tester");
            LanguageAPI.Add("ITEM_BLOODTESTER_DESC", "Gain 5 <style=cStack>(+5 per stack)</style> <style=cIsUtility>money</style> when healed .");
            LanguageAPI.Add("ITEM_BLOODTESTER_PICKUP", "Gain money when healed. ");

            LanguageAPI.Add("ITEM_EXTENDEDENEMYBUFFDURATIOM_NAME", "Magican's Earrings");
            LanguageAPI.Add("ITEM_EXTENDEDENEMYBUFFDURATIOM_DESC", "Extend <style=cIsUtility>positive buffs applied to you</style> by 2<style=cStack>(+1 per stack)</style> seconds.");
            LanguageAPI.Add("ITEM_EXTENDEDENEMYBUFFDURATIOM_PICKUP", "Extend positive buff duration.");

            LanguageAPI.Add("ITEM_REDSKATEBOARD_NAME", "Red Skateboard");
            LanguageAPI.Add("ITEM_REDSKATEBOARD_DESC", "<style=cIsUtility>Sprint</style> during attacks where you wouldn't be able to."); //by 2<style=cStack>(+1 per stack)</style> seconds.");
            LanguageAPI.Add("ITEM_REDSKATEBOARD_PICKUP", "Sprint while attacking.");
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
                case "ItemChampionOnKill":
                    itemChampionOnKillIndex = ItemAPI.Add(customItem);
                    break;
                case "LargerTeleporterRadius":
                    largerTeleporterRadiusIndex = ItemAPI.Add(customItem);
                    break;
                case "CrippleOnHit":
                    crippleOnHitIndex = ItemAPI.Add(customItem);
                    break;
                case "CloakOnInteraction":
                    cloakOnInteractionIndex = ItemAPI.Add(customItem);
                    break;
                case "ItemOnLevelUp":
                    itemOnLevelUpIndex = ItemAPI.Add(customItem);
                    break;
                case "RandomDebuffOnHit":
                    randomDebuffOnHitIndex = ItemAPI.Add(customItem);
                    break;
                case "MoneyOnInteraction":
                    moneyOnInteractionIndex = ItemAPI.Add(customItem);
                    break;
                case "BarrierOnCrit":
                    barrierOnCritIndex = ItemAPI.Add(customItem);
                    break;
                case "LightningOnCrit":
                    lightningOnCritIndex = ItemAPI.Add(customItem);
                    break;
                case "BarrierOnLevel":
                    barrierOnLevelIndex = ItemAPI.Add(customItem);
                    break;
                case "ExperienceOnHit":
                    experienceOnHitIndex = ItemAPI.Add(customItem);
                    break;
                case "Lemdog":
                    lemdogIndex = ItemAPI.Add(customItem);
                    break;
                case "BloodTester":
                    moneyOnHealIndex = ItemAPI.Add(customItem);
                    break;
                case "ExtendedEnemyBuffDuration":
                    extendEnemyBuffDurationIndex = ItemAPI.Add(customItem);
                    break;
                case "RedSkateboard":
                    skateboardIndex = ItemAPI.Add(customItem);
                    break;
                    default:
                    LogCore.LogF("Unregistered item!");
                    break;
            }
        }

        protected internal void Hook() {
            On.RoR2.GlobalEventManager.OnCrit += GlobalEventManager_OnCrit;
            On.RoR2.GlobalEventManager.OnInteractionBegin += GlobalEventManager_OnInteractionBegin;
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManagerOnOnCharacterDeath;
            On.RoR2.GlobalEventManager.OnTeamLevelUp += GlobalEventManager_OnTeamLevelUp;
            TeleporterInteraction.onTeleporterBeginChargingGlobal += TeleporterInteractionOnTeleporterBeginChargingGlobal;
            On.RoR2.GenericPickupController.GrantItem += GrantItem;
            On.RoR2.CharacterBody.AddBuff += CharacterBody_AddBuff;
            On.RoR2.HealthComponent.Heal += HealthComponent_Heal;
            On.RoR2.CharacterBody.AddTimedBuff += CharacterBody_AddTimedBuff;
            On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            //lol
            if (self) {
                if (self.inventory) {
                    var inventory = self.inventory;
                    var skateboardCount = inventory.GetItemCount(skateboardIndex);
                    if (skateboardCount > 0) {
                        var skillLocator = self.skillLocator;
                        if (skillLocator) {
                            skillLocator.primary.skillDef.noSprint = false;
                            LogCore.LogI("applied!");
                        }
                    }
                }
            } 
            orig(self);
        }

        private void CharacterBody_AddTimedBuff(On.RoR2.CharacterBody.orig_AddTimedBuff orig, CharacterBody self, BuffIndex buffType, float duration)
        {
            if (self) {
                var inv = self.inventory;
                if (inv) {
                    var earringsCount = inv.GetItemCount(extendEnemyBuffDurationIndex);
                    if (earringsCount > 0 && buffType != BuffIndex.MedkitHeal) {
                        //do thing???
                        duration += 1 + (1 * earringsCount);
                    }
                }
            }
            orig(self, buffType, duration);
        }

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
                        if (bloodTesters > 0 && body.teamComponent && body.isPlayerControlled && nonRegen == true) {
                            TeamManager.instance.GiveTeamMoney(body.teamComponent.teamIndex, (uint)amountToGift);
                        }
                    }
                }
            }
            return orig(self, amount, procChainMask, nonRegen);
        }

        private void CharacterBody_AddBuff(On.RoR2.CharacterBody.orig_AddBuff orig, CharacterBody self, BuffIndex buffType)
        {
            if (self) {
                if (self.inventory)
                {
                    if (self.inventory.GetItemCount(lemdogIndex) > 0) {
                        var def = BuffCatalog.GetBuffDef(buffType);
                        if (def.isDebuff && Util.CheckRoll(25 + (self.inventory.GetItemCount(lemdogIndex) * 2.5f), self.master)) {
                            var random = UnityEngine.Random.Range(0, lemdogList.Count);
                            var buff = lemdogList[random];
                            buffType = buff;
                        }
                    }
                }
            }
            orig(self, buffType);

        }

        public void GrantItem(On.RoR2.GenericPickupController.orig_GrantItem orig, GenericPickupController self, CharacterBody body, Inventory inventory)
        {
            orig(self, body, inventory);
            int cloakOnInteractionCount = inventory.GetItemCount(cloakOnInteractionIndex);
            if (self && inventory && cloakOnInteractionCount > 0)
            {
                body.AddTimedBuff(BuffIndex.Cloak, 2 * cloakOnInteractionCount);
            }
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
                        Inventory inv = attackerBody.inventory;
                        int experienceOnHitCount = inv.GetItemCount(experienceOnHitIndex);
                        int lightningOnCritCOunt = inv.GetItemCount(lightningOnCritIndex);
                        if (experienceOnHitCount > 0 && attackerBody.teamComponent)
                        {
                            float exp = 0.1f + (experienceOnHitCount * 0.4f);
                            TeamManager.instance.GiveTeamExperience(attackerBody.teamComponent.teamIndex, (ulong)(uint)exp);
                        }
                        if (self.combinedHealthFraction < .6f && lightningOnCritCOunt > 0) {
                            DotController.InflictDot(self.gameObject, attacker, DoTCore.instance.detritive, 5, 1);
                        }
                    }
                }
            }
            orig(self, damageInfo);
        }
        #region GlobalEventManager
        public void GlobalEventManager_OnInteractionBegin(On.RoR2.GlobalEventManager.orig_OnInteractionBegin orig, GlobalEventManager self, Interactor interactor, IInteractable interactable, UnityEngine.GameObject interactableObject)
        {
            orig(self, interactor, interactable, interactableObject);
            CharacterBody characterBody = interactor.GetComponent<CharacterBody>();
            if (characterBody)
            {
                Inventory inventory = characterBody.inventory;
                if (inventory && !interactableObject.HasComponent<GenericPickupController>())
                {
                    int moneyOnInteractionCount = inventory.GetItemCount(moneyOnInteractionIndex);

                    if (moneyOnInteractionCount > 0)
                    {
                        TeamManager.instance.GiveTeamMoney(characterBody.teamComponent.teamIndex, 5U + (uint)(moneyOnInteractionCount * 5));
                        //characterBody.master.GiveMoney(5U + (uint)(moneyOnInteractionCount * 5));
                    }
                }
            }
        }

        public void GlobalEventManager_OnCrit(On.RoR2.GlobalEventManager.orig_OnCrit orig, GlobalEventManager self, CharacterBody body, CharacterMaster master, float procCoefficient, ProcChainMask procChainMask)
        {
			if (body && procCoefficient > 0f && body && master && master.inventory)
            {
                var masterInventory = master.inventory;
                var healthComponent = body.healthComponent;
                int topazLense = masterInventory.GetItemCount(barrierOnCritIndex);
                //int lightningEye = masterInventory.GetItemCount(lightningOnCritIndex);
                if (topazLense > 0 && healthComponent)
                {
                    //oddly, this doesn't null reference
                    healthComponent.AddBarrier(7.5f + (topazLense * 2.5f));
                }
            }
            orig(self, body, master, procCoefficient, procChainMask);
        }

        public void GlobalEventManagerOnOnCharacterDeath(DamageReport damageReport)
        {
            if (!damageReport.attackerBody || !damageReport.victimBody || !damageReport.attacker || !damageReport.victim || !damageReport.attackerMaster || !damageReport.victimMaster || damageReport == null)
                return;
            CharacterBody attackerBody = damageReport.attackerBody;
            //CharacterBody victimBody = damageReport.victimBody;
            CharacterMaster attackerMaster = damageReport.attackerMaster;
            //CharacterMaster victimMaster = damageReport.victimMaster;
            Inventory attackerInventory = attackerMaster ? attackerMaster.inventory : null;
            //Inventory victimInventory = victimMaster ? victimMaster.inventory : null;

            int itemChampionOnKillCount = attackerInventory.GetItemCount(itemChampionOnKillIndex);
            if (itemChampionOnKillCount > 0 && attackerMaster && Util.CheckRoll(15 + (itemChampionOnKillCount * 10)) && damageReport.victim.body.isBoss == true)
            {
                Util.PlaySound("ui_obj_casinoChest_open", attackerBody.gameObject);
                attackerInventory.GiveItem(GetRandomItem(bossitemList), 1);
            }
        }

        public void GlobalEventManager_onServerDamageDealt(DamageReport damageReport)
        {
            if (!damageReport.attackerBody || !damageReport.victimBody || !damageReport.attacker || !damageReport.victim || !damageReport.attackerMaster || !damageReport.victimMaster || damageReport == null)
                return;
            if (damageReport.attackerBody && damageReport != null)
            {
                //CharacterBody attackerBody = damageReport.attackerBody;
                CharacterBody victimBody = damageReport.victimBody;
                CharacterBody attackerBody = damageReport.attackerBody;
                CharacterMaster attackerMaster = damageReport.attackerMaster;
                CharacterMaster victimMaster = damageReport.victimMaster;
                Inventory attackerInventory = attackerMaster ? attackerMaster.inventory : null;
                //Inventory victimInventory = victimMaster ? victimMaster.inventory : null;
                int scpRandom = UnityEngine.Random.Range(0, scpBuffList.Count);
                int rootCount = attackerInventory.GetItemCount(crippleOnHitIndex);
                int scpCount = attackerInventory.GetItemCount(randomDebuffOnHitIndex);
                if (rootCount > 0 && Util.CheckRoll(20, attackerMaster) && attackerMaster && damageReport.victimBody)
                {
                    victimBody.AddTimedBuff(BuffIndex.Cripple, 3);
                }
                if (scpCount > 0 && damageReport.victimBody && victimMaster)
                {
                    victimBody.AddTimedBuff(scpBuffList[scpRandom], (scpCount * 2));
                }

            }
        }

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
                                int barrierOnLevelUpCount = master.inventory.GetItemCount(barrierOnLevelIndex);

                                if (itemOnLevelUpCount > 0 && Util.CheckRoll(50, master))
                                {
                                    DropShipCall(characterBody.transform, itemOnLevelUpCount);

                                }
                                if (barrierOnLevelUpCount > 0)
                                {
                                    if (characterBody.healthComponent)
                                    {
                                        characterBody.healthComponent.AddBarrier(25 + (5 * barrierOnLevelUpCount));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            orig(teamIndex);
        }
        #endregion

        public void TeleporterInteractionOnTeleporterBeginChargingGlobal(TeleporterInteraction obj)
        {
            int count = Util.GetItemCountForTeam(TeamIndex.Player, largerTeleporterRadiusIndex, true, true);
            if (count > 0)
            {
                obj.holdoutZoneController.baseRadius = obj.holdoutZoneController.baseRadius + (count * 3);
            }
        }

        public void EquipmentCatalog_RegisterEquipment(On.RoR2.EquipmentCatalog.orig_RegisterEquipment orig, EquipmentIndex equipmentIndex, EquipmentDef equipmentDef)
        {
            if (equipmentIndex == EquipmentIndex.Meteor)
            {
                equipmentDef.enigmaCompatible = false;
            }
            orig(equipmentIndex, equipmentDef);
        }
    }
}