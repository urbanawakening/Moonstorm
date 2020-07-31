using R2API;
using RoR2;
using UnityEngine;

namespace Cloudburst.Items
{
    public sealed class Item
    {
        #region ItemIndexes
        public static ItemIndex itemChampionOnKillIndex { get; private set; }
        public static ItemIndex largerTeleporterRadiusIndex { get; private set; }
        public static ItemIndex eliteBuffOnInteractionIndex { get; private set; }
        public static ItemIndex crippleOnHitIndex { get; private set; }
        public static ItemIndex cloakOnInteractionIndex { get; private set; }
        public static ItemIndex itemOnLevelUpIndex { get; private set; }
        public static ItemIndex randomDebuffOnHitIndex { get; private set; }
        public static ItemIndex moneyOnInteractionIndex { get; private set; }
        public static ItemIndex barrierOnCritIndex { get; private set; }
        public static ItemIndex barrierOnLevelIndex { get; private set; }
        public static ItemIndex lightningOnCritIndex { get; private set; }
        public static ItemIndex experienceOnHitIndex { get; private set; }
        public static ItemIndex betterCritDamageOnLowHealthIndex { get; private set; }
        public static ItemIndex increasedJumpHeightIndex { get; private set; }
        #endregion
        public Item()
        {
            #region Basic
            //ITEM RULES//
            //-Items must be original!! Every item should have a seperate effect and shouldn't just be X item but better.
            //-Item names must fit RoR2!
            //-Lastly, items need to be fun!

            var prefab = Resources.Load<GameObject>("Prefabs/PickupModels/PickupMystery");
            var rule = new ItemDisplayRule
            {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = prefab,
                childName = "Chest",
                localScale = new Vector3(0f, 0, 0f),
                localAngles = new Vector3(0f, 0f, 0f),
                localPos = new Vector3(0, 0f, 0f)
            };
            string pickUpIconPath = "Textures/MiscIcons/texMysteryIcon";
            string pickUpModelPath = "Prefabs/PickupModels/PickupMystery";
            #endregion
            #region Tokens
            LanguageAPI.Add("ITEM_ITEMONCHAMPIONKILL_NAME", "Portable Grinder"); //model
            LanguageAPI.Add("ITEM_ITEMONCHAMPIONKILL_DESC", "<style=cIsUtility>25% chance <style=cStack>(+10% per stack)</style> for bosses to drop a random boss item when killed.</style>");
            LanguageAPI.Add("ITEM_ITEMONCHAMPIONKILL_PICKUP", "Chance for bosses to drop boss items when killed");
            LanguageAPI.Add("ITEM_ITEMONCHAMPIONKILL_LORE", "Order: \u201CPortable Grinder\u201D\r\nTracking Number: 56*****\r\nEstimated Delivery: 08\\25\\2057\\r\nShipping Method: Standard\r\nShipping Address: 836 Lane, Lab [42], Mars, \r\nShipping Details:\r\n\r\nHey man, I know you and your colleagues deal with paranormal stuff, so I thought you guys might be able to fix this. Basically, it\u2019s really funky, whenever I finish grinding something, my left hand will be holding something related to what I ground. For example I was grinding this steel box, and when I was finished a small steel box appeared in my hand. One time my curosity got the best of me, and I decided to grind a mouse. After I finished, it\u2019s tail appeared in my left hand. Anyways, I\u2019m hoping you can fix it.\r\n");

            LanguageAPI.Add("ITEM_LARGERTELEPORTERRADIUS_NAME", "Mechanical Trinket"); //model
            LanguageAPI.Add("ITEM_LARGERTELEPORTERRADIUS_DESC", "<style=cIsUtility>Increase teleporter radius by 3m</style> <style=cStack>(+3m per stack)</style>.");
            LanguageAPI.Add("ITEM_LARGERTELEPORTERRADIUS_PICKUP", "Increase teleporter radius.");

            LanguageAPI.Add("ITEM_ELITEBUFFONINTERACTION_NAME", "Quantum Core");
            LanguageAPI.Add("ITEM_ELITEBUFFONINTERACTION_DESC", "<style=cIsUtility>Become elite upon interaction</style> for 5 seconds <style=cStack>(+5 seconds per stack)</style>.");
            LanguageAPI.Add("ITEM_ELITEBUFFONINTERACTION_PICKUP", "Become elite on activating an interactable.");

            LanguageAPI.Add("ITEM_CRIPPLEONHIT_NAME", "Fabinhoru's Dagger");
            LanguageAPI.Add("ITEM_CRIPPLEONHIT_DESC", "20% Chance to <style=cIsDamage>cripple enemies</style> for 3 <style=cStack>(+3 seconds per stack)</style> seconds. ");
            LanguageAPI.Add("ITEM_CRIPPLEONHIT_PICKUP", "Chance to cripple enemies.");

            LanguageAPI.Add("ITEM_CLOAKBUFFONINTERACTION_NAME", "Fabinhoru's Cloak");
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
            LanguageAPI.Add("ITEM_BARRIERONCRIT_DESC", "Gain a <style=cIsHealing>temporary barrier</style> on critical hits for <style=cIsHealing>5 health</style> <style=cStack>(+2.5 per stack)</style>.");
            LanguageAPI.Add("ITEM_BARRIERONCRIT_PICKUP", "Gain barrier on critical hits");

            LanguageAPI.Add("ITEM_BARRIERONLEVEL_NAME", "Topaz Crown");
            LanguageAPI.Add("ITEM_BARRIERONLEVEL_DESC", "Gain a <style=cIsHealing>temporary barrier</style> on level up for <style=cIsHealing>30 health</style> <style=cStack>(+5 per stack)</style>.");
            LanguageAPI.Add("ITEM_BARRIERONLEVEL_PICKUP", "Gain barrier on level up");

            LanguageAPI.Add("ITEM_LIGHTNINGONHURT_NAME", "Lightning Eye"); //model(??)
            LanguageAPI.Add("ITEM_LIGHTNINGONHURT_DESC", "20% chance to <style=cIsDamage>strike lightning</style> on critical hits that does 350% <style=cStack>(+25% per stack)</style> damage.");
            LanguageAPI.Add("ITEM_LIGHTNINGONHURT_PICKUP", "Chance to strike lightning on critical hits.");

            LanguageAPI.Add("ITEM_EXPERIENCEONHIT_NAME", "Glass Harvester");
            LanguageAPI.Add("ITEM_EXPERIENCEONHIT_DESC", "Gain 0.5 <style=cStack>(+0.4 per stack)</style> <style=cIsUtility>experience</style> on hit.");
            LanguageAPI.Add("ITEM_EXPERIENCEONHIT_PICKUP", "Gain experience on hit");

            LanguageAPI.Add("ITEM_BETTERCRITSONLOWHEALTH_NAME", "Glass Axe");
            LanguageAPI.Add("ITEM_BETTERCRITSONLOWHEALTH_DESC", "Enemies take <style=cIsDamage>2 <style=cStack>(+2 per stack)</style> times damage from critical hits</style> when you're at low health.");
            LanguageAPI.Add("ITEM_BETTERCRITSONLOWHEALTH_PICKUP", "Guaranteed critical hits when you're at low health");

            LanguageAPI.Add("ITEM_INCREASEDJUMPHEIGHT_NAME", "Rocket Boosters");
            LanguageAPI.Add("ITEM_INCREASEDJUMPHEIGHT_DESC", "<style=Utility>Increase your base jump height</style> by 30% <style=cStack>(+30% per stack)</style>");
            LanguageAPI.Add("ITEM_INCREASEDJUMPHEIGHT_PICKUP", "Jump higher");
            #endregion
            #region ItemDefs
            ItemTag[] itemChampionOnKillTags = new ItemTag[2]
            {
                ItemTag.OnKillEffect,
                ItemTag.AIBlacklist
            };
            ItemDef itemChampionOnKillItemDef = new ItemDef //OnPlayerCharacterDeath
            {
                canRemove = true,
                descriptionToken = "ITEM_ITEMONCHAMPIONKILL_DESC",
                hidden = false,
                loreToken = "ITEM_ITEMONCHAMPIONKILL_LORE",
                name = "ItemChampionOnKill",
                nameToken = "ITEM_ITEMONCHAMPIONKILL_NAME",
                pickupIconPath = pickUpIconPath,
                pickupModelPath = "@Cloudburst:Assets/Items/Grinder/MDLGrinder.prefab",
                pickupToken = "ITEM_ITEMONCHAMPIONKILL_PICKUP",
                tags = itemChampionOnKillTags,
                tier = ItemTier.Tier3,
                unlockableName = ""
            };
            ItemTag[] largerTeleporterRadiusTags = new ItemTag[2]
{
                ItemTag.AIBlacklist,
                ItemTag.Utility
};
            ItemDef largerTeleporterRadiusItemDef = new ItemDef
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
            };
            ItemTag[] eliteBuffOnInteractionTags = new ItemTag[2]
            {
                ItemTag.AIBlacklist,
                ItemTag.Utility
            };
            ItemDef eliteBuffOnInteractionItemDef = new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_ELITEBUFFONINTERACTION_DESC",
                hidden = false,
                loreToken = "",
                name = "EliteBuffOnInteraction",
                nameToken = "ITEM_ELITEBUFFONINTERACTION_NAME",
                pickupIconPath = pickUpIconPath,
                pickupModelPath = pickUpModelPath,
                pickupToken = "ITEM_ELITEBUFFONINTERACTION_PICKUP",
                tags = eliteBuffOnInteractionTags,
                tier = ItemTier.Tier2,
                unlockableName = ""
            };
            ItemTag[] crippleOnHitTags = new ItemTag[1]
            {
                ItemTag.Damage
            };
            ItemDef crippleOnHitItemDef = new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_CRIPPLEONHIT_DESC",
                hidden = false,
                loreToken = "",
                name = "CrippleOnHit",
                nameToken = "ITEM_CRIPPLEONHIT_NAME",
                pickupIconPath = pickUpIconPath,
                pickupModelPath = "@Cloudburst:Assets/Items/FabinhoruDagger/MDLDagger.prefab",
                pickupToken = "ITEM_CRIPPLEONHIT_PICKUP",
                tags = crippleOnHitTags,
                tier = ItemTier.Tier1,
                unlockableName = ""
            };
            ItemTag[] cloakOnInteractionTags = new ItemTag[2]
            {
                ItemTag.AIBlacklist,
                ItemTag.Utility
            };
            //string unlockAble = Main.unlockAll.Value ? "CLOUDBURST_GRABORDIE_REWARD_ID" : "";
            ItemDef cloakOnInteractionItemDef = new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_CLOAKBUFFONINTERACTION_DESC",
                hidden = false,
                loreToken = "",
                name = "CloakOnInteraction",
                nameToken = "ITEM_CLOAKBUFFONINTERACTION_NAME",
                pickupIconPath = pickUpIconPath,
                pickupModelPath = pickUpModelPath,
                pickupToken = "ITEM_CLOAKBUFFONINTERACTION_PICKUP",
                tags = cloakOnInteractionTags,
                tier = ItemTier.Tier2,
                unlockableName = ""//"CLOUDBURST_GRABORDIE_REWARD_ID"
            };
            ItemTag[] itemOnLevelUpTags = new ItemTag[2]
{
                ItemTag.AIBlacklist,
                ItemTag.Utility
};
            ItemDef itemOnLevelUpItemDef = new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_ITEMONLEVELUP_DESC",
                hidden = false,
                loreToken = "",
                name = "ItemOnLevelUp",
                nameToken = "ITEM_ITEMONLEVELUP_NAME",
                pickupIconPath = pickUpIconPath,
                pickupModelPath = pickUpModelPath,
                pickupToken = "ITEM_ITEMONLEVELUP_PICKUP",
                tags = itemOnLevelUpTags,
                tier = ItemTier.Tier3,
                unlockableName = ""
            };
            ItemTag[] randomDebuffOnHitTags = new ItemTag[2]
{
                ItemTag.Utility,
                ItemTag.Damage
};
            ItemDef randomDebuffOnHitItemDef = new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_RANDOMDEBUFFONHIT_DESC",
                hidden = false,
                loreToken = "",
                name = "RandomDebuffOnHit",
                nameToken = "ITEM_RANDOMDEBUFFONHIT_NAME",
                pickupIconPath = pickUpIconPath,
                pickupModelPath = pickUpModelPath,
                pickupToken = "ITEM_RANDOMDEBUFFONHIT_PICKUP",
                tags = randomDebuffOnHitTags,
                tier = ItemTier.Tier3,
                unlockableName = ""
            };
            ItemTag[] moneyOnInteractionTags = new ItemTag[2]
{
                ItemTag.Utility,
                ItemTag.AIBlacklist
};
            ItemDef moneyOnInteractionItemDef = new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_MONEYONINTERACTION_DESC",
                hidden = false,
                loreToken = "",
                name = "MoneyOnInteraction",
                nameToken = "ITEM_MONEYONINTERACTION_NAME",
                pickupIconPath = pickUpIconPath,
                pickupModelPath = "@Cloudburst:Assets/Items/UESKeycard/MDLKeycard.prefab",
                pickupToken = "ITEM_MONEYONINTERACTION_PICKUP",
                tags = moneyOnInteractionTags,
                tier = ItemTier.Tier1,
                unlockableName = ""
            };
            ItemTag[] barrierOnCritTags = new ItemTag[2]
{
                    ItemTag.Utility,
                    ItemTag.Healing
};
            ItemDef barrierOnCritItemDef = new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_BARRIERONCRIT_DESC",
                hidden = false,
                loreToken = "",
                name = "BarrierOnCrit",
                nameToken = "ITEM_BARRIERONCRIT_NAME",
                pickupIconPath = pickUpIconPath,
                pickupModelPath = pickUpModelPath,
                pickupToken = "ITEM_BARRIERONCRIT_PICKUP",
                tags = barrierOnCritTags,
                tier = ItemTier.Tier1,
                unlockableName = ""
            };
            ItemTag[] lightningOnCritTags = new ItemTag[2]
{
                    ItemTag.Utility,
                    ItemTag.Damage
};
            ItemDef lightningOnCritItemDef = new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_LIGHTNINGONHURT_DESC",
                hidden = false,
                loreToken = "",
                name = "LightningOnHurt",
                nameToken = "ITEM_LIGHTNINGONHURT_NAME",
                pickupIconPath = pickUpIconPath,
                pickupModelPath = "@Cloudburst:Assets/Items/LightningEye/MDLLightningEye.prefab", //
                pickupToken = "ITEM_LIGHTNINGONHURT_PICKUP",
                tags = lightningOnCritTags,
                tier = ItemTier.Tier2,
                unlockableName = ""
            };
            ItemTag[] barrierOnLevelTags = new ItemTag[2]
{
                    ItemTag.Utility,
                    ItemTag.Healing
};
            ItemDef barrierOnLevelItemDef = new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_BARRIERONLEVEL_DESC",
                hidden = false,
                loreToken = "",
                name = "BarrierOnLevel",
                nameToken = "ITEM_BARRIERONLEVEL_NAME",
                pickupIconPath = pickUpIconPath,
                pickupModelPath = pickUpModelPath,
                pickupToken = "ITEM_BARRIERONLEVEL_PICKUP",
                tags = barrierOnLevelTags,
                tier = ItemTier.Tier1,
                unlockableName = ""
            };
            ItemTag[] experienceOnHitTags = new ItemTag[2]
{
                    ItemTag.Utility,
                    ItemTag.AIBlacklist
};
            ItemDef experienceOnHitItemDef = new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_EXPERIENCEONHIT_DESC",
                hidden = false,
                loreToken = "",
                name = "ExperienceOnHit",
                nameToken = "ITEM_EXPERIENCEONHIT_NAME",
                pickupIconPath = pickUpIconPath,
                pickupModelPath = pickUpModelPath,
                pickupToken = "ITEM_EXPERIENCEONHIT_PICKUP",
                tags = experienceOnHitTags,
                tier = ItemTier.Tier1,
                unlockableName = ""
            };
            ItemTag[] betterCritsOnLowHealthTags = new ItemTag[2]
            {
                    ItemTag.Damage,
                    ItemTag.AIBlacklist
            };
            ItemDef betterCritsOnLowHealthItemDef = new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_BETTERCRITSONLOWHEALTH_DESC",
                hidden = false,
                loreToken = "",
                name = "BetterCrisOnLowHealth",
                nameToken = "ITEM_BETTERCRITSONLOWHEALTH_NAME",
                pickupIconPath = pickUpIconPath,
                pickupModelPath = pickUpModelPath,
                pickupToken = "ITEM_BETTERCRITSONLOWHEALTH_PICKUP",
                tags = betterCritsOnLowHealthTags,
                tier = ItemTier.Tier3,
                unlockableName = ""
            };
            /* this is really cool!! */
            #endregion
            #region CustomItems 
            CustomItem itemChampionOnKill = new CustomItem(itemChampionOnKillItemDef, new[] { rule });
            CustomItem largerTeleporterRadius = new CustomItem(largerTeleporterRadiusItemDef, new[] { rule });
            CustomItem eliteBuffOnInteraction = new CustomItem(eliteBuffOnInteractionItemDef, new[] { rule });
            CustomItem crippleOnHit = new CustomItem(crippleOnHitItemDef, new[] { rule });
            CustomItem cloakOnInteraction = new CustomItem(cloakOnInteractionItemDef, new[] { rule });
            CustomItem itemOnLevelUp = new CustomItem(itemOnLevelUpItemDef, new[] { rule });
            CustomItem randomDebuffOnHit = new CustomItem(randomDebuffOnHitItemDef, new[] { rule });
            CustomItem moneyOnInteraction = new CustomItem(moneyOnInteractionItemDef, new[] { rule });
            CustomItem barrierOnCrit = new CustomItem(barrierOnCritItemDef, new[] { rule });
            CustomItem barrierOnLevel = new CustomItem(barrierOnLevelItemDef, new[] { rule });
            CustomItem lightningOnCrit = new CustomItem(lightningOnCritItemDef, new[] { rule });
            CustomItem experienceOnHit = new CustomItem(experienceOnHitItemDef, new[] { rule });
            CustomItem betterCritsOnLowHealth = new CustomItem(betterCritsOnLowHealthItemDef, new[] { rule });
            #endregion
            #region Set indexes  
            itemChampionOnKillIndex = ItemAPI.Add(itemChampionOnKill);
            largerTeleporterRadiusIndex = ItemAPI.Add(largerTeleporterRadius);
            eliteBuffOnInteractionIndex = ItemAPI.Add(eliteBuffOnInteraction);
            crippleOnHitIndex = ItemAPI.Add(crippleOnHit);
            cloakOnInteractionIndex = ItemAPI.Add(cloakOnInteraction);
            itemOnLevelUpIndex = ItemAPI.Add(itemOnLevelUp);
            randomDebuffOnHitIndex = ItemAPI.Add(randomDebuffOnHit);
            moneyOnInteractionIndex = ItemAPI.Add(moneyOnInteraction);
            barrierOnCritIndex = ItemAPI.Add(barrierOnCrit);
            barrierOnLevelIndex = ItemAPI.Add(barrierOnLevel);
            lightningOnCritIndex = ItemAPI.Add(lightningOnCrit);
            experienceOnHitIndex = ItemAPI.Add(experienceOnHit);
            betterCritDamageOnLowHealthIndex = ItemAPI.Add(betterCritsOnLowHealth);
            #endregion
            #region CloudCatalog
            #endregion
        }
    }
}