using R2API;
using RoR2;
using UnityEngine;

namespace CloudBurst.Items
{
    public sealed class Items
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
        #endregion
        public Items()
        {
            #region Basic
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
            LanguageAPI.Add("ITEM_ITEMONCHAMPIONKILL_NAME", "Portable Grinder");
            LanguageAPI.Add("ITEM_ITEMONCHAMPIONKILL_DESC", "<style=cIsUtility>65% chance <style=cStack>(+5% per stack)</style> for bosses to drop a random boss item when killed.</style>");
            LanguageAPI.Add("ITEM_ITEMONCHAMPIONKILL_PICKUP", "Chance for bosses to drop boss items when killed");

            LanguageAPI.Add("ITEM_LARGERTELEPORTERRADIUS_NAME", "Mechanical Trinket");
            LanguageAPI.Add("ITEM_LARGERTELEPORTERRADIUS_DESC", "<style=cIsUtility>Increase teleporter radius by 3m</style> <style=cStack>(+3m per stack)</style>.");
            LanguageAPI.Add("ITEM_LARGERTELEPORTERRADIUS_PICKUP", "Increase teleporter radius.");

            LanguageAPI.Add("ITEM_ELITEBUFFONINTERACTION_NAME", "Rechargable Quantum Core");
            LanguageAPI.Add("ITEM_ELITEBUFFONINTERACTION_DESC", "<style=cIsUtility>Become elite upon interaction</style> for 5 seconds <style=cStack>(+5 seconds per stack)</style>.");
            LanguageAPI.Add("ITEM_ELITEBUFFONINTERACTION_PICKUP", "Become elite on activating an interactable.");

            LanguageAPI.Add("ITEM_CRIPPLEONHIT_NAME", "Crippling Root");
            LanguageAPI.Add("ITEM_CRIPPLEONHIT_DESC", "20% Chance to <style=cIsDamage>cripple enemies</style> for 3 <style=cStack>(+3 seconds per stack)</style> seconds. ");
            LanguageAPI.Add("ITEM_CRIPPLEONHIT_PICKUP", "Chance to cripple enemies.");

            LanguageAPI.Add("ITEM_CLOAKBUFFONINTERACTION_NAME", "Odd Armpiece");
            LanguageAPI.Add("ITEM_CLOAKBUFFONINTERACTION_DESC", "Become <style=cIsUtility>cloaked for 2 seconds</style> <style=cStack>(+2 seconds per stack)</style> when picking up an item.");
            LanguageAPI.Add("ITEM_CLOAKBUFFONINTERACTION_PICKUP", "Become cloaked when picking up an item.");

            LanguageAPI.Add("ITEM_ITEMONLEVELUP_NAME", "Interstellar Nokia");
            LanguageAPI.Add("ITEM_ITEMONLEVELUP_DESC", "<style=cIsUtility>75% Chance to gain an item on level up</style>. <style=cStack>+5% chance to gain a green item per stack</style>");
            LanguageAPI.Add("ITEM_ITEMONLEVELUP_PICKUP", "Chance to gain an item on level up");

            LanguageAPI.Add("ITEM_RANDOMDEBUFFONHIT_LORE", "Order: \u201C[REDACTED]\u201D\r\nTracking Number: [REDACTED]\r\nEstimated Delivery: [REDACTED]\r\nShipping Method: [REDACTED]\r\nShipping Address: [REDACTED], [REDACTED]\r\nShipping Details:\r\n\r\nSecure, contain, protect.\r\n");
            LanguageAPI.Add("ITEM_RANDOMDEBUFFONHIT_NAME", "[REDACTED]");
            LanguageAPI.Add("ITEM_RANDOMDEBUFFONHIT_DESC", "[REDACTED]");
            LanguageAPI.Add("ITEM_RANDOMDEBUFFONHIT_PICKUP", "[REDACTED]");

            LanguageAPI.Add("ITEM_MONEYONINTERACTION_NAME", "Broken Keycard");
            LanguageAPI.Add("ITEM_MONEYONINTERACTION_DESC", "Gain <style=cIsUtility>10 dollars</style> <style=cStack>(+5 dollars chance per stack)</style> upon opening a chest. ");
            LanguageAPI.Add("ITEM_MONEYONINTERACTION_PICKUP", "Activating an interactable gives you money");

            LanguageAPI.Add("ITEM_BARRIERONCRIT_NAME", "Topaz Lense");
            LanguageAPI.Add("ITEM_BARRIERONCRIT_DESC", "Gain a <style=cIsHealing>temporary barrier</style> on critical hits for <style=cIsHealing>5 health</style> <style=cStack>(+2.5 per stack)</style>.");
            LanguageAPI.Add("ITEM_BARRIERONCRIT_PICKUP", "Gain barrier on critical hits");

            LanguageAPI.Add("ITEM_BARRIERONLEVEL_NAME", "Topaz Crown");
            LanguageAPI.Add("ITEM_BARRIERONLEVEL_DESC", "Gain a <style=cIsHealing>temporary barrier</style> on level up for <style=cIsHealing>30 health</style> <style=cStack>(+5 per stack)</style>.");
            LanguageAPI.Add("ITEM_BARRIERONLEVEL_PICKUP", "Gain barrier on level up");

            LanguageAPI.Add("ITEM_LIGHTNINGONHURT_NAME", "LIGHTNINGONHURT_NAME");
            LanguageAPI.Add("ITEM_LIGHTNINGONHURT_DESC", "LIGHTNINGONHURT_DESC");
            LanguageAPI.Add("ITEM_LIGHTNINGONHURT_PICKUP", "LIGHTNINGONHURT_PICKUP");
            #endregion
            #region ItemDefs
            ItemTag[] itemChampionOnKillTags = new ItemTag[2]
            {
                ItemTag.OnKillEffect,
                ItemTag.AIBlacklist
            };
            ItemDef itemChampionOnKillItemDef = new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_ITEMONCHAMPIONKILL_DESC",
                hidden = false,
                loreToken = "",
                name = "ItemChampionOnKill",
                nameToken = "ITEM_ITEMONCHAMPIONKILL_NAME",
                pickupIconPath = pickUpIconPath,
                pickupModelPath = pickUpModelPath,
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
                pickupIconPath = pickUpIconPath,
                pickupModelPath = pickUpModelPath,
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
                pickupModelPath = pickUpModelPath,
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
                unlockableName = ""
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
                descriptionToken = "ITEM_RANDOMDEBUFFONHIT_DESC",
                hidden = false,
                loreToken = "",
                name = "MoneyOnInteraction",
                nameToken = "ITEM_MONEYONINTERACTION_NAME",
                pickupIconPath = pickUpIconPath,
                pickupModelPath = pickUpModelPath,
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
            ItemTag[] lightningOnHurtTags = new ItemTag[2]
{
                    ItemTag.Utility,
                    ItemTag.Damage
};
            ItemDef lightningOnHurtItemDef = new ItemDef
            {
                canRemove = true,
                descriptionToken = "ITEM_LIGHTNINGONHURT_DESC",
                hidden = false,
                loreToken = "",
                name = "LightningOnHurt",
                nameToken = "ITEM_LIGHTNINGONHURT_NAME",
                pickupIconPath = pickUpIconPath,
                pickupModelPath = pickUpModelPath,
                pickupToken = "ITEM_LIGHTNINGONHURT_PICKUP",
                tags = barrierOnCritTags,
                tier = ItemTier.Tier1,
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
            #endregion
            #region CloudCatalog
            #endregion
        }
    }
}