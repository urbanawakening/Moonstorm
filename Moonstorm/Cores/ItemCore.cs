using Moonstorm.Cores.Components.Wyatt;
using R2API;
using RoR2;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Moonstorm.Cores
{
    class ItemCore
    {
        public static ItemCore instance;

        //TODO:
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
        protected internal ItemIndex experienceOnHitIndex;
        protected internal ItemIndex lemdogIndex;
        protected internal ItemIndex extendEnemyBuffDurationIndex;
        protected internal ItemIndex damageOnDamagedIndex;
        protected internal ItemIndex skinIndex;
        protected internal ItemIndex freezeEnemiesOnHitIndex;
        protected internal ItemIndex geigerCounterIndex;
        protected internal ItemIndex heartIndex;
        protected internal ItemIndex chipIndex;
        protected internal ItemIndex wyattWalkmanIndex;

        public List<ItemIndex> bossitemList = new List<ItemIndex>{
            ItemIndex.NovaOnLowHealth,
            ItemIndex.Knurl,
            ItemIndex.BeetleGland,
            ItemIndex.TitanGoldDuringTP,
            ItemIndex.SprintWisp,
            ItemIndex.Incubator,
            ItemIndex.BleedOnHitAndExplode,
            ItemIndex.FireballsOnHit 
            //Excluding pearls because those aren't boss Item, they come from the Cleansing Pool 
        };

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
            //BuffIndex.Cloak,
            BuffIndex.CloakSpeed,
            //BuffIndex.EngiShield,
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

        protected internal void RegisterItems()
        {
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
                pickupModelPath = "@Moonstorm:Assets/Moonstorm/Items/Grinder/MDLGrinder.prefab",
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
                pickupIconPath = "@Moonstorm:Assets/Moonstorm/Items/MechanicalTrinket/Icon.png",
                pickupModelPath = "@Moonstorm:Assets/Moonstorm/Items/MechanicalTrinket/MDLMechanicalTrinket.prefab",
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
                pickupModelPath = "@Moonstorm:Assets/Moonstorm/Items/FabinhoruDagger/MDLDagger.prefab",
                pickupToken = "ITEM_CRIPPLEONHIT_PICKUP",
                tags = crippleOnHitTags,
                tier = ItemTier.Tier2,
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
                pickupIconPath = "@Moonstorm:Assets/Moonstorm/Items/Cloak/JapeIcon.png",
                pickupModelPath = "@Moonstorm:Assets/Moonstorm/Items/Cloak/MDLCloak.prefab",
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
                name = "ItemOnLevelUp2",
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
                pickupModelPath = "@Moonstorm:Assets/Moonstorm/Items/UESKeycard/MDLKeycard.prefab",
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
                pickupIconPath = "@Moonstorm:Assets/Moonstorm/Items/Lemdog/LemDog_TexIcon.png",
                pickupModelPath = "@Moonstorm:Assets/Moonstorm/Items/Lemdog/MDLLemDog.prefab",
                pickupToken = "ITEM_LEMDOG_PICKUP",
                tags = betterCritsOnLowHealthTags,
                tier = ItemTier.Tier3,
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
                pickupIconPath = "@Moonstorm:Assets/Moonstorm/Items/Earrings/magicicon.png",
                pickupModelPath = "@Moonstorm:Assets/Moonstorm/Items/Earrings/MDLEarrings.prefab",
                pickupToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_PICKUP",
                tags = extendEnemyBuffDurationTags,
                tier = ItemTier.Tier2,
                unlockableName = ""
            });
            ItemTag[] damageOnDamageTags = new ItemTag[1]
{
                    ItemTag.Damage
};
            RegisterItem(new ItemDef
            {
                canRemove = true,
                //descriptionToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_DESC",
                hidden = false,
                loreToken = "",
                name = "DamageOnDamage",
                //nameToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_NAME",
                //pickupIconPath = "@Cloudburst:Assets/Items/Lemdog/LemDog_TexIcon.png",
                pickupModelPath = "@Moonstorm:Assets/Moonstorm/Items/RedSkull/MDLRedskull.prefab",
                //pickupToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_PICKUP",
                tags = damageOnDamageTags,
                tier = ItemTier.Tier2,
                unlockableName = ""
            });
            ItemTag[] freezeEnemiesOnHitTags = new ItemTag[2]
{
                    ItemTag.Damage,
                    ItemTag.Utility,
};
            RegisterItem(new ItemDef
            {
                canRemove = true,
                //descriptionToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_DESC",
                hidden = false,
                loreToken = "",
                name = "FreezeEnemiesOnHit",
                //nameToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_NAME",
                //pickupIconPath = "@Cloudburst:Assets/Items/Lemdog/LemDog_TexIcon.png",
                //pickupModelPath = "@Cloudburst:Assets/Items/Lemdog/MDLLemDog.prefab",
                //pickupToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_PICKUP",
                tags = freezeEnemiesOnHitTags,
                tier = ItemTier.Tier3,
                unlockableName = ""
            });
            ItemTag[] geigerCounterTags = new ItemTag[2]
{
                    ItemTag.Damage,
                    ItemTag.Utility,
};
            RegisterItem(new ItemDef
            {
                canRemove = true,
                //descriptionToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_DESC",
                hidden = false,
                loreToken = "",
                name = "GeigerCounter",
                //nameToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_NAME",
                pickupIconPath = "@Moonstorm:Assets/Moonstorm/Items/GeigerCounter/geigericon.png",
                pickupModelPath = "@Moonstorm:Assets/Moonstorm/Items/GeigerCounter/MDLGeigerCounter.prefab",
                //pickupToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_PICKUP",
                tags = geigerCounterTags,
                tier = ItemTier.Tier3,
                unlockableName = ""
            });
            ItemTag[] heartTags = new ItemTag[2]
{
                    ItemTag.AIBlacklist,
                    ItemTag.Utility
};
            RegisterItem(new ItemDef
            {
                canRemove = true,
                //descriptionToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_DESC",
                hidden = false,
                loreToken = "",
                name = "Heart",
                //nameToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_NAME",
                //pickupIconPath = "@Cloudburst:Assets/Items/Lemdog/LemDog_TexIcon.png",
                //pickupModelPath = "@Cloudburst:Assets/Items/Lemdog/MDLLemDog.prefab",
                //pickupToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_PICKUP",
                tags = heartTags,
                tier = ItemTier.Tier3,
                unlockableName = ""
            });
            ItemTag[] chipTags = new ItemTag[2]
{
                    ItemTag.AIBlacklist,
                    ItemTag.Damage
};
            RegisterItem(new ItemDef
            {
                canRemove = true,
                //descriptionToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_DESC",
                hidden = false,
                loreToken = "",
                name = "Chip",
                //nameToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_NAME",
                pickupIconPath = "@Moonstorm:Assets/Moonstorm/Items/MortarChip/chipIcon.png",
                pickupModelPath = "@Moonstorm:Assets/Moonstorm/Items/MortarChip/MDLMortarChip.prefab",
                //pickupToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_PICKUP",
                tags = chipTags,
                tier = ItemTier.Tier3,
                unlockableName = ""
            });
            ItemTag[] skinTags = new ItemTag[1]
            {
                    ItemTag.Utility,
            };
            RegisterItem(new ItemDef
            {
                canRemove = true,
                //descriptionToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_DESC",
                hidden = false,
                loreToken = "",
                name = "Skin",
                //nameToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_NAME",
                //pickupIconPath = "@Cloudburst:Assets/Items/Lemdog/LemDog_TexIcon.png",
                //pickupModelPath = "@Cloudburst:Assets/Items/Lemdog/MDLLemDog.prefab",
                //pickupToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_PICKUP",
                tags = skinTags,
                tier = ItemTier.Tier3,
                unlockableName = ""
            });
            ItemTag[] walkmanTags = new ItemTag[2]
            {
                    ItemTag.Utility,
                    ItemTag.WorldUnique
            };
            RegisterItem(new ItemDef
            {
                canRemove = false,
                //descriptionToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_DESC",
                hidden = false,
                loreToken = "",
                name = "WyattWalkman",
                //nameToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_NAME",
                //pickupIconPath = "@Cloudburst:Assets/Items/Lemdog/LemDog_TexIcon.png",
                //pickupModelPath = "@Cloudburst:Assets/Items/Lemdog/MDLLemDog.prefab",
                //pickupToken = "ITEM_EXTENDEDENEMYBUFFDURATIOM_PICKUP",
                tags = walkmanTags,
                tier = ItemTier.Tier3,
                unlockableName = ""
            });

            //chipIndex
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

            LanguageAPI.Add("ITEM_ITEMONLEVELUP_NAME", "Care Package Requester");
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

            LanguageAPI.Add("ITEM_EXPERIENCEONHIT_NAME", "Glass Harvester");
            LanguageAPI.Add("ITEM_EXPERIENCEONHIT_DESC", "Gain 0.5 <style=cStack>(+0.4 per stack)</style> <style=cIsUtility>experience</style> on hit.");
            LanguageAPI.Add("ITEM_EXPERIENCEONHIT_PICKUP", "Gain experience on hit");

            LanguageAPI.Add("ITEM_LEMDOG_NAME", "Lemdog");
            LanguageAPI.Add("ITEM_LEMDOG_DESC", "25% <style=cStack>(+2.5)<style=cStack> chance for <style=cIsUtility>applied debuffs to become beneficial buffs</style>");
            LanguageAPI.Add("ITEM_LEMDOG_PICKUP", "Chance for debuffs to become beneficial buffs when applied");

            LanguageAPI.Add("ITEM_BLOODTESTER_NAME", "Portable Blood Tester");
            LanguageAPI.Add("ITEM_BLOODTESTER_DESC", "Gain 5 <style=cStack>(+5 per stack)</style> <style=cIsUtility>money</style> when healed .");
            LanguageAPI.Add("ITEM_BLOODTESTER_PICKUP", "Gain money when healed. ");

            LanguageAPI.Add("ITEM_DAMAGEONDAMAGE_NAME", "Red Skull");
            LanguageAPI.Add("ITEM_DAMAGEONDAMAGE_DESC", "<style=cIsDamage>Deal more 25% <style=cStack>(+10% per stack)</style> more damage</style> when under 50% health.");
            LanguageAPI.Add("ITEM_DAMAGEONDAMAGE_PICKUP", "Damage nearby enemies when hurt. ");

            LanguageAPI.Add("ITEM_EXTENDEDENEMYBUFFDURATIOM_NAME", "Magician's Earrings");
            LanguageAPI.Add("ITEM_EXTENDEDENEMYBUFFDURATIOM_DESC", "Extend <style=cIsUtility>positive buffs applied to you</style> by 2<style=cStack>(+1 per stack)</style> seconds.");
            LanguageAPI.Add("ITEM_EXTENDEDENEMYBUFFDURATIOM_PICKUP", "Extend positive buff duration.");

            LanguageAPI.Add("ITEM_FREEZEENEMIESONHIT_NAME", "Relegdon's Veil");
            LanguageAPI.Add("ITEM_FREEZEENEMIESONHIT_DESC", "5% chance to <style=cIsUtility>freeze</style> enemies for 3 seconds <style=cStack>(+1 per stack)</style> seconds.");
            LanguageAPI.Add("ITEM_FREEZEENEMIESONHIT_PICKUP", "Chance to freeze enemies on hit.");

            LanguageAPI.Add("ITEM_GEIGERCOUNTER_NAME", "Geiger Counter");
            LanguageAPI.Add("ITEM_GEIGERCOUNTER_DESC", "Every 10 skill uses <style=cIsDamage>explode for 500% <style=cStack>(+250% damage per stack)</style> damage</style> blighting enemies.");
            LanguageAPI.Add("ITEM_GEIGERCOUNTER_PICKUP", "Using skills charges a large blast.");

            LanguageAPI.Add("ITEM_HEART_NAME", "Baboon Heart");
            LanguageAPI.Add("ITEM_HEART_DESC", "Getting hit below 50% health <style=cIsUtility>grants temporary invincibility for 2 seconds <style=cStack>(+1 second per stack)</style></style>. Recharge by killing enemies.");
            LanguageAPI.Add("ITEM_HEART_PICKUP", "Gain invincibility when hit at low health. Recharge by killing enemies.");

            LanguageAPI.Add("ITEM_CHIP_NAME", "Mortar Chip");
            LanguageAPI.Add("ITEM_CHIP_DESC", "Allies <style=cIsUtility>explode for 500%<style=cStack>(+100% per stack)</style></style>.");
            LanguageAPI.Add("ITEM_CHIP_PICKUP", "Allies explode on death!");

            LanguageAPI.Add("ITEM_SKIN_NAME", "Vigilante's Skin");
            LanguageAPI.Add("ITEM_SKIN_DESC", "For each enemy within an 10m radius gain <style=cIsUtility>+5 armor</style>. Maximum 25 <style=cStack>(+5 per stack)</style>");
            LanguageAPI.Add("ITEM_SKIN_PICKUP", "More enemies, more armor.");

            LanguageAPI.Add("ITEM_WYATTWALKMAN_NAME", "Walkman");
            LanguageAPI.Add("ITEM_WYATTWALKMAN_DESC", "Gain <style=cIsUtility>+x% speed</style> and <style=cIsHealing>+x% regen</style> while in combat every 3 seconds. Maximum 10 stacks <style=cStack>(+1 per stack)</style>");
            LanguageAPI.Add("ITEM_WYATTWALKMAN_PICKUP", "Gain speed while in combat");
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
                case "ItemOnLevelUp2":
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
                case "BarrierOnLevel":
                    barrierOnLevelIndex = ItemAPI.Add(customItem);
                    break;
                case "ExperienceOnHit":
                    experienceOnHitIndex = ItemAPI.Add(customItem);
                    break;
                case "Lemdog":
                    lemdogIndex = ItemAPI.Add(customItem);
                    break;
                case "ExtendedEnemyBuffDuration":
                    extendEnemyBuffDurationIndex = ItemAPI.Add(customItem);
                    break;
                case "DamageOnDamage":
                    damageOnDamagedIndex = ItemAPI.Add(customItem);
                    break;
                case "FreezeEnemiesOnHit":
                    freezeEnemiesOnHitIndex = ItemAPI.Add(customItem);
                    break;
                case "GeigerCounter":
                    geigerCounterIndex = ItemAPI.Add(customItem);
                    break;
                case "Heart":
                    heartIndex = ItemAPI.Add(customItem);
                    break;
                case "Chip":
                    chipIndex = ItemAPI.Add(customItem);
                    break;
                case "Skin":
                    skinIndex = ItemAPI.Add(customItem);
                    break;
                case "WyattWalkman":
                    wyattWalkmanIndex = ItemAPI.Add(customItem);
                    break;
                default:
                    LogCore.LogF(itemDef.name + " is unregistered! Item will not spawn!");
                    break;
            }
        }

        protected internal void Hook()
        {
            On.RoR2.GlobalEventManager.OnCrit += GlobalEventManager_OnCrit;
            On.RoR2.GlobalEventManager.OnInteractionBegin += GlobalEventManager_OnInteractionBegin;
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManagerOnOnCharacterDeath;
            On.RoR2.GlobalEventManager.OnTeamLevelUp += GlobalEventManager_OnTeamLevelUp;
            TeleporterInteraction.onTeleporterBeginChargingGlobal += TeleporterInteractionOnTeleporterBeginChargingGlobal;
            On.RoR2.GenericPickupController.GrantItem += GrantItem;
            On.RoR2.CharacterBody.AddTimedBuff += CharacterBody_AddTimedBuff1;
            On.RoR2.CharacterBody.AddTimedBuff += CharacterBody_AddTimedBuff;
            On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void CharacterBody_AddTimedBuff1(On.RoR2.CharacterBody.orig_AddTimedBuff orig, CharacterBody self, BuffIndex buffType, float duration)
        {
            if (self)
            {
                if (self.inventory)
                {
                    if (self.inventory.GetItemCount(lemdogIndex) > 0)
                    {
                        var def = BuffCatalog.GetBuffDef(buffType);
                        if (def.isDebuff && Util.CheckRoll(25 + (self.inventory.GetItemCount(lemdogIndex) * 2.5f), self.master))
                        {
                            var random = Random.Range(0, lemdogList.Count);
                            var buff = lemdogList[random];
                            buffType = buff;
                        }
                    }
                }
            }
            orig(self, buffType, duration);
        }

        private void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            //lol
            if (self)
            {
                if (self.inventory)
                {
                    var inventory = self.inventory;

                    var counterCount = inventory.GetItemCount(geigerCounterIndex);
                    var skinCount = inventory.GetItemCount(skinIndex);
                    var wyattCount = inventory.GetItemCount(wyattWalkmanIndex);

                    if (wyattCount > 0) {
                        self.AddItemBehavior<WyattItemWalkmanBehavior>(wyattCount);
                    }
                    if (counterCount > 0)
                    {
                        self.AddItemBehavior<GeigerCounterBehavior>(counterCount);
                    }
                    if (skinIndex > 0)
                    {
                        self.AddItemBehavior<SkinBehavior>(skinCount);
                    }
                }
            }
            orig(self);
        }

        private void CharacterBody_AddTimedBuff(On.RoR2.CharacterBody.orig_AddTimedBuff orig, CharacterBody self, BuffIndex buffType, float duration)
        {
            if (self)
            {
                var inv = self.inventory;
                if (inv)
                {
                    var earringsCount = inv.GetItemCount(extendEnemyBuffDurationIndex);
                    if (earringsCount > 0 && buffType != BuffIndex.MedkitHeal & buffType != BuffIndex.ElementalRingsCooldown)
                    {
                        //do thing???
                        duration += 1 + (1 * earringsCount);
                    }
                }
            }
            orig(self, buffType, duration);
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
                        var victimBody = self.body;

                        Inventory inv = attackerBody.inventory;
                        Inventory inv2 = null;

                        if (self.body && self.body.inventory)
                        {
                            inv2 = self.body.inventory;
                        }

                        int experienceOnHitCount = inv.GetItemCount(experienceOnHitIndex);
                        int heartCount = 0;

                        if (inv2)
                        {
                            heartCount = inv2.GetItemCount(heartIndex);
                        }
                        int damageOnDamagedCOunt = inv.GetItemCount(damageOnDamagedIndex);

                        if (damageOnDamagedCOunt > 0 && attackerBody.healthComponent && attackerBody.healthComponent.combinedHealthFraction < .5f)
                        {
                            float extra = damageInfo.damage * (.25f + (damageOnDamagedCOunt * .1f));

                            LogCore.LogD(damageInfo.damage);
                            LogCore.LogD(extra);

                            damageInfo.damage += extra;

                            LogCore.LogD(damageInfo.damage);
                        }

                        if (self.combinedHealthFraction < .4f && heartCount > 0)
                        {
                            if (victimBody && !victimBody.HasBuff(BuffCore.instance.baboomChargeIndex))
                            {
                                victimBody.AddTimedBuff(BuffIndex.HiddenInvincibility, 1 + (heartCount));
                                for (int i = 0; i < 15; i++)
                                {
                                    victimBody.AddBuff(BuffCore.instance.baboomChargeIndex);
                                }
                            }
                        }

                        if (experienceOnHitCount > 0 && attackerBody.teamComponent)
                        {
                            float exp = 0.1f + (experienceOnHitCount * 0.4f);
                            TeamManager.instance.GiveTeamExperience(attackerBody.teamComponent.teamIndex, (uint)exp);
                        }
                    }
                }
            }
            var combinedHealthb4Damage = self.combinedHealth;
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
            var victimBody = damageReport.victimBody;
            CharacterMaster attackerMaster = damageReport.attackerMaster;
            //CharacterMaster victimMaster = damageReport.victimMaster;
            Inventory attackerInventory = attackerMaster ? attackerMaster.inventory : null;
            //Inventory victimInventory = victimMaster ? victimMaster.inventory : null;


            int itemChampionOnKillCount = attackerInventory.GetItemCount(itemChampionOnKillIndex);
            int heartCount = attackerInventory.GetItemCount(heartIndex);
            int chipCount = Util.GetItemCountForTeam(TeamIndex.Player, chipIndex, false, true);

            if (chipCount > 0 && victimBody && victimBody.teamComponent && victimBody.teamComponent.teamIndex == TeamIndex.Player)
            {
                LogCore.LogI("fired");
                EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/LightningStakeNova"), new EffectData()
                {
                    scale = 25,
                    origin = victimBody.transform.position,
                    rotation = Quaternion.identity
                }, true);
                BlastAttack attack = new BlastAttack()
                {
                    attacker = victimBody.gameObject,
                    attackerFiltering = AttackerFiltering.Default,
                    baseDamage = (5 + chipCount) * victimBody.damage,
                    baseForce = 2500,
                    bonusForce = new Vector3(0, 0, 0),
                    crit = victimBody.RollCrit(),
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.BlightOnHit,
                    falloffModel = BlastAttack.FalloffModel.None,
                    impactEffect = EffectIndex.Invalid,
                    inflictor = victimBody.gameObject,
                    losType = BlastAttack.LoSType.NearestHit,
                    position = victimBody.transform.position,
                    procChainMask = default,
                    procCoefficient = 0.5f,
                    radius = 30,
                    teamIndex = victimBody.teamComponent.teamIndex,
                };
                attack.Fire();
            }

            /*if (chipCount > 0 && victimBody && victimBody.teamComponent && victimBody.teamComponent.teamIndex == TeamIndex.Player)
            {
                EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/impacteffects/ExplosionVFX"), new EffectData()
                {
                    scale = 25,
                    origin = victimBody.transform.position,
                    rotation = Quaternion.identity
                }, true);

                BlastAttack attack = new BlastAttack()
                {
                    attacker = victimBody.gameObject,
                    attackerFiltering = AttackerFiltering.Default,
                    baseDamage = (5 + chipCount) * victimBody.damage,
                    baseForce = 2500,
                    bonusForce = new Vector3(0, 0, 0),
                    crit = victimBody.RollCrit(),
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.BlightOnHit,
                    falloffModel = BlastAttack.FalloffModel.None,
                    impactEffect = EffectIndex.Invalid,
                    inflictor = victimBody.gameObject,
                    losType = BlastAttack.LoSType.NearestHit,
                    position = victimBody.transform.position,
                    procChainMask = default,
                    procCoefficient = 0.5f,
                    radius = 30,
                    teamIndex = victimBody.teamComponent.teamIndex,
                };
                attack.Fire();*/
            //}


            if (heartCount > 0 && attackerBody.HasBuff(BuffCore.instance.baboomChargeIndex))
            {
                attackerBody.RemoveBuff(BuffCore.instance.baboomChargeIndex);
            }

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
                int freezeCount = attackerInventory.GetItemCount(freezeEnemiesOnHitIndex);


                if (rootCount > 0 && Util.CheckRoll(20, attackerMaster) && attackerMaster && damageReport.victimBody)
                {
                    victimBody.AddTimedBuff(BuffIndex.Cripple, 3);
                }

                if (freezeCount > 0 && Util.CheckRoll(5, attackerMaster) && victimBody && !victimBody.isBoss)
                {
                    var setStateOnHurt = victimBody.GetComponent<SetStateOnHurt>();
                    if (setStateOnHurt)
                    {
                        setStateOnHurt.SetFrozen(5 + freezeCount);
                    }
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
            int count = Util.GetItemCountForTeam(TeamIndex.Player, largerTeleporterRadiusIndex, false, true);
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
    public class WyattItemWalkmanBehavior : CharacterBody.ItemBehavior
    {
        //VICE VS YOU
        //YOUUUUU
        private float timer;
        public void FixedUpdate()
        {
            if (!body.outOfCombat)
            {
                timer += Time.deltaTime;
                if (timer >= 3)
                {
                    body.gameObject.GetComponent<WyattWalkmanBehavior>().TriggerBehaviorAuthority();
                    timer = 0;
                }
            }
            else
            {
                body.gameObject.GetComponent<WyattWalkmanBehavior>().UntriggerAuthority();
                timer = 0;
            }
        }
    }
    public class DormantFungusBehavior : CharacterBody.ItemBehavior
    {
        //VICE VS YOU
        //YOUUUUU
        private float timer;
        public void FixedUpdate()
        {
            if (body.isSprinting)
            {
                timer += Time.deltaTime;
                if (timer >= 5)
                {
                    body.healthComponent.HealFraction(.1f * stack, default);
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
        public void FixedUpdate()
        {
            timer += Time.deltaTime;
            if (timer >= 3)
            {
                if (body.teamComponent)
                {
                    TeamManager.instance.GiveTeamExperience(body.teamComponent.teamIndex, (ulong)(1 * stack));
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
    public class GeigerCounterBehavior : CharacterBody.ItemBehavior
    {
        private float counter;
        public void Start()
        {
            On.RoR2.GenericSkill.OnExecute += GenericSkill_OnExecute;
        }

        private void GenericSkill_OnExecute(On.RoR2.GenericSkill.orig_OnExecute orig, GenericSkill self)
        {
            if (self.characterBody.Equals(body))
            {
                if (!self.characterBody.skillLocator.primary.Equals(self))
                {
                    counter += 1;
                }
            }
            orig(self);
        }

        public void FixedUpdate()
        {
            if (counter == 10)
            {
                LogCore.LogD("Fired!");

                EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/LightningStakeNova"), new EffectData()
                {
                    scale = 25,
                    origin = transform.position,
                    rotation = Quaternion.identity
                }, true);

                BlastAttack attack = new BlastAttack()
                {
                    attacker = base.gameObject,
                    attackerFiltering = AttackerFiltering.Default,
                    baseDamage = (2.5f + (2.5f * stack)) * body.damage,
                    baseForce = 2500,
                    bonusForce = new Vector3(0, 0, 0),
                    crit = body.RollCrit(),
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.BlightOnHit,
                    falloffModel = BlastAttack.FalloffModel.None,
                    impactEffect = EffectIndex.Invalid,
                    inflictor = base.gameObject,
                    losType = BlastAttack.LoSType.NearestHit,
                    position = base.transform.position,
                    procChainMask = default,
                    procCoefficient = 0.5f,
                    radius = 25,
                    teamIndex = body.teamComponent.teamIndex,
                };
                attack.Fire();
                counter = 0;
            }
        }
    }
    public class SkinBehavior : CharacterBody.ItemBehavior
    {
        private float timer;

        public void FixedUpdate()
        {
            timer += Time.deltaTime;
            //LogCore.LogI(timer);

            if (timer >= 1)
            {
                var count = body.GetBuffCount(BuffCore.instance.skinIndex);
                for (int i = 0; i < count; i++)
                {
                    body.RemoveBuff(BuffCore.instance.skinIndex);
                }

                var cap = 20 + stack;

                foreach (var collision in Physics.OverlapSphere(transform.position, 25))
                {
                    var cb = collision.GetComponentInParent<CharacterBody>();
                    if (cb && !cb.Equals(body) && body.GetBuffCount(BuffCore.instance.skinIndex) < cap)
                    {
                        body.AddBuff(BuffCore.instance.skinIndex);
                        //LogCore.LogI(Language.GetString(cb.baseNameToken));
                    }
                }
                timer = 0;
            }
        }
    }
}